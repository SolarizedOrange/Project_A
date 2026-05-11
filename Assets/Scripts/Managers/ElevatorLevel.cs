using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ElevatorLevel : Level
{
    private ElevatorLevel linkedLevel;
    public ElevatorLevel LinkedLevel
    {
        set
        {
            if (value != null)
            {
                linkedLevel = value;
                foreach (var Door in Doors)
                {
                    Door.MoveDoor(true);
                }
            }
        }
        get 
        {
            return linkedLevel;
        }
    }
    public Door[] Doors;
    [SerializeField] Transform SpawnPoint;

    public void TeleportToLinkedLevel(PlayerController player)
    {
        var pos = LinkedLevel.SpawnPoint.position;
        pos.z = player.transform.position.z;
        player.transform.position = pos;
    }
}
