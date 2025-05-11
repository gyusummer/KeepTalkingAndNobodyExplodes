using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using EPOOutline;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class BombInfo
{
    public TimeSpan LimitTime;
    public int ModuleCount;
    public int StrikeCount;

    public static BombInfo GetDefault()
    {
        return new BombInfo
        {
            LimitTime = new TimeSpan(0, 60, 0),
            ModuleCount = 5,
            StrikeCount = 99
        };
    }
}

[RequireComponent(typeof(PointHighlighter))]
public class Bomb : MonoBehaviour, ISelectable
{
    [Serializable]
    private class AudioClips
    {
        public AudioClip PutDown;
        public AudioClip Defuse;
        public AudioClip Strike;
        public AudioClip Explode;
    }

    public static Bomb Main;

    public event Action OnBombStrike;

    public BombInfo Info;
    private string serial;
    public string Indicator;
    public string[] Battery;
    public TimerModule TimerModule;

    private static readonly string[] INDICATOR_LIST =
        { "SND", "CLR", "CAR", "IND", "FRQ", "SIG", "NSA", "MSA", "TRN", "BOB", "FRK" };

    public GameObject GameObject => gameObject;
    public Transform Transform => transform;
    public Collider Collider { get; private set; }

    [SerializeField] private PrefabGroup componentPrefabs;
    [SerializeField] private Transform[] moduleAnchors;
    [SerializeField] private Transform[] widgetAnchors;
    [SerializeField] private AudioClips audioClips;
    public int CurStrike { get; private set; } = 0;
    private int curDisarm = 0;

    public int CurDisarm
    {
        get => curDisarm;
        set
        {
            curDisarm = value;
            if (curDisarm >= Info.ModuleCount)
            {
                Defuse();
            }
        }
    }

    private Vector3 originalPosition;
    private Vector3 originalRotation;

    private void Awake()
    {
        if (Main == null)
        {
            Main = this;
        }

        serial = GenerateSerial();
        Indicator = INDICATOR_LIST[Random.Range(0, INDICATOR_LIST.Length)];

        Battery = new string[Random.Range(1, 5)];
        for (int i = 0; i < Battery.Length; i++)
        {
            Battery[i] = Random.Range(0, 1) < 0.5f ? "AA" : "D";
        }
    }

    private void Start()
    {
        Collider = GetComponent<Collider>();
        TimerModule = GetComponentInChildren<TimerModule>();

        try
        {
            Info = SceneChanger.Instance.currentStageInfo.BombInfo;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        if (Info == null)
        {
            Debug.Log("BombInfo is null");
            Info = BombInfo.GetDefault();
        }

        Debug.Log($"Serial: {serial}\n" +
                  $"Indicator: {Indicator}\n" +
                  $"Battery: {Battery.Length}\n" +
                  $"LimitTime: {Info.LimitTime}");

        FillModules();
        AttachWidgets();
    }

    private void FillModules()
    {
        DisarmableModule[] moduleCandidates = SceneChanger.Instance.currentStageInfo.ModuleCandidates.prefabs;
        if (Info.ModuleCount > moduleCandidates.Length)
        {
            Debug.LogAssertion("Module count exceeds available modules.");
            return;
        }


        int frontCount = Info.ModuleCount;
        if (Info.ModuleCount >= 6)
        {
            frontCount = Info.ModuleCount / 2;
        }

        int backCount = Info.ModuleCount - frontCount;

        var frontAnchors = RandomUtil.GetShuffled(moduleAnchors[..6]);
        var backAnchors = RandomUtil.GetShuffled(moduleAnchors[6..]);

        TimerModule = Instantiate(componentPrefabs.timer, frontAnchors[5]);
        TimerModule.Bomb = this;

        var modules = RandomUtil.GetRandomSubset(moduleCandidates, Info.ModuleCount);
        var frontModules = modules[..frontCount];
        var backModules = modules[frontCount..];

        // swap button module to front
        for (int i = 0; i < backCount; i++)
        {
            if (backModules[i] is ButtonModule)
            {
                var front = Array.FindIndex(frontModules, m => m is not ButtonModule);
                Debug.Log($"{backModules[i].GetType().Name} ::: {frontModules[i].GetType().Name}");
                (frontModules[front], backModules[i]) = (backModules[i], frontModules[front]);
            }
        }

        for (int i = 0; i < frontCount; i++)
        {
            var module = frontModules[i];
            module.Bomb = this;
            Instantiate(module, frontAnchors[i]);
        }

        for (int i = 0; i < backCount; i++)
        {
            var module = backModules[i];
            module.Bomb = this;
            Instantiate(module, backAnchors[i]);
        }

        CoverEmpties();
    }

    private void CoverEmpties()
    {
        for (int i = 0; i < moduleAnchors.Length; i++)
        {
            if (moduleAnchors[i].childCount == 0)
            {
                Instantiate(componentPrefabs.emptyCoverPrefab, moduleAnchors[i]);
            }
        }
    }

    private void AttachWidgets()
    {
        int widgetCount = 2 + Battery.Length;
        var widgetTransforms = RandomUtil.GetRandomSubset(widgetAnchors, widgetCount);

        GameObject serialObj = Instantiate(componentPrefabs.widgets[0], widgetTransforms[widgetCount - 1]);
        serialObj.GetComponentsInChildren<TMP_Text>()[1].text = this.serial;
        GameObject indicatorObj = Instantiate(componentPrefabs.widgets[1], widgetTransforms[widgetCount - 2]);
        indicatorObj.GetComponentInChildren<TMP_Text>().text = this.Indicator;

        for (int i = 0; i < Battery.Length; i++)
        {
            var t = widgetTransforms[i];
            switch (Battery[i])
            {
                case "AA":
                    Instantiate(componentPrefabs.widgets[2], t);
                    break;
                case "D":
                    Instantiate(componentPrefabs.widgets[3], t);
                    break;
            }
        }
    }

    public void Strike(DisarmableModule module)
    {
        Debug.Log("Strike");
        CurStrike++;

        if (CurStrike >= Info.StrikeCount)
        {
            Explode(module);
        }
        else
        {
            OnBombStrike?.Invoke();
            TimerModule.StrikeCounter[CurStrike - 1].SetActive(true);
            AudioManager.Instance.PlaySfx(audioClips.Strike);
        }
    }

    public void Explode(DisarmableModule module)
    {
        AudioManager.Instance.PlaySfx(audioClips.Explode);
        TimerModule.StopTimer();
        ResultInfo info = new ResultInfo();
        info.isDefused = false;
        info.stageInfo = SceneChanger.Instance.currentStageInfo;
        info.leftTimeString = TimerModule.LeftTimeString;
        info.causeOfExplosion = module == null ? "TimeLimit" : module.GetType().Name;

        FacilityManager.Instance.ShowResult(info);
        Debug.Log("Explode");
    }

    private void Defuse()
    {
        AudioManager.Instance.PlaySfx(audioClips.Defuse);
        TimerModule.StopTimer();
        ResultInfo info = new ResultInfo();
        info.isDefused = true;
        info.stageInfo = SceneChanger.Instance.currentStageInfo;

        info.leftTimeString = TimerModule.LeftTimeString;

        FacilityManager.Instance.ShowResult(info);
        Debug.Log("Defuse");
    }

    public bool IsSerialOdd()
    {
        char serialLast = serial.Last();
        int lastAsInt = serialLast - 48;
        return lastAsInt % 2 == 1;
    }

    public bool HasSerialVowel()
    {
        char[] vowels = { 'A', 'E', 'I', 'O', 'U' };
        return serial.Any(c => vowels.Contains(c));
    }

    public ISelectable OnSelected(Transform selectPosition)
    {
        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;

        var targetPosition = selectPosition.position - selectPosition.up * 0.1f;

        transform.DOMove(targetPosition, 0.5f);
        transform.DORotate(selectPosition.eulerAngles, 0.5f);
        Collider.enabled = false;
        return this;
    }

    public ISelectable OnDeselected()
    {
        transform.DOMove(originalPosition, 0.5f);
        transform.DORotate(originalRotation, 0.5f);
        Collider.enabled = true;
        return null;
    }

    private string GenerateSerial()
    {
        var sb = new StringBuilder(6);
        sb.Append(RandomUtil.GetRandomAlphabetOrDigit());
        sb.Append(RandomUtil.GetRandomAlphabet());
        sb.Append(RandomUtil.GetRandomDigit());
        sb.Append(RandomUtil.GetRandomAlphabet());
        sb.Append(RandomUtil.GetRandomAlphabet());
        sb.Append(RandomUtil.GetRandomDigit());
        return sb.ToString();
    }
}