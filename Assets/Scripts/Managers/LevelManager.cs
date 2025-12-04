using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : ManagerBase<LevelManager>
{
    [Header("Level Settings")]
    [SerializeField] List<GameObject> levelAssets;

    [Header("Chunks Settings")]
    [SerializeField] float createDistance = 100;
    [SerializeField] float loadDistance = 10;

    LinkedList<Level> chunk_x = new ();
    // LinkedList<Level> chunk_y = new ();

    LinkedListNode<Level> rightActiveNode = null;
    LinkedListNode<Level> leftActiveNode = null;
    Vector3 playerPos;
    GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;
        playerPos = gameManager.Player.transform.position + gameManager.PlayerSpawnOffset;
        AddLevel();
    }

	void Update()
	{
        playerPos = gameManager.Player.transform.position + gameManager.PlayerSpawnOffset;

		UpdateLevelState();
        AddLevel();
	}

	void AddLevel()
	{
        // Add Init Level
        if (chunk_x.Count < 1)
		{
            var level = CreateRandomLevel(null, Vector3.zero);
            level.ToggleActive(true);
            chunk_x.AddLast(level);

            rightActiveNode = chunk_x.Last;;
            leftActiveNode = chunk_x.First;
		}

        var right = chunk_x.Last.Value;
        float dist = Mathf.Abs(playerPos.x - (right.LevelPosition.x + right.LevelBounds.bounds.extents.x));
        if (dist < createDistance)
        {
            var newLevel = CreateRandomLevel(right,Vector3.right);
            chunk_x.AddLast(newLevel);
        }

        var left = chunk_x.First.Value;
        dist = Mathf.Abs(playerPos.x - (left.LevelPosition.x - left.LevelBounds.bounds.extents.x));
        if (dist < createDistance)
        {
            var newLevel = CreateRandomLevel(left, -Vector3.right);
            chunk_x.AddFirst(newLevel);
        }
    }

    Level CreateRandomLevel(Level lastLevel,Vector3 dir)
	{
        // TODO: Level Polling
        var pick = Instantiate(levelAssets[Random.Range(0, levelAssets.Count)]);
        var level = pick.GetComponentInChildren<Level>();
        var pos = playerPos;

        if (lastLevel != null)
            pos = lastLevel.LevelPosition + Vector3.Scale(lastLevel.LevelBounds.bounds.extents, dir) + Vector3.Scale(level.LevelBounds.bounds.extents, dir);
        
        level.LevelPosition = pos;
        return level;
	}

    void UpdateLevelState()
	{
        if (chunk_x.Count < 1) return;

        var curRight = rightActiveNode.Value;
        float dist = Mathf.Abs(playerPos.x - (curRight.LevelPosition.x + curRight.LevelBounds.bounds.extents.x));
        if(dist > loadDistance)
		{
            curRight.ToggleActive(false);
            rightActiveNode = rightActiveNode.Previous;
		}
        else
        {
            var nextRight = rightActiveNode.Next;
            if (nextRight != null)
            {
                var nextDist = Mathf.Abs(playerPos.x - (nextRight.Value.LevelPosition.x + nextRight.Value.LevelBounds.bounds.extents.x));
                if (nextDist < loadDistance)
                {
                    nextRight.Value.ToggleActive(true);
                    rightActiveNode = nextRight;
                }
            }
        }

        var curLeft = leftActiveNode.Value;
        dist = Mathf.Abs(playerPos.x - (curLeft.LevelPosition.x - curLeft.LevelBounds.bounds.extents.x));
        if(dist > loadDistance)
		{
            curLeft.ToggleActive(false);
            leftActiveNode = leftActiveNode.Next;
		}
        else if (dist < loadDistance)
        {
            var nextLeft = leftActiveNode.Previous;
            if (nextLeft != null)
            {
                var nextDist = Mathf.Abs(playerPos.x - (nextLeft.Value.LevelPosition.x - nextLeft.Value.LevelBounds.bounds.extents.x));
                if (nextDist < loadDistance)
                {
                    nextLeft.Value.ToggleActive(true);
                    leftActiveNode = nextLeft;
                }
            }
        }
	}
}
