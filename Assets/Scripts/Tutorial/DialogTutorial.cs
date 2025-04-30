using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DialogTutorial : MonoBehaviour
{
    public GameObject dialog;
    public GameObject[] dialogPages;
    private int currentPageIndex = 0;
    public GameObject prevButton;
    public GameObject nextButton;
    public GameObject finishButton;
    
    public void ShowNextPage()
    {
        if (currentPageIndex < dialogPages.Length - 1)
        {
            currentPageIndex++;
            dialogPages[currentPageIndex].SetActive(true);
        }
        dialog.transform.DORotate(dialog.transform.eulerAngles + new Vector3(0f, -50f, 0f), 0.5f).onComplete += () =>
        {
            if (currentPageIndex >= 2)
            {
                dialogPages[currentPageIndex - 2].SetActive(false);
            }
        };
        if(currentPageIndex >= 1)
        {
            prevButton.SetActive(true);
        }

        if (currentPageIndex == dialogPages.Length - 1)
        {
            nextButton.SetActive(false);
            finishButton.SetActive(true);
        }
    }
    public void ShowPreviousPage()
    {
        if (currentPageIndex <= 0)
        {
            return;
        }

        nextButton.SetActive(true);
        finishButton.SetActive(false);
        dialogPages[currentPageIndex].SetActive(false);
        currentPageIndex--;
        dialog.transform.DORotate(dialog.transform.eulerAngles + new Vector3(0f, 50f, 0f), 0.5f);
        if (currentPageIndex >= 1)
        {
            dialogPages[currentPageIndex - 1].SetActive(true);
        }
        if (currentPageIndex == 0)
        {
            prevButton.SetActive(false);
        }
    }

    public void Finish()
    {
        SceneChanger.Instance.LoadSetupScene();
    }
}
