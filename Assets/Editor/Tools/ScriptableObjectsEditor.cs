using UnityEngine;
using System.Collections;
using UnityEditor;

public class ScriptableObjectsEditor : MonoBehaviour {

    [MenuItem("Assets/CreateLevelData")]
    public static void CreateLevelData()
    {
        LevelData asset = ScriptableObject.CreateInstance<LevelData>();
        AssetDatabase.CreateAsset(asset, "Assets/Prefabs/Data/LevelData.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
