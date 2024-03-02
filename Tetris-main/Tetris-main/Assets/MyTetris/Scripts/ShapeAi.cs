using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeAi : Shape
{
    public static int isSpeedAi = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
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
            if (isSpeedAi == 1)
            {
                dropTimer -= Time.deltaTime * 8;
            }
            if (isSpeedAi == 2)
            {
                dropTimer -= Time.deltaTime * 16;
            }

            else
            {
                dropTimer -= Time.deltaTime;
            }
        }
    }
    public override void Fall()
    {
        Vector2 pos = transform.position;
        pos.y -= 1;
        transform.position = pos;
        if (controller.model.IsValidMapPosition(transform) == false)
        {
            pos.y += 1;
            transform.position = pos;
            isPause = true;

            bool isClear = controller.model.PlaceShape(transform);
            if (isClear)
            {
                controller.audioManger.PlayClearLineAudio();
            }
            gameManger.FallDownAi();
            return;
        }
        controller.audioManger.PlayDropAudio();
    }
}
