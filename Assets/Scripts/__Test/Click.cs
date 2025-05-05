using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Click : MonoBehaviour, IPointerClickHandler
{
	private void Start()
	{
		Debug.Log("Click Start");

		int i = -3;
		
		Debug.Log(Math.Clamp(i,0,10));
		Debug.Log(i);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Pointer");
	}

	private void OnMouseUpAsButton()
	{
		Debug.Log("Mouse");
	}
}
