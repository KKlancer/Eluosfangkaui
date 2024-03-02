using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shape : MonoBehaviour
{
    public Controller controller;
    public float dropTimer = 1f;
    public bool isPause = false;
    public GameManger gameManger;

    private Button[] buttons;
    private HandSlide handSlide;

    public static int isSpeed = 0;
    public bool isDownButtom = false;
    private Transform pivot;
    //敌方方块

    private void Awake()
    {
        buttons = GameObject.Find("ControlButtonsUI").GetComponentsInChildren<Button>();
        handSlide = GameObject.Find("SolidScreenUI").GetComponent<HandSlide>();
        handSlide.RightMove += OnRightButtonClick;
        handSlide.LiftMove += OnLeftButtonClick;
        handSlide.DownMove += OnDownButtonClick;
        handSlide.DoubleClick += OnRotateButtonClick;
        //buttons[0].onClick.AddListener(OnRotateButtonClick);
        //buttons[1].onClick.AddListener(OnRightButtonClick);
        buttons[0].onClick.AddListener(OnSpeedButtonClick);
        //buttons[3].onClick.AddListener(OnRotateButtonClick);
        pivot = transform.Find("Pivot").transform;

    }
    public void Init(Color color,Controller controller,GameManger gameManger)
    {
        foreach(Transform t in transform)
        {
            if (t.tag == "Block")
            {
                t.GetComponent<SpriteRenderer>().color = color;
            }
        }
        this.controller = controller;
        this.gameManger = gameManger;
    }

    public virtual void Fall()
    {
        Vector2 pos = transform.position;
        pos.y -= 1;
        transform.position = pos;
        if (controller.model.IsValidMapPosition(transform) == false)
        {
            pos.y += 1;
            transform.position = pos;
            isPause = true;
           
           bool isClear= controller.model.PlaceShape(transform);
            if (isClear)
            {
                controller.audioManger.PlayClearLineAudio();
            }
            gameManger.FallDown();
            return;
        }
        controller.audioManger.PlayDropAudio();
    }

    public void PauseGame()
    {
        isPause = true;
    }

    public void StartGame()
    {
        isPause = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPause)
        {
            return;
        }
            
        if (dropTimer <= 0)
        {
            Fall();
            
            dropTimer = 1f;
        }
        else
        {
            if (isSpeed==1)
            {
                dropTimer -= Time.deltaTime * 8;
            }
            if(isSpeed==2)
            {
                dropTimer -= Time.deltaTime * 16;
            }

            else
            {
                dropTimer -= Time.deltaTime;
            }
        }
    }

    public void OnLeftButtonClick()
    {
        if (isPause) return;
        
        Vector2 pos = transform.position;
        pos.x -= 1;
        transform.position = pos;

        if (controller.model.IsValidMapPosition(transform) == false)
        {
            pos.x += 1;
            transform.position = pos;
            return;
        }

        controller.audioManger.PlayMoveAudio();
    }


    public void OnRightButtonClick()
    {
        if (isPause) return;

        Vector2 pos = transform.position;
        pos.x += 1;
        transform.position = pos;

        if (controller.model.IsValidMapPosition(transform) == false)
        {
            pos.x -= 1;
            transform.position = pos;
            return;
        }
        controller.audioManger.PlayMoveAudio();
    }

    public void OnSpeedButtonClick()
    {
        if (isPause) return;
        isSpeed += 1;
        if(isSpeed>2)
        {
            isSpeed = 0;
        }
        gameManger.controller.view.isSpeedUIChange(isSpeed);
        
    }
    public void OnDownButtonClick(int x)
    {
        if (isPause) return;

        Vector2 pos = transform.position;
        pos.y -= x;
        transform.position = pos;
        if (controller.model.IsValidMapPosition(transform) == false)
        {
            for (int i=0; i<x;i++)
            {
                pos.y += 1;
                transform.position = pos;
                if (controller.model.IsValidMapPosition(transform) == true)
                {
                    controller.audioManger.PlayMoveAudio();
                    return;
                }
            }

        }
        controller.audioManger.PlayMoveAudio();
    }

    public void OnRotateButtonClick()
    {
        if (isPause) return;


        transform.RotateAround(pivot.position, Vector3.forward, 90f);
        if (controller.model.IsValidMapPosition(transform) == false)
        {
            transform.RotateAround(pivot.position, Vector3.forward, -90f);
            return;
        }
        controller.audioManger.PlayRotateAudio();
    }
}
