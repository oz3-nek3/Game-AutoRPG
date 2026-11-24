using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class FloorData
{
    public int floorLevel;
    public int width;
    public int height;

    // SerializeReferenceを使うことで、ITileを継承した異なるクラスを混在して保存できる
    [HideInInspector, SerializeReference]
    public ITile[] tiles;

    public FloorData(int level, int w, int h)
    {
        floorLevel = level;
        width = w;
        height = h;
        tiles = new ITile[width * height];
        
        // 初期状態は全てFloorTileで埋める
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new FloorTile();
        }
    }

    public ITile GetTile(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return new WallTile(); // 範囲外は壁扱い
        return tiles[y * width + x];
    }

    public void SetTile(int x, int y, ITile tile)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return;
        tiles[y * width + x] = tile;
    }
}

[CreateAssetMenu(fileName = "NewDungeonData", menuName = "CustomRPG/DungeonData")]
public class DungeonData : ScriptableObject
{
    public List<FloorData> floors = new List<FloorData>();

    public void Initialize(int w, int h)
    {
        floors.Clear();
        floors.Add(new FloorData(0, w, h));
    }

    public FloorData GetFloorByLevel(int level)
    {
        return floors.FirstOrDefault(f => f.floorLevel == level);
    }

    public FloorData AddFloorAbove()
    {
        if (floors.Count == 0) return null;
        int maxLevel = floors.Max(f => f.floorLevel);
        var refFloor = GetFloorByLevel(maxLevel);
        var newFloor = new FloorData(maxLevel + 1, refFloor.width, refFloor.height);
        floors.Add(newFloor);
        SortFloors();
        return newFloor;
    }

    public FloorData AddFloorBelow()
    {
        if (floors.Count == 0) return null;
        int minLevel = floors.Min(f => f.floorLevel);
        var refFloor = GetFloorByLevel(minLevel);
        var newFloor = new FloorData(minLevel - 1, refFloor.width, refFloor.height);
        floors.Add(newFloor);
        SortFloors();
        return newFloor;
    }

    public void RemoveFloor(FloorData floor)
    {
        if (floors.Count > 1)
        {
            floors.Remove(floor);
        }
    }

    public void SortFloors()
    {
        floors = floors.OrderByDescending(f => f.floorLevel).ToList();
    }
}