using System;
using System.Collections.Generic;
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
    [SerializeField] int loadDistance = 10;

    Dictionary<Vector2Int,Level> chunkDic;
    Dictionary<Level,List<Vector2Int>> chunkDicReverse;

    float maxChunkExtendY;
    Vector2Int lastPos;
    Vector2Int playerPos;
    float playerZ;
    PlayerController player;
    void Start()
    {
        chunkDic = new ();
        chunkDicReverse = new();

        player = GameManager.Instance.Player;
        UpdatePlayerPosition();
        lastPos = playerPos;

        InitMapChunkSize();
        InitLevel();
    }

	void Update()
	{
        lastPos = playerPos;
        UpdatePlayerPosition();
		UpdateLevel();
	}

    void UpdatePlayerPosition()
    {
        var pos3D = player.transform.position + offset;
        playerPos = new Vector2Int(Mathf.RoundToInt(pos3D.x), Mathf.RoundToInt(pos3D.y));
        playerZ = pos3D.z;
    }

    void InitMapChunkSize()
    {
        foreach(var item in levelAssets)
        {
            maxChunkExtendY = Mathf.Max(item.Extends.y,maxChunkExtendY);
        }
    }

    Level CreateRandomLevel(Vector2Int createPos)
	{
        // TODO: Level Positioning + Pooling
        var level = levelAssets[Random.Range(0, levelAssets.Count)];
        var extend = level.Extends;

        // Adjust createPos (real create Pos)
        Vector3 adjustCretePos = (Vector3Int)createPos + new Vector3(0,0,playerZ);
        // row check
        if (chunkDic.TryGetValue(createPos+Vector2Int.right, out var right)) 
        {
            adjustCretePos.x = right.LevelPosition.x - right.Extends.x - extend.x;
            adjustCretePos.y = right.LevelPosition.y - right.Extends.y + extend.y;
        }
        else if (chunkDic.TryGetValue(createPos+Vector2Int.left, out var left))
        {
            adjustCretePos.x = left.LevelPosition.x + left.Extends.x + extend.x;
            adjustCretePos.y = left.LevelPosition.y - left.Extends.y + extend.y;
        }
        // col check
        if (chunkDic.TryGetValue(createPos+Vector2Int.up, out var up))
        {
            adjustCretePos.y = up.LevelPosition.y - up.Extends.y - (maxChunkExtendY * 2 - extend.y);
        }
        else if (chunkDic.TryGetValue(createPos+Vector2Int.down, out var down))
        {
            adjustCretePos.y = down.LevelPosition.y + (maxChunkExtendY * 2 - down.Extends.y) + extend.y;
        }
        
        level = Instantiate(level);
        //Test
        // level.ToggleActive(true);

        level.LevelPosition = adjustCretePos;
        var minVector2 = adjustCretePos - extend;
        var min = new Vector2Int((int)minVector2.x, (int)minVector2.y);
        var maxVector2 = adjustCretePos + extend + new Vector3(0, maxChunkExtendY * 2 - extend.y,0);
        var max = new Vector2Int((int)maxVector2.x,(int)maxVector2.y);
        for (int x = min.x; x <= max.x; x++)
        {
            for (int y = min.y; y <= max.y; y++)
            {
                var pos = new Vector2Int(x,y);
                if (chunkDic.ContainsKey(pos)) continue;
                chunkDic.Add(pos, level);

                if (chunkDicReverse.ContainsKey(level) == false)
                    chunkDicReverse.Add(level,new ());
                chunkDicReverse[level].Add(pos);
            }
        }
        return level;
	}

    void InitLevel()
    {
        for (int x = playerPos.x - loadDistance; x <= playerPos.x + loadDistance; x++)
        {
            for (int y = playerPos.y -loadDistance; y <= playerPos.y + loadDistance; y++)
            {
                var curPos = new Vector2Int(x,y);
                if (chunkDic.ContainsKey(curPos)) continue;

                CreateRandomLevel(curPos);
            }
        }
    }

    void UpdateLevel()
	{
        var diff = playerPos - lastPos;
        diff = new Vector2Int(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
        if (diff == Vector2Int.zero) return;

        for (int x = playerPos.x - loadDistance - diff.x; x <= playerPos.x + loadDistance + diff.x; x++)
        {
            for (int y = playerPos.y -loadDistance - diff.y; y <= playerPos.y + loadDistance + diff.y; y++)
            {
                var curPos = new Vector2Int(x,y);
                if (chunkDic.TryGetValue(curPos, out var chunk))
                {
                    // Enable Chunk in LoadDistance
                    chunk.ToggleActive((curPos - playerPos).sqrMagnitude <= loadDistance * loadDistance);
                }
                else
                {
                    CreateRandomLevel(curPos);
                }
            }
        }
    }
}
