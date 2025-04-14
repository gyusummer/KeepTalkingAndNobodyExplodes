using UnityEditor;
using UnityEngine;

public class DisableConvertUnits : EditorWindow
{
    private string targetFolder = "Assets/Models"; // 대상 폴더 경로

    [MenuItem("Tools/Disable Convert Units")]
    public static void ShowWindow()
    {
        GetWindow<DisableConvertUnits>("Disable Convert Units");
    }

    private void OnGUI()
    {
        GUILayout.Label("Convert Units 비활성화할 폴더", EditorStyles.boldLabel);
        targetFolder = EditorGUILayout.TextField("Target Folder", targetFolder);

        if (GUILayout.Button("적용"))
        {
            ApplyDisableConvertUnits();
        }
    }

    private void ApplyDisableConvertUnits()
    {
        string[] guids = AssetDatabase.FindAssets("t:Model", new[] { targetFolder });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;

            if (importer != null)
            {
                importer.useFileScale = false; // Convert Units 끄기
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                Debug.Log($"Convert Units 끔: {path}");
            }
        }

        Debug.Log("모든 FBX에 Convert Units 비활성화 적용 완료");
    }
}