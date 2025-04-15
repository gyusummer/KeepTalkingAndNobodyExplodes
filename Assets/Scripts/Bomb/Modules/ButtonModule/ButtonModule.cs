using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonModule : MonoBehaviour
{
    private static readonly int OPEN = Animator.StringToHash("Open");
    private static readonly int CLOSE = Animator.StringToHash("Close");
    [SerializeField]private Animator animator;

    public void OpenLid()
    {
        animator.ResetTrigger(CLOSE);
        animator.SetTrigger(OPEN);
    }
    public void CloseLid()
    {
        animator.ResetTrigger(OPEN);
        animator.SetTrigger(CLOSE);
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse Enter");
    }
}
