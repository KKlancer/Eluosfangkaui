using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManger gameManger;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Model.CenterJudgeMove(1);
            gameManger.PlayerBlockGenerateTop.position +=new Vector3(1,0,0);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Model.CenterJudgeMove(-1);
           gameManger.PlayerBlockGenerateTop.position -= new Vector3(1, 0, 0);
        }
    }
}
