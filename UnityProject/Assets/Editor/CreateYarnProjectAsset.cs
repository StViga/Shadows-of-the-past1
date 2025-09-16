#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Yarn.Unity;

public static class CreateYarnProjectAsset
{
    [MenuItem("Assets/Create/Yarn/Empty Yarn Project Asset")] 
    public static void Create()
    {
        var asset = ScriptableObject.CreateInstance<YarnProject>();
        var path = "Assets/Resources/Yarn";
        System.IO.Directory.CreateDirectory(path);
        var full = AssetDatabase.GenerateUniqueAssetPath(path + "/Project.asset");
        AssetDatabase.CreateAsset(asset, full);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        Debug.Log("Created YarnProject at " + full + ". Add your .yarn files to it in the Inspector.");
    }
}
#endif