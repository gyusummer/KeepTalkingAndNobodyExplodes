using System;
using System.Linq;
using DG.Tweening;
using EPOOutline;
using UnityEngine;
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

    public TimeSpan LimitTime = new TimeSpan(0, 5, 0);
    
    [SerializeField] private PrefabGroup modulePrefabs;
    [SerializeField] private Transform[] componentAnchors;
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
    }

    private void SetModulesPosition()
    {
        
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

    private void Explode()
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