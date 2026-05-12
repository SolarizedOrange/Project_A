using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : ManagerBase<LevelManager>
{
    [Header("Level Settings")]
    [SerializeField] List<Level> levelAssets;

    [Header("Chunks Settings")]
    // [SerializeField] float createDistance = 100;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector2 margin;
    [SerializeField] Vector2Int loadDistance = new Vector2Int(10, 10);

    Dictionary<Vector2Int,Level> chunkDic;
    // Dictionary<Level,List<Vector2Int>> chunkDicReverse;
    public List<ElevatorLevel> unlinkedElevatorLevels;

    float maxChunkExtendY;
    Vector2Int lastPos;
    Level lastLevel;
    Level curLevel;
    Vector2Int playerPos;
    float playerZ;
    PlayerController player;
    Vector2Int[] dirs = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    void Start()
    {
        chunkDic = new ();
        // chunkDicReverse = new();
        unlinkedElevatorLevels = new();

        player = GameManager.Instance.Player;
        playerZ = (player.transform.position + offset).z;
        UpdatePlayerPosition();

        InitMapChunkSize();
        InitLevel();
    }

    void InitMapChunkSize()
    {
        foreach(var item in levelAssets)
        {
            maxChunkExtendY = Mathf.Max(item.Extends.y,maxChunkExtendY);
        }
    }
    void InitLevel()
    {
        for (int x = playerPos.x - loadDistance.x; x <= playerPos.x + loadDistance.x; x++)
        {
            for (int y = playerPos.y - loadDistance.y; y <= playerPos.y + loadDistance.y; y++)
            {
                var curPos = new Vector2Int(x,y);
                if (chunkDic.ContainsKey(curPos)) continue;

                CreateRandomLevel(curPos);
            }
        }

        // fix player pos fix
        if (chunkDic.TryGetValue(playerPos, out var curLevel))
        {
            var pos = player.transform.position;
            player.transform.position = new Vector3(pos.x,(curLevel.LevelCenter - curLevel.Extends + offset).y,pos.z);
        }
    }

	void Update()
	{
        UpdatePlayerPosition();
        UpdateElevator();

		UpdateLevel();
        LinkRandomElevatorLevel();
	}

    void UpdatePlayerPosition()
    {
        lastPos = playerPos;
        lastLevel = curLevel;
        var pos3D = player.transform.position + offset;
        playerPos = new Vector2Int(Mathf.RoundToInt(pos3D.x), Mathf.RoundToInt(pos3D.y));
        if (chunkDic.TryGetValue(playerPos, out curLevel))
        {
            player.transform.SetParent(curLevel.transform);
        }
    }

    void UpdateElevator()
    {
        if (curLevel == lastLevel) return;
        if (curLevel is ElevatorLevel elevator)
        {
            elevator.TeleportToLinkedLevel(player);
            UpdatePlayerPosition();
            lastLevel = curLevel;
        }
    }


    Level CreateRandomLevel(Vector2Int createPos)
	{
        // TODO: Level Positioning + Pooling
        var isIncludeElevator = true;
        Level level;

        // add neighbor chunks
        var neighbors = new Dictionary<Vector2Int, Level>();
        var directions = new Vector2Int[] 
        {
            Vector2Int.left, Vector2Int.right,
            Vector2Int.up, Vector2Int.down
        };
        foreach (var dir in directions)
        {
            if (chunkDic.TryGetValue(createPos + dir, out var chunk))
            {
                neighbors.Add(dir, chunk);
                if (chunk is ElevatorLevel || createPos == playerPos)
                    isIncludeElevator = false;
            }
        }

        if (isIncludeElevator)
        {
            level = levelAssets[Random.Range(0, levelAssets.Count)];
        }
        else
        {
            level = levelAssets.Where(x => x is not ElevatorLevel)
                                .OrderBy(r => Random.value)
                                .FirstOrDefault();
        }

        var extend = level.Extends;

        // Adjust createPos (real create Pos)
        Vector3 adjustCretePos = (Vector3Int)createPos + new Vector3(0,0,playerZ);

        bool isCalculateY = false;
        foreach (var item in neighbors)
        {
            var key = item.Key;
            var value = item.Value;
            // col check
            if (key.y != 0)
            {
                if (isCalculateY) continue;
                adjustCretePos.y = value.LevelCenter.y - value.Extends.y + extend.y + -key.y * (maxChunkExtendY * 2 + margin.y);
                isCalculateY = true;
            }
            // row check
            else if (key.x != 0)
            {
                adjustCretePos.x = value.LevelCenter.x - key.x * (value.Extends.x + extend.x + margin.x);
                adjustCretePos.y = value.LevelCenter.y - value.Extends.y + extend.y;
            }
        }
        
        level = Instantiate(level);

        level.LevelCenter = adjustCretePos;
        var minVector2 = adjustCretePos - extend;
        var min = new Vector2Int(Mathf.CeilToInt(minVector2.x), Mathf.CeilToInt(minVector2.y));
        var maxVector2 = adjustCretePos + extend + new Vector3(0, 2*(maxChunkExtendY - extend.y) ,0);
        var max = new Vector2Int(Mathf.CeilToInt(maxVector2.x),Mathf.CeilToInt(maxVector2.y));
        for (int x = min.x; x <= max.x; x++)
        {
            for (int y = min.y; y <= max.y; y++)
            {
                var pos = new Vector2Int(x,y);
                if (chunkDic.ContainsKey(pos)) continue;
                chunkDic.Add(pos, level);

                if (level is ElevatorLevel elevatorLevel)
                {
                    if (unlinkedElevatorLevels.Contains(elevatorLevel) == false)
                        unlinkedElevatorLevels.Add(elevatorLevel);
                }
            }
        }
        return level;
	}

    void UpdateLevel()
    {
        var diff = playerPos - lastPos;
        diff = new Vector2Int(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
        if (diff == Vector2Int.zero) return;

        var visited = new HashSet<Vector2Int>();
        var queue = new Queue<Vector2Int>();

        queue.Enqueue(playerPos);
        visited.Add(playerPos);

        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();

            // Skip if the chunk is outside the load distance (with margin)
            if (Mathf.Abs(cur.x - playerPos.x) > loadDistance.x + diff.x ||
                Mathf.Abs(cur.y - playerPos.y) > loadDistance.y + diff.y)
                continue;

            if (chunkDic.TryGetValue(cur, out var chunk))
            {
                var active = Mathf.Abs((cur - playerPos).x) <= loadDistance.x &&
                            Mathf.Abs((cur - playerPos).y) <= loadDistance.y;
                chunk.ToggleActive(active);
            }
            else
            {
                CreateRandomLevel(cur);
            }

            // Enqueue neighboring chunks
            foreach (var dir in dirs)
            {
                var next = cur + dir;
                if (!visited.Contains(next) &&
                    Mathf.Abs(next.x - playerPos.x) <= loadDistance.x + diff.x &&
                    Mathf.Abs(next.y - playerPos.y) <= loadDistance.y + diff.y)
                {
                    visited.Add(next);
                    queue.Enqueue(next);
                }
            }
        }
    }
    
    void LinkRandomElevatorLevel()
    {
        while(unlinkedElevatorLevels.Count >= 2)
        {
            var r = Random.Range(0, unlinkedElevatorLevels.Count);
            var pick1 = unlinkedElevatorLevels[r];
            unlinkedElevatorLevels[r] = unlinkedElevatorLevels[unlinkedElevatorLevels.Count - 1];
            unlinkedElevatorLevels.RemoveAt(unlinkedElevatorLevels.Count - 1);

            r = Random.Range(0, unlinkedElevatorLevels.Count);
            var pick2 = unlinkedElevatorLevels[r];
            unlinkedElevatorLevels[r] = unlinkedElevatorLevels[unlinkedElevatorLevels.Count - 1];
            unlinkedElevatorLevels.RemoveAt(unlinkedElevatorLevels.Count - 1);

            pick1.LinkedLevel = pick2;
            pick2.LinkedLevel = pick1;
        }
    }
}
