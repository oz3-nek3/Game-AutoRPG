using UnityEngine;

// タイルのインターフェース
public interface ITile
{
    TileType Type { get; }
}

// 列挙型はUI表示や判定用として残しておきます
public enum TileType
{
    Floor,
    Wall,
    UpStairs,
    DownStairs,
    Exit,
    Treasure
}

// ------------------------------------
// 以下、各タイルの具象クラス
// ------------------------------------

[System.Serializable]
public class FloorTile : ITile
{
    public TileType Type => TileType.Floor;
}

[System.Serializable]
public class WallTile : ITile
{
    public TileType Type => TileType.Wall;
}

[System.Serializable]
public class ExitTile : ITile
{
    public TileType Type => TileType.Exit;
}

[System.Serializable]
public class UpStairsTile : ITile
{
    public TileType Type => TileType.UpStairs;
    
    // 上の階のどの座標に繋がっているか
    public Vector2Int connectedPosition;
}

[System.Serializable]
public class DownStairsTile : ITile
{
    public TileType Type => TileType.DownStairs;

    // 下の階のどの座標に繋がっているか
    public Vector2Int connectedPosition;
}

[System.Serializable]
public class TreasureTile : ITile
{
    public TileType Type => TileType.Treasure;

    // 宝箱の中身（例：アイテムID、ゴールド量など）
    public string itemID = "Potion";
    public int amount = 1;
}