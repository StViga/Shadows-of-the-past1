#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class SpriteAutoImporter
{
    [MenuItem("Tools/Art/Import Spritesheet 16x16")]
    public static void Import16() => ImportSelected(16, 16, 16);

    [MenuItem("Tools/Art/Import Spritesheet 32x32")]
    public static void Import32() => ImportSelected(32, 32, 32);

    private static void ImportSelected(int w, int h, int ppu)
    {
        foreach (var obj in Selection.objects)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path)) continue;
            var ti = AssetImporter.GetAtPath(path) as TextureImporter;
            if (ti == null) continue;
            ti.textureType = TextureImporterType.Sprite;
            ti.spriteImportMode = SpriteImportMode.Multiple;
            ti.filterMode = FilterMode.Point;
            ti.mipmapEnabled = false;
            ti.spritePixelsPerUnit = ppu;
            var size = GetTextureSize(path);
            if (size.x <= 0 || size.y <= 0) continue;
            int cols = size.x / w;
            int rows = size.y / h;
            var metas = new SpriteMetaData[cols * rows];
            int i = 0;
            for (int y = rows - 1; y >= 0; y--)
            for (int x = 0; x < cols; x++)
            {
                var r = new Rect(x * w, y * h, w, h);
                metas[i] = new SpriteMetaData {
                    name = $"{Path.GetFileNameWithoutExtension(path)}_{i}",
                    rect = r,
                    alignment = (int)SpriteAlignment.Center
                };
                i++;
            }
            ti.spritesheet = metas;
            ti.SaveAndReimport();
            Debug.Log($"Sliced {path} into {metas.Length} sprites at {w}x{h}");
        }
    }

    private static Vector2Int GetTextureSize(string path)
    {
        var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (tex == null) return Vector2Int.zero;
        return new Vector2Int(tex.width, tex.height);
    }
}
#endif