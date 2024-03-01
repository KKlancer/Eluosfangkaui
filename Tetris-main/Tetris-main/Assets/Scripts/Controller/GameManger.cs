using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    private bool isPause = true;
    public Shape[] shapes;
    public ShapeAi[] shapeAis;
    public Color[] colors;
    public Shape  currentShape=null;
    public ShapeAi currentShapeAi = null;
    public Controller controller;
    public GameObject blockHolder;
    public GameObject blockHolderAi;


    //方块生成挂点
    public  Transform PlayerBlockGenerateTop;
    public  Transform PlayerBlockGenerateButtom;

    public  Transform AiBlockGenerateTop;
    public  Transform AiBlockGenerateButtom;
    void Start()
    {
       // controller = GetComponent<Controller>();
    }

    public void StartGame()
    {
        if (currentShape != null)
            currentShape.StartGame();
        isPause = false;
    }
    public void PauseGame()
    {
        if (currentShape != null)
            currentShape.PauseGame();
        isPause = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isPause) return;
        if (currentShape == null)
        {
            SpawnShape();
        }
        if (currentShapeAi == null)
        {
            SpawnShapeAi();
        }


    }

    public void SpawnShape()
    {
        currentShape = Instantiate(shapes[Random.Range(0, shapes.Length)],PlayerBlockGenerateTop.position,Quaternion.identity, blockHolder.transform);
        currentShape.Init(colors[Random.Range(0, colors.Length)],controller,this);
    }
    public void SpawnShapeAi()
    {
        currentShapeAi = Instantiate(shapeAis[Random.Range(0, shapeAis.Length)], AiBlockGenerateTop.position, Quaternion.identity, blockHolder.transform);
        currentShapeAi.Init(colors[Random.Range(0, colors.Length)], controller, this);
    }

    public void FallDown()
    {
        if (controller.model.isUpdateScore)
        {
            controller.view.UpdateScoreUI(controller.model.Score, controller.model.BestScore);
        }

       foreach(Transform child in blockHolder.transform)
        {
            if (child.childCount <= 1)
            {
                Destroy(child.gameObject);
            }
        }
      
        if (controller.model.IsGameOver())
        {
           
            PauseGame();
            controller.view.ShowGameOverUI(controller.model.Score);
        } 
          currentShape = null;


    }
    public void FallDownAi()
    {
        currentShapeAi = null;
    }
    public void ClearCurrentShape()
    {
        if (currentShape != null)
        {
            Destroy(currentShape.gameObject);
            currentShape = null;
        }
        if (currentShapeAi != null)
        {
            Destroy(currentShapeAi.gameObject);
            currentShapeAi = null;
        }
    }
}
