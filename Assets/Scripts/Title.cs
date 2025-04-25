using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
	public Image blind;
	public TMP_Text text;

	private void Start()
	{
		blind.gameObject.SetActive(true);
		blind.DOFade(0f, 3f).onComplete += () =>
		{
			blind.gameObject.SetActive(false);
		};
		StartCoroutine(CycleTextAlpha());
	}

	private IEnumerator CycleTextAlpha()
	{
		while (gameObject.activeInHierarchy)
		{
			yield return text.DOFade(0.2f, 2f).WaitForCompletion();
			yield return text.DOFade(1f, 2f).WaitForCompletion();
		}
	}
}
