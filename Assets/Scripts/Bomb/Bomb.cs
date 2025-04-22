using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EPOOutline;
using TMPro;
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
            LimitTime = new TimeSpan(0, 5, 0),
            ModuleCount = 3,
            StrikeCount = 3
        };
    }
}

public class Bomb : MonoBehaviour, ISelectable
{
    public static Bomb Main;
    
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
    private int curStrike = 0;
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
        // TODO: Randomize serial
        serial = "4X8SB2";
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

        Info = SceneChanger.Instance.BombInfo;

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
        var anchors= RandomUtil.GetShuffled(moduleAnchors[0..5]);
        
        timerModule = Instantiate(componentPrefabs.timer, anchors[0]);
        timerModule.bomb = this;

        var modules = RandomUtil.GetRandomSubset(componentPrefabs.modules, Info.ModuleCount);
        for(int i = 0; i < Info.ModuleCount; i++)
        {
            var module = modules[i];
            module.bomb = this;
            Instantiate(module, anchors[i + 1]);
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
        curStrike++;

        if (curStrike >= Info.StrikeCount)
        {
            Explode(module);
        }
        else
        {
            timerModule.strikeCounter[curStrike - 1].SetActive(true);
        }
    }

    public void Explode(DisarmableModule module)
    {
        ResultInfo info = new ResultInfo();
        info.isDefused = false;
        info.stageInfo = SceneChanger.Instance.currentStageInfo;
        info.leftTimeString = timerModule.leftTimeString;
        info.causeOfExplosion = module == null? "TimeLimit" : module.GetType().Name;
        
        Result.Instance.ShowResult(info);
        Debug.Log("Explode");
    }

    private void Defuse()
    {
        timerModule.StopTimer();
        ResultInfo info = new ResultInfo();
        info.isDefused = true;
        info.stageInfo = SceneChanger.Instance.currentStageInfo;
        info.leftTimeString = timerModule.leftTimeString;
        
        Result.Instance.ShowResult(info);
        Debug.Log("Defuse");
    }

    public bool IsSerialOdd()
    {
        char serialLast = serial.Last();
        int lastAsInt = serialLast - 48;
        return lastAsInt % 2 == 1;
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
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
    }
}