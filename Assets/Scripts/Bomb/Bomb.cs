using System;
using System.Linq;
using DG.Tweening;
using EPOOutline;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Bomb : MonoBehaviour, ISelectable
{
    public static Bomb Instance;
    public static string Serial;
    public static string Indicator;
    public static string[] Battery;
    public static TimerModule TimerModule;

    private static readonly string[] INDICATOR_LIST =
        { "SND", "CLR", "CAR", "IND", "FRQ", "SIG", "NSA", "MSA", "TRN", "BOB", "FRK" };
    
    public GameObject GameObject => gameObject;
    public Transform Transform => transform;
    public Collider Collider => selectCollider;
    private Collider selectCollider;

    public TimeSpan LimitTime = new TimeSpan(0, 1, 0);
    
    [SerializeField] private PrefabGroup componentPrefabs;
    [SerializeField] private Transform[] moduleAnchors;
    [SerializeField] private Transform[] widgetAnchors;
    private Outlinable outline;
    private int opportunity = 3;
    private int strikeCount = 0;
    private Vector3 originalPosition;
    private Vector3 originalRotation;

    private void Awake()
    {
        Instance = this;
        // TODO: Randomize serial
        Serial = "4X8SB2";
        Indicator = INDICATOR_LIST[Random.Range(0, INDICATOR_LIST.Length)];

        Battery = new string[Random.Range(1, 5)];
        for (int i = 0; i < Battery.Length; i++)
        {
            Battery[i] = Random.Range(0, 1) < 0.5f ? "AA" : "D";
        }
    }

    private void Start()
    {
        selectCollider = GetComponent<Collider>();
        outline = GetComponent<Outlinable>();
        TimerModule = GetComponentInChildren<TimerModule>();

        outline.enabled = false;
        Debug.Log($"Serial: {Serial}\n" +
                  $"Indicator: {Indicator}\n" +
                  $"Battery: {Battery.Length}\n" +
                  $"LimitTime: {LimitTime}");
        
        FillModules();
        AttachWidgets();
    }

    private void FillModules()
    {
        TimerModule = componentPrefabs.timer;
        var temp= RandomUtil.GetShuffled(moduleAnchors[0..5]);
        Instantiate(TimerModule.gameObject, temp[0]);
        for(int i = 0; i < componentPrefabs.modules.Length; i++)
        {
            var module = componentPrefabs.modules[i];
            Instantiate(module.gameObject, temp[i + 1]);
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
        
        GameObject serial = Instantiate(componentPrefabs.widgets[0], widgetTransforms[widgetCount - 1]);
        serial.GetComponentInChildren<TMP_Text>().text = Serial;
        GameObject indicator = Instantiate(componentPrefabs.widgets[1], widgetTransforms[widgetCount - 2]);
        indicator.GetComponentInChildren<TMP_Text>().text = Indicator;

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

    public void Strike()
    {
        Debug.Log("Strike");
        strikeCount++;

        if (strikeCount >= opportunity)
        {
            Explode();
        }
        else
        {
            TimerModule.strikeCounter[strikeCount - 1].SetActive(true);
        }
    }

    public void Explode()
    {
        Debug.Log("Explode");
    }

    public bool IsSerialOdd()
    {
        char serialLast = Serial.Last();
        int lastAsInt = serialLast - 48;
        return lastAsInt % 2 == 1;
    }

    public ISelectable OnSelected(Transform selectPosition)
    {
        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;

        transform.DOMove(selectPosition.position, 0.5f);
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