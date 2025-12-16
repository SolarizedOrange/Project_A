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

    Queue<LogicalEndureNote> q;
    bool inProcess = false;
    bool success;
    int missCount;
    float marginTime = 0.1f;
    float lastActivatedTime = -10.0f;
    [SerializeField] float cooldown = 5.0f;

    public static UnityEvent EndureStartEvent = new();
    public static UnityEvent<bool> EndureEndEvent = new();
    public static UnityEvent<EndureNote> EndureNoteEvent = new();
    public static UnityEvent EndureMissEvent = new();
    
    void Start()
    {
        q = new();
    }

    public void DoDamage(HitBoxType hitBoxType, float damage)
    {
        PlayerCtrl.HP.Value -= damage;
        if (PlayerCtrl.HP.Value <= 50 && !inProcess && Time.time - lastActivatedTime > cooldown)
        {
            success = true;
            inProcess = true;
            missCount = 0;
            
            EndureStartEvent?.Invoke();
            
            AddNote(new EndureNote{StartTime = Time.time + 5.0f, Duration = 0.0f, Type = EndureNoteType.Tap});
            AddNote(new EndureNote{StartTime = Time.time + 6.0f, Duration = 1.0f, Type = EndureNoteType.Hold});
            
            StartCoroutine(StartEndureRoutine());

            Debug.Log($"START: {Time.time}");
        }
    }

    void AddNote(EndureNote note)
    {
        if (note.Type == EndureNoteType.Hold)
        {
            q.Enqueue(new LogicalEndureNote{Time = note.StartTime, Type = LogicalNoteType.HoldStart});
            q.Enqueue(new LogicalEndureNote{Time = note.StartTime + note.Duration, Type = LogicalNoteType.HoldEnd});
        }
        else
        {
            q.Enqueue(new LogicalEndureNote{Time = note.StartTime, Type = LogicalNoteType.Tap});
        }
        EndureNoteEvent?.Invoke(note);
    }

    void AddMiss()
    {
        missCount++;
        EndureMissEvent?.Invoke();
        if (missCount >= 5)
        {
            q.Clear();
            EndureEndEvent?.Invoke(false);
            inProcess = false;
            lastActivatedTime = Time.time;
        }
    }

    IEnumerator StartEndureRoutine()
    {
        while (q.Count > 0)
        {
            if (q.Peek().Time + marginTime < Time.time)
            {
                Debug.Log($"MISS: {q.Peek().Time - Time.time} Type: {q.Peek().Type}");
                if (q.Peek().Type == LogicalNoteType.HoldStart)
                {
                    q.Dequeue();
                }
                q.Dequeue();
                AddMiss();
            }
            yield return null;
        }
        if (success)
        {
            // Heal
            EndureEndEvent?.Invoke(true);
            lastActivatedTime = Time.time;
            inProcess = false;
        }
    }

    void OnEndure(InputValue value)
    {
        if (q.Count > 0)
        {
            if (Mathf.Abs(q.Peek().Time - Time.time) < marginTime)
            {
                if (value.isPressed == (q.Peek().Type != LogicalNoteType.HoldEnd))
                {
                    Debug.Log("HIT");
                    q.Dequeue();
                }
            }
            else if (value.isPressed)
            {
                Debug.Log($"PRESSMISS: {q.Peek().Time - Time.time} Type: {q.Peek().Type}");
                if (q.Peek().Type == LogicalNoteType.HoldStart)
                {
                    q.Dequeue();
                }
                q.Dequeue();
                AddMiss();
            }
        }
    }
}
