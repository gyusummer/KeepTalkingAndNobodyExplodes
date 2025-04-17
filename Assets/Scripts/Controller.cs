using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Controller : MonoBehaviour
{
    private ISelectable selectedObject;
    [SerializeField]private Transform selectPosition;
    private Transform originalTransform;
    // private Vector3 originalPosition;
    // private Vector3 originalRotation;

    private Vector2 mouseMove;
    private float rightDownTime;

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
                if (hit.transform.TryGetComponent(out ISelectable selectable))
                {
                    Select(selectable);
                }
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
            mouseMove.x = Input.GetAxisRaw("Mouse X");
            mouseMove.y = Input.GetAxisRaw("Mouse Y");
            Vector3 rot = new Vector3(mouseMove.y,0,-mouseMove.x);
            rot *= Time.deltaTime * rotWeight;
            selectedObject.Transform.Rotate(rot);
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

    private void Select(ISelectable obj)
    {
        selectedObject = obj.OnSelected(selectPosition);
    }

    private void DeSelect()
    {
        selectedObject = selectedObject.OnDeselected();
    }
}
