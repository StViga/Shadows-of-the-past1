#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapBuilder
{
    [MenuItem("Tools/World/Replace Quads With Tilemap")] 
    public static void Replace()
    {
        var root = new GameObject("TilemapRoot");
        var grid = root.AddComponent<Grid>();
        grid.cellLayout = GridLayout.CellLayout.Isometric;
        grid.cellSize = new Vector3(1, 0.5f, 1);

        var tilemapGO = new GameObject("Ground");
        tilemapGO.transform.SetParent(root.transform);
        var tm = tilemapGO.AddComponent<Tilemap>();
        tilemapGO.AddComponent<TilemapRenderer>();

        Debug.Log("Create a Tile asset from your imported sprites and paint the map. This helper just scaffolds the Tilemap root.");
    }
}
#endif