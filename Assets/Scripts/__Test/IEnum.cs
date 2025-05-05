using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEnum : MonoBehaviour
{
	private void Start()
	{
		Test();
	}

	private IEnumerator Test()
	{
		Debug.Log("Test");
		return null;
	}
}
