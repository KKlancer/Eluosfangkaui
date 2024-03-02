using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class HandSlide : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public Action RightMove;
    public Action LiftMove;
    public Action<int> DownMove;

    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    private Camera mainCamera;

    public float minSwipeDistance = 200f;
    private void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        
    }

    //public void OnDrag(PointerEventData eventData)
    //{
    //    // 获取拖拽时的当前位置
    //    fingerUpPosition = eventData.position;

    //    // 检查滑动
    //    CheckSwipe();
    //}

    void CheckSwipe()
    {
        float deltaX = fingerDownPosition.x-fingerUpPosition.x;
        float deltaY = fingerDownPosition.y-fingerUpPosition.y;
        if (Mathf.Abs(deltaX) > minSwipeDistance || Mathf.Abs(deltaY) > minSwipeDistance)
        {
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                // Horizontal swipe
                if (deltaX > 0)
                {
                    RightMove?.Invoke();
                    Debug.Log("Right Swipe");
                }
                else
                {
                    LiftMove?.Invoke();
                    Debug.Log("Left Swipe");
                }
            }
            else
            {
                // Vertical swipe
                if (deltaY > 0)
                {
                    Debug.Log("Up Swipe");
                }
                else
                {
                    DownMove?.Invoke(Mathf.Abs(Mathf.RoundToInt(deltaY/mainCamera.orthographicSize)));
                    Debug.Log("Down Swipe"+ Mathf.Abs(Mathf.RoundToInt(deltaY / mainCamera.orthographicSize)));
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        fingerUpPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        fingerDownPosition = eventData.position;
        CheckSwipe();
    }

    public void OnDrag(PointerEventData eventData)
    {
        return;
    }
}