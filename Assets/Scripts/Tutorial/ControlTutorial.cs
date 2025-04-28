using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class ControlTutorialManager : MonoBehaviour
{
    public static ControlTutorialManager Instance;
    public AudioSource audio;
    public AudioClip completeCurrentStep;
    public TutorialBomb bomb1;
    public Transform bomb1DisposalPoint;
    public TutorialBomb bomb2;
    public GameObject props;
    public CanvasGroup[] dialogs;

    private Camera camera;
    private float animationTime = 0.5f;
    private int currentDialogIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        camera = Camera.main;

        for (int i = 1; i < dialogs.Length; i++)
        {
            dialogs[i].alpha = 0;
        }
    }
    public void BombEvent(string bombEvent)
    {
        if (currentDialogIndex == 2 && bombEvent == "BlueWireCut")
        {
            SpawnBomb2();
            StepDialog();
        }
        else if (currentDialogIndex == 3 && bombEvent == "BombDeselected")
        {
            StepDialog();
        }
        else if (currentDialogIndex == 5 && bombEvent == "ButtonReleased")
        {
            StepDialog();
        }
        else if (currentDialogIndex == 6 && bombEvent == "BombRotated")
        {
            StepDialog();
        }
        else if (currentDialogIndex == 7 && bombEvent == "BombDeselected")
        {
            FoldTable();
        }
    }

    public void BombEvent(string bombEvent, TutorialBomb bomb)
    {
        if (currentDialogIndex == 1 && bombEvent == "BombSelected" && bomb == bomb1)
        {
            StepDialog();
        }
        else if (currentDialogIndex == 4 && bombEvent == "BombSelected" && bomb == bomb2)
        {
            StepDialog();
        }
    }

    private void StepDialog()
    {
        Sequence seq = DOTween.Sequence();
        currentDialogIndex++;
        seq.Append(dialogs[currentDialogIndex - 1].DOFade(0, animationTime));
        seq.AppendCallback(() =>
        {
            dialogs[currentDialogIndex - 1].gameObject.SetActive(false);
        });
        seq.AppendCallback(() =>
        {
            dialogs[currentDialogIndex].gameObject.SetActive(true);
        });
        seq.Append(dialogs[currentDialogIndex].DOFade(1, animationTime));
        audio.PlayOneShot(completeCurrentStep);
    }

    public void OpenTable()
    {
        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(StepDialog);
        seq.AppendCallback(() =>
        {
            props.transform.localScale = new Vector3(0,1,1);
            props.SetActive(true);
        });
        seq.Append(props.transform.DOScale(1, animationTime));
        seq.AppendCallback(() =>
        {
            bomb1.transform.localScale = Vector3.zero;
            bomb1.gameObject.SetActive(true);
            bomb1.transform.DOScale(1, animationTime);
        });
        seq.Append(camera.transform.DORotate(Quaternion.AngleAxis(30, camera.transform.right).eulerAngles, animationTime));
    }

    private void FoldTable()
    {
        Sequence seq = DOTween.Sequence();
        seq.Pause();

        seq.Append(camera.transform.DORotate(Quaternion.identity.eulerAngles, animationTime));
        seq.AppendCallback(() =>
        {
            bomb1.transform.DOScale(0, animationTime);
            bomb2.transform.DOScale(0, animationTime);
        });
        seq.AppendCallback(() =>
        {
            bomb1.gameObject.SetActive(false);
            bomb2.gameObject.SetActive(false);
        });
        seq.Append(props.transform.DOScaleX(0, animationTime));
        seq.AppendCallback(() =>
        {
            props.gameObject.SetActive(false);
        });
        seq.AppendCallback(() =>
        {
            SceneChanger.Instance.LoadSetupScene();
        });
        seq.Restart();
    }
    
    private void SpawnBomb2()
    {
        bomb1.originalPosition = bomb1DisposalPoint.position;
        bomb1.originalRotation = bomb1DisposalPoint.rotation.eulerAngles;
        
        bomb2.transform.localScale = Vector3.zero;
        bomb2.gameObject.SetActive(true);
        bomb2.transform.DOScale(1, animationTime);
    }
    
}
