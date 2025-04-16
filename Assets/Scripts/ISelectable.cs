using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public GameObject gameObject { get; }
    public Transform transform { get; }
    public void OnSelected();
    public void OnDeselected();
}
