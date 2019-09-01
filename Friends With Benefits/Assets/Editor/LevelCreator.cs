using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Reflection;

[CustomEditor(typeof(LevelManager))]
public class LevelCreator : Editor
{
    [MenuItem("BITCHEZ/Generate New Level...")]
    static void GenerateNewLevel()
    {
        //create root
        GameObject newLevel = new GameObject("NewLevel");
        LevelManager levelManager = newLevel.AddComponent<LevelManager>();
        levelManager.spawnPoints = new Transform[4];

        //add tile grid
        GameObject tileGrid = new GameObject("TileGrid");
        tileGrid.transform.SetParent(newLevel.transform);
        Grid grid = tileGrid.AddComponent<Grid>();
        grid.cellSize = new Vector3(1f, 1f, 0f);
        grid.cellGap = new Vector3(0f, 0f, 0f);
        grid.cellLayout = GridLayout.CellLayout.Rectangle;
        grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;

        //add background tilemap layer
        GameObject backgroundTilemap = new GameObject("Tilemap_Background");
        backgroundTilemap.transform.SetParent(tileGrid.transform);
        backgroundTilemap.AddComponent<Tilemap>();
        TilemapRenderer bgRenderer = backgroundTilemap.AddComponent<TilemapRenderer>();
        bgRenderer.sortingLayerName = "Background";

        //add obstacles tilemap layer
        GameObject obstaclesTilemap = new GameObject("Tilemap_Obstacles");
        obstaclesTilemap.transform.SetParent(tileGrid.transform);
        obstaclesTilemap.layer = LayerMask.NameToLayer("Obstacle");
        obstaclesTilemap.AddComponent<Tilemap>();
        TilemapRenderer obstaclesRenderer = obstaclesTilemap.AddComponent<TilemapRenderer>();
        obstaclesRenderer.sortingLayerName = "Foreground";
        obstaclesTilemap.AddComponent<TilemapCollider2D>();

        //add props tilemap layer
        GameObject propsTilemap = new GameObject("Tilemap_Props");
        propsTilemap.transform.SetParent(tileGrid.transform);
        propsTilemap.AddComponent<Tilemap>();
        TilemapRenderer propsRenderer = propsTilemap.AddComponent<TilemapRenderer>();
        propsRenderer.sortingLayerName = "Foreground";
        propsRenderer.sortingOrder = -1;

        //add spawn points
        GameObject spawnPointsRoot = new GameObject("SpawnPoints");
        spawnPointsRoot.transform.SetParent(newLevel.transform);
        float increment = 10;
        for(int i = 0; i < 4; i++)
        {
            GameObject newSpawnPoint = new GameObject("SpawnPoint" + "_" + (i + 1).ToString());
            newSpawnPoint.transform.SetParent(spawnPointsRoot.transform);
            float xValue = (increment * i) - (increment * 1.5f);
            newSpawnPoint.transform.position = new Vector3(xValue, 0f, 0f);
            SetSpawnPointIcon(newSpawnPoint, i);
            levelManager.spawnPoints[i] = newSpawnPoint.transform;
        }
        
        //set new level as selected
        Selection.activeGameObject = newLevel;

        EditorApplication.ExecuteMenuItem("Window/2D/Tile Palette");
    }

    Tool lastTool = Tool.None;

    private void OnEnable()
    {
        lastTool = Tools.current;
        Tools.current = Tool.None;
    }

    void OnDisable()
    {
        Tools.current = lastTool;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelManager levelManager = (LevelManager)target;
    }

    protected virtual void OnSceneGUI()
    {
        LevelManager levelManager = (LevelManager)target;

        for (int i = 0; i < levelManager.spawnPoints.Length; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newTargetPosition = Handles.PositionHandle(levelManager.spawnPoints[i].position, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(levelManager, "Moved Spawn Point");
                levelManager.spawnPoints[i].position = new Vector3(Mathf.Round(newTargetPosition.x), Mathf.Round(newTargetPosition.y), 0f);
            }
        }
    }

    static void SetSpawnPointIcon(GameObject go, int i)
    {
        Texture2D texture = EditorGUIUtility.FindTexture("sv_label_1");
        if (i == 1)
        {
            texture = EditorGUIUtility.FindTexture("sv_label_4");

        }
        else if(i == 2)
        {
            texture = EditorGUIUtility.FindTexture("sv_label_6");
        }
        else if(i == 3)
        {
            texture = EditorGUIUtility.FindTexture("sv_label_3");
        }

        if (texture == null)
        {
            Debug.LogError("Couldn't find an icon...");
            return;
        }

        var so = new SerializedObject(go);
        var iconProperty = so.FindProperty("m_Icon");
        iconProperty.objectReferenceValue = texture;
        so.ApplyModifiedProperties();
    }
}
