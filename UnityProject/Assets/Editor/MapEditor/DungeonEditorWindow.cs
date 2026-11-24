using UnityEngine;
using UnityEditor;
using System.IO;

public class DungeonEditorWindow : EditorWindow
{
    private DungeonData targetData;
    private TileType selectedTileType = TileType.Floor;
    private int newWidth = 10;
    private int newHeight = 10;
    
    private int currentFloorIndex = 0;
    private Vector2 scrollPosition;
    private const float TileSize = 30f;

    // エディタ上で個別のタイル設定を保持するための変数
    private Vector2Int selectedTilePos = new Vector2Int(-1, -1);

    private string newFileName = "NewDungeonData";
    private const string SaveFolderPath = "Assets/Addressables/DungeonData";

    [MenuItem("MyGame/Dungeon/Dungeon Editor")]
    public static void ShowWindow()
    {
        GetWindow<DungeonEditorWindow>("Dungeon Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("ダンジョンエディタ", EditorStyles.boldLabel);

        DrawCreateNewSection();
        EditorGUILayout.Space();

        targetData = (DungeonData)EditorGUILayout.ObjectField("編集するデータ", targetData, typeof(DungeonData), false);

        if (targetData == null || targetData.floors == null || targetData.floors.Count == 0)
        {
            EditorGUILayout.HelpBox("データを新規作成するか、既存のデータをセットしてください。", MessageType.Info);
            return;
        }

        EditorGUILayout.Space();
        
        GUILayout.BeginHorizontal();
        
        // 左側：マップ描画エリア
        GUILayout.BeginVertical(GUILayout.Width(position.width * 0.7f));
        DrawFloorManagement();
        EditorGUILayout.Space();
        GUILayout.Label("ペンの種類:", EditorStyles.boldLabel);
        selectedTileType = (TileType)GUILayout.Toolbar((int)selectedTileType, System.Enum.GetNames(typeof(TileType)));
        EditorGUILayout.Space();
        DrawGridAndHandleInput();
        GUILayout.EndVertical();

        // 右側：タイル詳細編集エリア
        GUILayout.BeginVertical("box", GUILayout.Width(position.width * 0.28f));
        DrawTilePropertiesPanel();
        GUILayout.EndVertical();
        
        GUILayout.EndHorizontal();
    }

    // (DrawFloorManagement, GetFloorName, CreateNewDungeonData, DrawCreateNewSection は変更なしのため省略せずにそのまま記述してください)
    #region Unchanged Methods
    private void DrawFloorManagement()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("階層（フロア）管理", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        for (int i = 0; i < targetData.floors.Count; i++)
        {
            FloorData floor = targetData.floors[i];
            string floorName = GetFloorName(floor.floorLevel);
            GUI.backgroundColor = (currentFloorIndex == i) ? Color.cyan : Color.white;
            if (GUILayout.Button(floorName, GUILayout.Width(50)))
            {
                currentFloorIndex = i;
                selectedTilePos = new Vector2Int(-1, -1); // フロア移動時に選択解除
            }
        }
        GUI.backgroundColor = Color.white;
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("↑ 上の階を追加"))
        {
            var addedFloor = targetData.AddFloorAbove();
            currentFloorIndex = targetData.floors.IndexOf(addedFloor);
            EditorUtility.SetDirty(targetData);
        }
        if (GUILayout.Button("↓ 下の階を追加"))
        {
            var addedFloor = targetData.AddFloorBelow();
            currentFloorIndex = targetData.floors.IndexOf(addedFloor);
            EditorUtility.SetDirty(targetData);
        }
        GUI.backgroundColor = new Color(1f, 0.5f, 0.5f);
        if (GUILayout.Button("× 現在のフロアを削除") && targetData.floors.Count > 1)
        {
            targetData.RemoveFloor(targetData.floors[currentFloorIndex]);
            currentFloorIndex = Mathf.Clamp(currentFloorIndex, 0, targetData.floors.Count - 1);
            EditorUtility.SetDirty(targetData);
        }
        GUI.backgroundColor = Color.white;
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private string GetFloorName(int level)
    {
        if (level >= 0) return $"{level + 1}F";
        return $"B{Mathf.Abs(level)}F";
    }

    private void DrawCreateNewSection()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("新規データ作成", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        newWidth = EditorGUILayout.IntField("初期Width", newWidth);
        newHeight = EditorGUILayout.IntField("初期Height", newHeight);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        newFileName = EditorGUILayout.TextField("ファイル名", newFileName);
        if (GUILayout.Button("新規作成", GUILayout.Width(100)))
        {
            CreateNewDungeonData();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void CreateNewDungeonData()
    {
        if (!Directory.Exists(SaveFolderPath)) Directory.CreateDirectory(SaveFolderPath);
        string path = $"{SaveFolderPath}/{newFileName}.asset";
        path = AssetDatabase.GenerateUniqueAssetPath(path);
        DungeonData newData = ScriptableObject.CreateInstance<DungeonData>();
        newData.Initialize(newWidth, newHeight);
        AssetDatabase.CreateAsset(newData, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        targetData = newData;
        currentFloorIndex = 0;
    }
    #endregion

    private void DrawGridAndHandleInput()
    {
        if (currentFloorIndex >= targetData.floors.Count) return;

        FloorData currentFloor = targetData.floors[currentFloorIndex];
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        Rect gridRect = GUILayoutUtility.GetRect(currentFloor.width * TileSize, currentFloor.height * TileSize);

        if (Event.current.type == EventType.Repaint)
        {
            for (int y = 0; y < currentFloor.height; y++)
            {
                for (int x = 0; x < currentFloor.width; x++)
                {
                    ITile tile = currentFloor.GetTile(x, y);
                    float drawY = gridRect.y + (currentFloor.height - 1 - y) * TileSize;
                    float drawX = gridRect.x + x * TileSize;
                    
                    Rect tileRect = new Rect(drawX, drawY, TileSize, TileSize);
                    
                    // 選択中のタイルは枠を強調
                    if (selectedTilePos.x == x && selectedTilePos.y == y)
                    {
                        EditorGUI.DrawRect(new Rect(tileRect.x - 2, tileRect.y - 2, TileSize + 4, TileSize + 4), Color.magenta);
                    }

                    EditorGUI.DrawRect(tileRect, GetTileColor(tile.Type));
                    GUI.Box(tileRect, GUIContent.none);
                }
            }
        }

        Event e = Event.current;
        if (gridRect.Contains(e.mousePosition))
        {
            int x = (int)((e.mousePosition.x - gridRect.x) / TileSize);
            int y = currentFloor.height - 1 - (int)((e.mousePosition.y - gridRect.y) / TileSize);

            // 左クリック＆ドラッグで塗る
            if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
            {
                ITile oldTile = currentFloor.GetTile(x, y);

                if (oldTile.Type != selectedTileType)
                {
                    ITile newTile = CreateTileInstance(selectedTileType, x, y);
                    currentFloor.SetTile(x, y, newTile);
                    HandleStairsSync(currentFloor, x, y, oldTile.Type, selectedTileType);
                    EditorUtility.SetDirty(targetData);
                    Repaint();
                }
                e.Use();
            }
            // 右クリックでタイルのプロパティを選択
            else if (e.type == EventType.MouseDown && e.button == 1)
            {
                selectedTilePos = new Vector2Int(x, y);
                Repaint();
                e.Use();
            }
        }

        EditorGUILayout.EndScrollView();
    }

    // 選択されたタイルのプロパティ編集パネル
    private void DrawTilePropertiesPanel()
    {
        GUILayout.Label("タイル詳細 (右クリック選択)", EditorStyles.boldLabel);

        if (currentFloorIndex >= targetData.floors.Count || selectedTilePos.x < 0)
        {
            GUILayout.Label("タイルが選択されていません");
            return;
        }

        FloorData currentFloor = targetData.floors[currentFloorIndex];
        ITile selectedTile = currentFloor.GetTile(selectedTilePos.x, selectedTilePos.y);

        GUILayout.Label($"座標: ({selectedTilePos.x}, {selectedTilePos.y})");
        GUILayout.Label($"タイプ: {selectedTile.Type}");

        EditorGUILayout.Space();

        // クラスごとの専用プロパティを描画
        if (selectedTile is TreasureTile treasure)
        {
            treasure.itemID = EditorGUILayout.TextField("Item ID", treasure.itemID);
            treasure.amount = EditorGUILayout.IntField("Amount", treasure.amount);
            if (GUI.changed) EditorUtility.SetDirty(targetData);
        }
        else if (selectedTile is UpStairsTile upStairs)
        {
            GUILayout.Label($"接続先: 上の階の {upStairs.connectedPosition}");
        }
        else if (selectedTile is DownStairsTile downStairs)
        {
            GUILayout.Label($"接続先: 下の階の {downStairs.connectedPosition}");
        }
    }

    // Enumから対応するクラスのインスタンスを生成する
    private ITile CreateTileInstance(TileType type, int x, int y)
    {
        switch (type)
        {
            case TileType.Floor: return new FloorTile();
            case TileType.Wall: return new WallTile();
            case TileType.Exit: return new ExitTile();
            case TileType.Treasure: return new TreasureTile();
            case TileType.UpStairs: 
                return new UpStairsTile { connectedPosition = new Vector2Int(x, y) };
            case TileType.DownStairs: 
                return new DownStairsTile { connectedPosition = new Vector2Int(x, y) };
            default: return new FloorTile();
        }
    }

    private void HandleStairsSync(FloorData currentFloor, int x, int y, TileType oldType, TileType newType)
    {
        if (newType == TileType.UpStairs)
        {
            FloorData upperFloor = targetData.GetFloorByLevel(currentFloor.floorLevel + 1);
            if (upperFloor != null) upperFloor.SetTile(x, y, new DownStairsTile { connectedPosition = new Vector2Int(x, y) });
        }
        else if (newType == TileType.DownStairs)
        {
            FloorData lowerFloor = targetData.GetFloorByLevel(currentFloor.floorLevel - 1);
            if (lowerFloor != null) lowerFloor.SetTile(x, y, new UpStairsTile { connectedPosition = new Vector2Int(x, y) });
        }

        if (oldType == TileType.UpStairs && newType != TileType.UpStairs)
        {
            FloorData upperFloor = targetData.GetFloorByLevel(currentFloor.floorLevel + 1);
            if (upperFloor != null && upperFloor.GetTile(x, y).Type == TileType.DownStairs)
            {
                upperFloor.SetTile(x, y, new FloorTile());
            }
        }
        else if (oldType == TileType.DownStairs && newType != TileType.DownStairs)
        {
            FloorData lowerFloor = targetData.GetFloorByLevel(currentFloor.floorLevel - 1);
            if (lowerFloor != null && lowerFloor.GetTile(x, y).Type == TileType.UpStairs)
            {
                lowerFloor.SetTile(x, y, new FloorTile());
            }
        }
    }

    private Color GetTileColor(TileType type)
    {
        switch (type)
        {
            case TileType.Floor: return Color.white;
            case TileType.Wall: return new Color(0.2f, 0.2f, 0.2f);
            case TileType.UpStairs: return Color.cyan;
            case TileType.DownStairs: return Color.blue;
            case TileType.Exit: return Color.red;
            case TileType.Treasure: return Color.yellow;
            default: return Color.gray;
        }
    }
}