using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveEmptyChildObjects : MonoBehaviour
{
    [ContextMenu("Remove Empty Children")]
    void RemoveEmptyChildren()
    {
        // 자식들을 임시로 저장해두기 (삭제 중에는 순회하면 안됨)
        Transform[] children = GetComponentsInChildren<Transform>(true);

        int removedCount = 0;
        foreach (Transform child in children)
        {
            if (child == transform)
                continue; // 자기 자신은 제외

            // 자식이 없고 컴포넌트가 Transform 하나만 있는 경우
            if (child.childCount == 0 && child.GetComponents<Component>().Length == 1)
            {
                DestroyImmediate(child.gameObject);
                removedCount++;
            }
        }

        Debug.Log($"Removed {removedCount} empty child object(s).");
    }
}
