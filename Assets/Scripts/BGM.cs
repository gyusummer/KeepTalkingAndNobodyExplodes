using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour
{
	public AudioSource audio;

	private void Start()
	{
		audio.volume = 0f;
		audio.DOFade(1f, 1f);
		SceneChanger.Instance.OnScneChange += OnSceneChange;
	}

	private void OnSceneChange()
	{
		audio.DOFade(0f, 1f);
	}

	private void OnDestroy()
	{
		SceneChanger.Instance.OnScneChange -= OnSceneChange;
	}
}
