using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class HandSlide : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler,IPointerClickHandler
{
    //�¼�
    public Action RightMove;
    public Action LiftMove;
    public Action<int> DownMove;
    public Action DoubleClick;
    //
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    private Camera mainCamera;
    //
    private Vector2 doubleClickPos;
    private float doubleClickTime = 0.3f; // ����˫��ʱ����
    private float lastClickTime = -1f;

    public float minSwipeDistance = 200f;
    private void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        
    }

    //public void OnDrag(PointerEventData eventData)
    //{
    //    // ��ȡ��קʱ�ĵ�ǰλ��
    //    fingerUpPosition = eventData.position;

    //    // ��黬��
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - lastClickTime < doubleClickTime&&Vector2.Distance(doubleClickPos,eventData.position)<=1f)
        {
            // ������ִ��˫�������ĺ���
            DoubleClick?.Invoke();
            //
            lastClickTime = 0f;
        }
        else
        {
            // ����ʱ��¼��ǰ���ʱ��
            lastClickTime = Time.time;
            doubleClickPos = eventData.position;
        }
    }
}