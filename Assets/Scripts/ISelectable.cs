using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public GameObject GameObject { get; }
    public Transform Transform { get; }
    public Collider Collider { get; }
    public ISelectable OnSelected(Transform selectPosition);
    public ISelectable OnDeselected();
}
