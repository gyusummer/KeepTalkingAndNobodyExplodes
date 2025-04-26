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
            StrikeCount = 3
        };
    }
}
[RequireComponent(typeof(Outlinable),typeof(AudioSource))]
public class Bomb : MonoBehaviour, ISelectable
{
    [Serializable]
    private class AudioClips
    {
        public AudioClip outlineTick;
        public AudioClip putDown;
        public AudioClip defuse;
        public AudioClip strike;
        public AudioClip explode;
    }
    public static Bomb Main;

    public event Action OnBombStrike;
    
    public BombInfo Info;
    private string serial;
    public string indicator;
    public string[] battery;
    public TimerModule timerModule;

    private static readonly string[] INDICATOR_LIST =
        { "SND", "CLR", "CAR", "IND", "FRQ", "SIG", "NSA", "MSA", "TRN", "BOB", "FRK" };
    
    public GameObject GameObject => gameObject;
    public Transform Transform => transform;
    public Collider Collider => selectCollider;
    private Collider selectCollider;
    
    [SerializeField] private PrefabGroup componentPrefabs;
    [SerializeField] private Transform[] moduleAnchors;
    [SerializeField] private Transform[] widgetAnchors;
    private Outlinable outline;
    [SerializeField] private AudioSource audio;
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
        indicator = INDICATOR_LIST[Random.Range(0, INDICATOR_LIST.Length)];

        battery = new string[Random.Range(1, 5)];
        for (int i = 0; i < battery.Length; i++)
        {
            battery[i] = Random.Range(0, 1) < 0.5f ? "AA" : "D";
        }
    }

    private void Start()
    {
        selectCollider = GetComponent<Collider>();
        outline = GetComponent<Outlinable>();
        timerModule = GetComponentInChildren<TimerModule>();

        outline.enabled = false;

        try
        {
            Info = SceneChanger.Instance.BombInfo;
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
                  $"Indicator: {indicator}\n" +
                  $"Battery: {battery.Length}\n" +
                  $"LimitTime: {Info.LimitTime}");
        
        FillModules();
        AttachWidgets();
    }
    private void FillModules()
    {
        if (Info.ModuleCount > componentPrefabs.modules.Length)
        {
            Debug.LogAssertion("Module count exceeds available modules.");
            return;
        }
        // we only use front anchors now
        var anchors= RandomUtil.GetShuffled(moduleAnchors[0..6]);
        
        timerModule = Instantiate(componentPrefabs.timer, anchors[5]);
        timerModule.bomb = this;

        var modules = RandomUtil.GetRandomSubset(componentPrefabs.modules, Info.ModuleCount);
        for(int i = 0; i < Info.ModuleCount; i++)
        {
            var module = modules[i];
            module.bomb = this;
            Instantiate(module, anchors[i]);
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
        int widgetCount = 2 + battery.Length;
        var widgetTransforms = RandomUtil.GetRandomSubset(widgetAnchors, widgetCount);
        
        GameObject serialObj = Instantiate(componentPrefabs.widgets[0], widgetTransforms[widgetCount - 1]);
        serialObj.GetComponentInChildren<TMP_Text>().text = this.serial;
        GameObject indicatorObj = Instantiate(componentPrefabs.widgets[1], widgetTransforms[widgetCount - 2]);
        indicatorObj.GetComponentInChildren<TMP_Text>().text = this.indicator;

        for (int i = 0; i < battery.Length; i++)
        {
            var t = widgetTransforms[i];
            switch (battery[i])
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
            timerModule.strikeCounter[CurStrike - 1].SetActive(true);
            PlaySound(audioClips.strike);
        }
    }

    public void Explode(DisarmableModule module)
    {
        PlaySound(audioClips.explode);
        timerModule.StopTimer();
        ResultInfo info = new ResultInfo();
        info.isDefused = false;
        info.stageInfo = SceneChanger.Instance.currentStageInfo;
        info.leftTimeString = timerModule.leftTimeString;
        info.causeOfExplosion = module == null? "TimeLimit" : module.GetType().Name;
        
        FacilityManager.Instance.ShowResult(info);
        Debug.Log("Explode");
    }

    private void Defuse()
    {
        PlaySound(audioClips.defuse);
        timerModule.StopTimer();
        ResultInfo info = new ResultInfo();
        info.isDefused = true;
        info.stageInfo = SceneChanger.Instance.currentStageInfo;

        info.leftTimeString = timerModule.leftTimeString;
        
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
        Debug.Log($"Selected ::: {gameObject.name}");
        Collider.enabled = false;
        return this;
    }

    public ISelectable OnDeselected()
    {
        Debug.Log($"DeSelected ::: {gameObject.name}");
        transform.DOMove(originalPosition, 0.5f);
        transform.DORotate(originalRotation, 0.5f);
        Collider.enabled = true;
        return null;
    }

    private void OnMouseEnter()
    {
        PlaySound(audioClips.outlineTick);
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
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

    private void PlaySound(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
    }
}