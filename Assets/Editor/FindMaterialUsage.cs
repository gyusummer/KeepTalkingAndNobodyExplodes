using UnityEngine;
using UnityEditor;

public class FindMaterialUsage : EditorWindow
{
    private Material targetMaterial;

    [MenuItem("Tools/Find Material Usage")]
    public static void ShowWindow()
    {
        GetWindow<FindMaterialUsage>("Find Material Usage");
    }

    private void OnGUI()
    {
        targetMaterial = (Material)EditorGUILayout.ObjectField("Target Material", targetMaterial, typeof(Material), false);

        if (GUILayout.Button("Find"))
        {
            FindObjectsUsingMaterial(targetMaterial);
        }
    }

    private static void FindObjectsUsingMaterial(Material mat)
    {
        if (mat == null)
        {
            Debug.LogWarning("Material is null.");
            return;
        }

        Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            foreach (var material in renderer.sharedMaterials)
            {
                if (material == mat)
                {
                    Debug.Log($"Found '{mat.name}' on GameObject: {renderer.gameObject.name}", renderer.gameObject);
                    break;
                }
            }
        }
    }
}