using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance;
    [SerializeField]private Transform selectPosition;

    public ISelectable selectedObject;
    private Vector2 mouseMove;
    private float rotationX = 0f;
    private float rotationZ = 0f;
    private float rightDownTime;
    
    private void Awake()
    {
        Instance = this;
    }
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
        if (Input.GetMouseButton(1) && selectedObject is not DisarmableModule)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotWeight * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotWeight * Time.deltaTime;

            rotationZ -= mouseX;
            rotationX += mouseY;

            selectedObject.Transform.rotation = Quaternion.Euler(rotationX, 0f, rotationZ);
        }
        if (Input.GetMouseButtonUp(1))
        {
            bool isPointerMoved = mouseMove.magnitude > 0.5f;
            if (Time.time < rightDownTime + 0.2f && isPointerMoved == false)
            {
                DeSelect();
            }
            else
            {
                var now = selectedObject.Transform.forward;
                var qua = Quaternion.FromToRotation(now, selectPosition.forward) * selectedObject.Transform.rotation;
                selectedObject.Transform.DORotate(qua.eulerAngles, 0.5f);
                
                rotationX = selectedObject.Transform.rotation.eulerAngles.x;
                rotationZ = selectedObject.Transform.rotation.eulerAngles.z;
            }
        }
    }
    public void Select(ISelectable obj)
    {
        selectedObject = obj.OnSelected(selectPosition);
        rotationX = selectedObject.Transform.rotation.eulerAngles.x;
        rotationZ = selectedObject.Transform.rotation.eulerAngles.z;
    }

    private void DeSelect()
    {
        selectedObject = selectedObject.OnDeselected();
    }
}
