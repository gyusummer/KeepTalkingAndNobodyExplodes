using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	private static AudioManager instance;
	public static AudioManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("AudioManager").AddComponent<AudioManager>();
			}

			return instance;
		}
	}

	[SerializeField] private AudioSource Sfx;
	[SerializeField] private AudioSource Bgm;

    private void Awake()
    {
	    if (instance == null)
	    {
		    instance = this;
	    }

	    if (Sfx == null)
	    {
		    Sfx = gameObject.AddComponent<AudioSource>();
	    }
	    if (Bgm == null)
	    {
		    Bgm = gameObject.AddComponent<AudioSource>();
	    }
    }

    private void Start()
	{
		Bgm.volume = 0f;
		Bgm.DOFade(1f, 1f);
		SceneChanger.Instance.OnScneChange += FadeBgm;
	}

	public void PlaySfx(AudioClip clip)
	{
		Sfx.PlayOneShot(clip);
	}

	private void FadeBgm()
	{
		Bgm.DOFade(0f, 1f);
	}

	private void OnDestroy()
	{
		SceneChanger.Instance.OnScneChange -= FadeBgm;
	}
}
