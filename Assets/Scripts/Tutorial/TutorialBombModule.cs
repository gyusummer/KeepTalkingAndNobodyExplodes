using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBombModule : TutorialModule
{
    private static readonly int OPEN = Animator.StringToHash("Open");
    private static readonly int CLOSE = Animator.StringToHash("Close");
    
    [SerializeField]private Animator animator;
    public override ISelectable OnSelected(Transform selectPosition)
    {
        OpenLid();
        return base.OnSelected(selectPosition);
    }
    public override ISelectable OnDeselected()
    {
        CloseLid();
        return base.OnDeselected();
    }
    private void OpenLid()
    {
        animator.ResetTrigger(CLOSE);
        animator.SetTrigger(OPEN);
    }
    private void CloseLid()
    {
        animator.ResetTrigger(OPEN);
        animator.SetTrigger(CLOSE);
    }
}
