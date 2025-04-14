using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Controller : MonoBehaviour
{
    private GameObject selectedObject;
    [SerializeField]private Transform selectPosition;
    private Vector3 originalPosition;
    private Vector3 originalRotation;

    private Vector2 mouseMove;
    private float rightDownTime;
    
    private float selectAnimDur = 0.5f;

    private void Update()
    {
        UpdateMouseInput();
    }

    private float rotWeight = 300f;

    private void UpdateMouseInput()
    {
        // Debug.Log(mouseMove.magnitude);
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Select(hit.collider.gameObject);
            }
        }

        if (selectedObject == null) return;

        if (Input.GetMouseButtonDown(1))
        {
            rightDownTime = Time.time;
        }
        // right click drag
        if (Input.GetMouseButton(1))
        {
            if (selectedObject != null)
            {
                mouseMove.x = Input.GetAxisRaw("Mouse X");
                mouseMove.y = Input.GetAxisRaw("Mouse Y");
                Vector3 rot = new Vector3(mouseMove.y,0,-mouseMove.x);
                rot *= Time.deltaTime * rotWeight;
                selectedObject.transform.Rotate(rot);
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            bool isPointerMoved = mouseMove.magnitude > 0.5f;
            if (Time.time < rightDownTime + 0.2f && isPointerMoved == false)
            {
                DeSelect();
            }
        }
    }

    private void Select(GameObject obj)
    {
        selectedObject = obj.transform.root.gameObject;
        originalPosition = selectedObject.transform.position;
        originalRotation = selectedObject.transform.eulerAngles;
        
        selectedObject.transform.DOMove(selectPosition.position, selectAnimDur);
        selectedObject.transform.DORotate(selectPosition.eulerAngles, selectAnimDur);
        Debug.Log($"Selected ::: {selectedObject.gameObject.name}");
    }

    private void DeSelect()
    {
        Debug.Log($"DeSelected ::: {selectedObject.gameObject.name}");
        selectedObject.transform.DOMove(originalPosition, selectAnimDur);
        selectedObject.transform.DORotate(originalRotation, selectAnimDur);
        selectedObject = null;
    }
}
