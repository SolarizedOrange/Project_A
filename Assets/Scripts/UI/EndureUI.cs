using System.Collections.Generic;
using UnityEngine;

public class EndureUI : MonoBehaviour
{
    PlayerController player;
    [SerializeField] EndureNoteUI prefab;
    Queue<EndureNoteUI> queue = new();
    Camera mainCamera;

    void Start()
    {
        player = GameManager.Instance.Player;
        mainCamera = Camera.main;
    }
    void OnEnable()
    {
        PlayerDamage.EndureEndEvent.AddListener(EndNoteUI);
        PlayerDamage.EndureNoteEvent.AddListener(AddNoteUI);
        PlayerDamage.EndureMissEvent.AddListener(RemoveNoteUI);

        // Melee Attack
        PlayerCombat.MeleeNoteEvent.AddListener(AddNoteUI);
        PlayerCombat.MeleeEndEvent.AddListener(EndNoteUI);
    }

    void OnDisable()
    {
        PlayerDamage.EndureEndEvent.RemoveListener(EndNoteUI);
        PlayerDamage.EndureNoteEvent.RemoveListener(AddNoteUI);
        PlayerDamage.EndureMissEvent.RemoveListener(RemoveNoteUI);

        // Melee Attack
        PlayerCombat.MeleeNoteEvent.RemoveListener(AddNoteUI);
        PlayerCombat.MeleeEndEvent.RemoveListener(EndNoteUI);
    }

    void AddNoteUI(EndureNote note)
    {
        var noteUI = Instantiate(prefab, transform);
        noteUI.Init(note);
        queue.Enqueue(noteUI);
    }

    void RemoveNoteUI()
    {
        if (queue.Count > 0)
        {
            Destroy(queue.Dequeue().gameObject);
        }
    }

    void ClearNoteUI()
    {
        while (queue.Count > 0)
        {
            Destroy(queue.Dequeue().gameObject);
        }
    }

    void EndNoteUI(bool success)
    {
        ClearNoteUI();
    }

    void FixedUpdate()
    {
        if (queue.Count > 0)
        {
            var noteUI = queue.Peek();
            
            transform.position = mainCamera.WorldToScreenPoint(player.transform.position);

            if (Time.time - noteUI.EndTime > 3.0f)
            {
                RemoveNoteUI();
            }
            // else if (Time.time - queue.Peek().StartTime > 0.0f)
            // {
            //     queue.Peek().Hit();
            // }
        }
    }

}
