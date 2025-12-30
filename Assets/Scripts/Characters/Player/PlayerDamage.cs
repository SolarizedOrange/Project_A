using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class EndureNote
{
    public float StartTime;
    public float Duration;
    public EndureNoteType Type;
}

public class PlayerDamage: PlayerComponent
{
    [SerializeField] float cooldown = 5.0f;
    [SerializeField] int missTolerance = 5;

    bool inProcess = false;
    bool invincible = false;
    bool success;
    int missCount;
    int dodgeCount = 0;
    int currentHPSection = 1;
    float marginTime = 0.1f;
    float lastActivatedTime = -10.0f;
    float[] sections = {1.0f, 0.75f, 0.5f, 0.25f, 0.0f};

    // Class whose only use is for handling endure mechanics.
    // This class should only be used within PlayerDamage class.
    struct LogicalEndureNote
    {
        public float Time;
        public LogicalNoteType Type;
    }
    public enum LogicalNoteType
    {
        Tap, HoldStart, HoldEnd
    }

    Queue<LogicalEndureNote> queue = new();

    public static UnityEvent EndureStartEvent = new();
    public static UnityEvent<bool> EndureEndEvent = new();
    public static UnityEvent<EndureNote> EndureNoteEvent = new();
    public static UnityEvent EndureMissEvent = new();

    public void UpdateDamage()
    {
        if (!inProcess && currentHPSection > 1 && Time.time > lastActivatedTime + cooldown)
        {
            StartCoroutine(StartEndureRoutine());
        }
    }
    public void DoDamage(HitBoxType hitBoxType, float damage)
    {
        if (!invincible)
        {            
            // 플레이어가 버티기 모드가 아닐 때는 모든 데미지 적용,
            // 플레이어가 버티기 모드일 때는 데미지의 1/10 적용,
            // 하지만 currentHPSection을 넘어가는 피해를 받을 시 더 이상 체력이 깎이지 않음.

            damage = Mathf.Max(0.0f, damage - (PlayerCtrl.HasArmor.Value ? 5.0f : 0.0f));

            PlayerCtrl.HP.Value = Mathf.Max(
                PlayerCtrl.HP.Value - damage * (inProcess ? 0.1f : 1.0f),
                PlayerCtrl.Stat.Hp * sections[currentHPSection]
            );
            if (!inProcess)
            {
                CheckHP();
            }
        }
    }

    void CheckHP()
    {
        if (PlayerCtrl.HP.Value <= PlayerCtrl.Stat.Hp * sections[currentHPSection])
        {
            for (int i = currentHPSection + 1; i < sections.Length; i++)
            {
                if (PlayerCtrl.HP.Value > PlayerCtrl.Stat.Hp * sections[i])
                {
                    currentHPSection = i;
                    break;
                }
            }

            if (PlayerCtrl.HasArmor.Value && currentHPSection == (sections.Length - 1))
            {
                Debug.Log("ARMOR");
                PlayerCtrl.HasArmor.Value = false;

                currentHPSection = Mathf.Max(1, currentHPSection - 2);
                PlayerCtrl.HP.Value = PlayerCtrl.Stat.Hp * sections[currentHPSection - 1];
            }
            StartCoroutine(InvincibleRoutine());
        }
    }

    void GenerateNotes()
    {
        int noteCount = 5 + Mathf.Min(10, dodgeCount * 2);
        float noteTime = 1.5f;

        while (noteCount > 0)
        {
            int num;
            switch(Random.Range(0, 4))
            {
                case 0:
                    num = Mathf.Min(Random.Range(2, 5), noteCount);
                    for (int i = 0; i < num; i++)
                    {
                        AddNote(new EndureNote{StartTime = Time.time + noteTime, Duration = 0.0f, Type = EndureNoteType.Tap});
                        AddNote(new EndureNote{StartTime = Time.time + noteTime + 0.25f, Duration = 0.0f, Type = EndureNoteType.Tap});
                        noteTime += 1.0f;
                        noteCount -= 1;
                    }
                    break;
                case 1:
                    num = Mathf.Min(Random.Range(1, 3), noteCount);
                    for (int i = 0; i < num; i++)
                    {
                        AddNote(new EndureNote{StartTime = Time.time + noteTime, Duration = 0.25f, Type = EndureNoteType.Hold});
                        noteTime += 0.5f;
                        noteCount -= 1;
                    }
                    break;
                case 2:
                    num = Mathf.Min(Random.Range(1, 3), noteCount);
                    for (int i = 0; i < num; i++)
                    {
                        AddNote(new EndureNote{StartTime = Time.time + noteTime, Duration = 0.5f, Type = EndureNoteType.Hold});
                        noteTime += 0.75f;
                        noteCount -= 1;
                    }
                    break;
                case 3:
                    num = Mathf.Min(Random.Range(1, 3), noteCount);
                    for (int i = 0; i < num; i++)
                    {
                        AddNote(new EndureNote{StartTime = Time.time + noteTime, Duration = 1.0f, Type = EndureNoteType.Hold});
                        noteTime += 1.5f;
                        noteCount -= 1;
                    }
                    break;
            }
        }

        Debug.Log($"START: {Time.time}");
    }

    void AddNote(EndureNote note)
    {
        if (note.Type == EndureNoteType.Hold)
        {
            queue.Enqueue(new LogicalEndureNote{Time = note.StartTime, Type = LogicalNoteType.HoldStart});
            queue.Enqueue(new LogicalEndureNote{Time = note.StartTime + note.Duration, Type = LogicalNoteType.HoldEnd});
        }
        else
        {
            queue.Enqueue(new LogicalEndureNote{Time = note.StartTime, Type = LogicalNoteType.Tap});
        }
        EndureNoteEvent?.Invoke(note);
    }

    void AddMiss()
    {
        missCount++;
        EndureMissEvent?.Invoke();
        if (missCount >= missTolerance) // Fail
        {
            queue.Clear();
            success = false;
            EndureEndEvent?.Invoke(false);
        }
    }

    IEnumerator StartEndureRoutine()
    {
        EndureStartEvent?.Invoke();

        success = true;
        inProcess = true;
        missCount = 0;

        GenerateNotes();

        while (queue.Count > 0)
        {
            if (queue.Peek().Time + marginTime < Time.time)
            {
                Debug.Log($"MISS: {queue.Peek().Time - Time.time} Type: {queue.Peek().Type}");
                if (queue.Peek().Type == LogicalNoteType.HoldStart)
                {
                    queue.Dequeue();
                }
                queue.Dequeue();
                AddMiss();
            }
            yield return null;
        }

        lastActivatedTime = Time.time;
        inProcess = false;

        if (success)
        {
            // Heal
            EndureEndEvent?.Invoke(true);
            currentHPSection--;
            PlayerCtrl.HP.Value = PlayerCtrl.Stat.Hp * sections[currentHPSection - 1];
            dodgeCount++;
        }
    }
    IEnumerator InvincibleRoutine()
    {
        invincible = true;
        yield return new WaitForEndOfFrame();
        invincible = false;
    }

    void OnEndure(InputValue value)
    {
        if (queue.Count > 0)
        {
            if (Mathf.Abs(queue.Peek().Time - Time.time) < marginTime)
            {
                if (value.isPressed == (queue.Peek().Type != LogicalNoteType.HoldEnd))
                {
                    Debug.Log("HIT");
                    queue.Dequeue();
                }
            }
            else if (value.isPressed || (queue.Peek().Type == LogicalNoteType.HoldEnd))
            {
                Debug.Log($"PRESSMISS: {queue.Peek().Time - Time.time} Type: {queue.Peek().Type}");
                if (queue.Peek().Type == LogicalNoteType.HoldStart)
                {
                    queue.Dequeue();
                }
                queue.Dequeue();
                AddMiss();
            }
        }
    }
}
