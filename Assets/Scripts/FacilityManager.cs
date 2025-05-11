using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

public class ResultInfo
{
    public bool isDefused;
    public StageInfoSAO stageInfo;
    public string leftTimeString;
    public string causeOfExplosion;

    public string[] ToStringArray()
    {
        string[] result = new string[6];
        result[0] = stageInfo.Identifier;
        result[1] = $"{stageInfo.LimitMinute:D2}:{stageInfo.LimitSecond:D2}";
        result[2] = $"{stageInfo.Modules} Modules";
        result[3] = $"{stageInfo.Strikes} Strikes";
        result[4] = $"Time Remaining:\n{leftTimeString}";
        result[5] = isDefused ? String.Empty : $"Cause of Explosion:\n{causeOfExplosion}";
        return result;
    }
}
public class FacilityManager : MonoBehaviour
{
    public static FacilityManager Instance;
    public GameObject light;
    public BombBinder binder;
    public AudioSource audio;
    public AudioClip switchOn;
    public AudioClip fanfare;
    public AudioClip lose;
    public AudioClip win;

    public Canvas canvas;
    public Image black;

    private bool isOver = false;
    [SerializeField] private Image hint;
    [SerializeField] private Sprite[] hints;
    private int hintCursor = 0;
    private Camera cam;
    private Vector3 camPos;
    
    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        cam = Camera.main;
        camPos = cam.transform.position;
        yield return new WaitForSeconds(2.0f);
        
        RenderSettings.reflectionIntensity = 1.0f;
        RenderSettings.ambientIntensity = 1.0f;
        light.SetActive(true);
        audio.PlayOneShot(switchOn);
        Bomb.Main.TimerModule.StartTimer();
        
        yield return new WaitForSeconds(2.0f);

        audio.volume = 0f;
        audio.Play();
        audio.DOFade(1.0f, 1.0f);
    }

    public void ShowResult(ResultInfo info)
    {
        isOver = true;
        hint.gameObject.SetActive(false);
        cam.transform.DOMoveX(camPos.x, 0.5f);

        binder.ShowResultPage(info);
        audio.Stop();

        if (info.isDefused == true)
        {
            StartCoroutine(Win_Coroutine());
        }
        else
        {
            StartCoroutine(Lose_Coroutine());
        }
    }

    private void Update()
    {
        if (isOver)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            cam.transform.DOMoveX(camPos.x * - 1, 0.5f);
            hint.gameObject.SetActive(true);
        }
        if (Input.GetKey(KeyCode.H))
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                if(Input.mouseScrollDelta.y > 0)
                {
                    hintCursor--;
                }
                else if(Input.mouseScrollDelta.y < 0)
                {
                    hintCursor++;
                }

                hintCursor = Math.Clamp(hintCursor, 0, hints.Length - 1);
                hint.sprite = hints[hintCursor];

            }
        }

        if (Input.GetKeyUp(KeyCode.H))
        {
            cam.transform.DOMoveX(camPos.x, 0.5f);
            hint.gameObject.SetActive(false);
        }
    }

    private IEnumerator Win_Coroutine()
    {
        yield return new WaitForSeconds(2.0f);
        audio.PlayOneShot(fanfare);
        yield return new WaitForSeconds(3.0f);
        black.color = Color.clear;
        black.gameObject.SetActive(true);
        audio.clip = win;
        black.DOFade(1.0f, 1.0f).onComplete += StartCommonRoutine;
    }
    private IEnumerator Lose_Coroutine()
    {
        black.color = Color.black;
        black.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        audio.clip = lose;
        StartCommonRoutine();
    }
    
    private void StartCommonRoutine()
    {
        StartCoroutine(CommonRoutine_Coroutine());
    }
    private IEnumerator CommonRoutine_Coroutine()
    {
        audio.Play();
        yield return new WaitForSeconds(1f);
        Controller.Instance.transform.position = transform.position;
        canvas.planeDistance = 3.0f;
        binder.gameObject.SetActive(true);
        Controller.Instance.Select(binder);
    }
}
