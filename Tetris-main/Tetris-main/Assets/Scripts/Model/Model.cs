using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Model : MonoBehaviour
{
    public const int maxRows = 26;
    public const int maxColumns = 20;
    private int score = 0;

    public int Score { get { return score; } }
    private int bestScore = 0;
    public int BestScore { get { return bestScore; } }
    private int numbersGame = 0;
    public int NumbersGame { get { return numbersGame; } }


    private int isMute = 0;
    public int IsMute { get { return isMute; } }
    public bool isUpdateScore = false;
    
    //双方方块碰撞检测


    private Transform[,] map = new Transform[maxColumns, maxRows];

    /// <summary>
    /// 中间检测功能
    /// </summary>
    private static Transform CenterJudge;
    public static Action CenterJudgeMoved;
    private static List<List<float>> Player;
    private static List<List<float>> Ai;


    public void ClearHistory()
    {
        score = 0;
        bestScore = 0;
        numbersGame = 0;
        Save();
    }
    public void SetMute(int mute)
    {
        isMute = mute;
        Save();
    }
    private void Awake()
    {
        Load();
    }
    private void Start()
    {
        InitPlayerAndAi();
    }
    public void RestartGame()
    {
        InitPlayerAndAi();
        for(int i = 0; i < maxRows; i++)
        {
            for(int j = 0; j < maxColumns; j++)
            {
                if (map[j, i] != null)
                {
                    Destroy(map[j, i].gameObject);
                    map[j, i] = null;
                }
            }
        }
        score = 0;

    }
    public bool IsGameOver()
    {
        for(int i = 23; i< maxRows; i++)
        {
            for(int j = 0; j < maxColumns; j++)
            {
                if (map[j, i] != null)
                {
                    float x = i;
                    float y = j;
                    float result = x + y * 0.01f;
                    Debug.Log(Player[i][Player[i].Count - 1]);
                    Debug.Log(result);
                    bool PlayerIsWin=true;
                    foreach(float p in Player[i])
                    {
                        if (p == result)
                        {
                            PlayerIsWin = false;
                            Debug.Log("You Lost!" );
                        }
                    }
                    if(PlayerIsWin)
                    {
                        Debug.Log("You Win!" );
                    }

                    numbersGame++;
                    Save();
                    return true;
                }
                   
            }
        }
        return false;
    }
    public void Load()
    {
        bestScore= PlayerPrefs.GetInt("bestScore", 0);
        numbersGame = PlayerPrefs.GetInt("numbersGame", 0);
        isMute= PlayerPrefs.GetInt("isMute", 0);
    }
    //初始化player和ai数组
    private void InitPlayerAndAi()
    {
        CenterJudge = GameObject.Find("CenterJudgeUI").transform;
        CenterJudge.position = new Vector3(9.5f, 0.0f, 0.0f);
        Player = new List<List<float>>();
        Ai = new List<List<float>>();

        Player.Add(new List<float> { 0.00f, 0.01f, 0.02f, 0.03f, 0.04f, 0.05f, 0.06f, 0.07f, 0.08f, 0.09f });//0
        Player.Add(new List<float> { 1.00f, 1.01f, 1.02f, 1.03f, 1.04f, 1.05f, 1.06f, 1.07f, 1.08f, 1.09f });//1
        Player.Add(new List<float> { 2.00f, 2.01f, 2.02f, 2.03f, 2.04f, 2.05f, 2.06f, 2.07f, 2.08f, 2.09f });//2
        Player.Add(new List<float> { 3.00f, 3.01f, 3.02f, 3.03f, 3.04f, 3.05f, 3.06f, 3.07f, 3.08f, 3.09f });//3
        Player.Add(new List<float> { 4.00f, 4.01f, 4.02f, 4.03f, 4.04f, 4.05f, 4.06f, 4.07f, 4.08f, 4.09f });//4
        Player.Add(new List<float> { 5.00f, 5.01f, 5.02f, 5.03f, 5.04f, 5.05f, 5.06f, 5.07f, 5.08f, 5.09f });//5
        Player.Add(new List<float> { 6.00f, 6.01f, 6.02f, 6.03f, 6.04f, 6.05f, 6.06f, 6.07f, 6.08f, 6.09f });//6
        Player.Add(new List<float> { 7.00f, 7.01f, 7.02f, 7.03f, 7.04f, 7.05f, 7.06f, 7.07f, 7.08f, 7.09f });//7
        Player.Add(new List<float> { 8.00f, 8.01f, 8.02f, 8.03f, 8.04f, 8.05f, 8.06f, 8.07f, 8.08f, 8.09f });//8
        Player.Add(new List<float> { 9.00f, 9.01f, 9.02f, 9.03f, 9.04f, 9.05f, 9.06f, 9.07f, 9.08f, 9.09f });//9
        Player.Add(new List<float> { 10.00f, 10.01f, 10.02f, 10.03f, 10.04f, 10.05f, 10.06f, 10.07f, 10.08f, 10.09f });//10
        Player.Add(new List<float> { 11.00f, 11.01f, 11.02f, 11.03f, 11.04f, 11.05f, 11.06f, 11.07f, 11.08f, 11.09f });//11
        Player.Add(new List<float> { 12.00f, 12.01f, 12.02f, 12.03f, 12.04f, 12.05f, 12.06f, 12.07f, 12.08f, 12.09f });//12
        Player.Add(new List<float> { 13.00f, 13.01f, 13.02f, 13.03f, 13.04f, 13.05f, 13.06f, 13.07f, 13.08f, 13.09f });//13
        Player.Add(new List<float> { 14.00f, 14.01f, 14.02f, 14.03f, 14.04f, 14.05f, 14.06f, 14.07f, 14.08f, 14.09f });//14
        Player.Add(new List<float> { 15.00f, 15.01f, 15.02f, 15.03f, 15.04f, 15.05f, 15.06f, 15.07f, 15.08f, 15.09f });//15
        Player.Add(new List<float> { 16.00f, 16.01f, 16.02f, 16.03f, 16.04f, 16.05f, 16.06f, 16.07f, 16.08f, 16.09f });//16
        Player.Add(new List<float> { 17.00f, 17.01f, 17.02f, 17.03f, 17.04f, 17.05f, 17.06f, 17.07f, 17.08f, 17.09f });//17
        Player.Add(new List<float> { 18.00f, 18.01f, 18.02f, 18.03f, 18.04f, 18.05f, 18.06f, 18.07f, 18.08f, 18.09f });//18
        Player.Add(new List<float> { 19.00f, 19.01f, 19.02f, 19.03f, 19.04f, 19.05f, 19.06f, 19.07f, 19.08f, 19.09f });//19
        Player.Add(new List<float> { 20.00f, 20.01f, 20.02f, 20.03f, 20.04f, 20.05f, 20.06f, 20.07f, 20.08f, 20.09f });//20
        Player.Add(new List<float> { 21.00f, 21.01f, 21.02f, 21.03f, 21.04f, 21.05f, 21.06f, 21.07f, 21.08f, 21.09f });//21
        Player.Add(new List<float> { 22.00f, 22.01f, 22.02f, 22.03f, 22.04f, 22.05f, 22.06f, 22.07f, 22.08f, 22.09f });//22
        Player.Add(new List<float> { 23.00f, 23.01f, 23.02f, 23.03f, 23.04f, 23.05f, 23.06f, 23.07f, 23.08f, 23.09f });//23


        Ai.Add(new List<float> { 0.10f, 0.11f, 0.12f, 0.13f, 0.14f, 0.15f, 0.16f, 0.17f, 0.18f, 0.19f });//0
        Ai.Add(new List<float> { 1.10f, 1.11f, 1.12f, 1.13f, 1.14f, 1.15f, 1.16f, 1.17f, 1.18f, 1.19f });//1
        Ai.Add(new List<float> { 2.10f, 2.11f, 2.12f, 2.13f, 2.14f, 2.15f, 2.16f, 2.17f, 2.18f, 2.19f });//2
        Ai.Add(new List<float> { 3.10f, 3.11f, 3.12f, 3.13f, 3.14f, 3.15f, 3.16f, 3.17f, 3.18f, 3.19f });//3
        Ai.Add(new List<float> { 4.10f, 4.11f, 4.12f, 4.13f, 4.14f, 4.15f, 4.16f, 4.17f, 4.18f, 4.19f });//4
        Ai.Add(new List<float> { 5.10f, 5.11f, 5.12f, 5.13f, 5.14f, 5.15f, 5.16f, 5.17f, 5.18f, 5.19f });//5
        Ai.Add(new List<float> { 6.10f, 6.11f, 6.12f, 6.13f, 6.14f, 6.15f, 6.16f, 6.17f, 6.18f, 6.19f });//6
        Ai.Add(new List<float> { 7.10f, 7.11f, 7.12f, 7.13f, 7.14f, 7.15f, 7.16f, 7.17f, 7.18f, 7.19f });//7
        Ai.Add(new List<float> { 8.10f, 8.11f, 8.12f, 8.13f, 8.14f, 8.15f, 8.16f, 8.17f, 8.18f, 8.19f });//8
        Ai.Add(new List<float> { 9.10f, 9.11f, 9.12f, 9.13f, 9.14f, 9.15f, 9.16f, 9.17f, 9.18f, 9.19f });//9
        Ai.Add(new List<float> { 10.10f, 10.11f, 10.12f, 10.13f, 10.14f, 10.15f, 10.16f, 10.17f, 10.18f, 10.19f });//10
        Ai.Add(new List<float> { 11.10f, 11.11f, 11.12f, 11.13f, 11.14f, 11.15f, 11.16f, 11.17f, 11.18f, 11.19f });//11
        Ai.Add(new List<float> { 12.10f, 12.11f, 12.12f, 12.13f, 12.14f, 12.15f, 12.16f, 12.17f, 12.18f, 12.19f });//12
        Ai.Add(new List<float> { 12.10f, 12.11f, 12.12f, 12.13f, 12.14f, 12.15f, 12.16f, 12.17f, 12.18f, 12.19f });//13
        Ai.Add(new List<float> { 14.10f, 14.11f, 14.12f, 14.13f, 14.14f, 14.15f, 14.16f, 14.17f, 14.18f, 14.19f });//14
        Ai.Add(new List<float> { 15.10f, 15.11f, 15.12f, 15.13f, 15.14f, 15.15f, 15.16f, 15.17f, 15.18f, 15.19f });//15
        Ai.Add(new List<float> { 16.10f, 16.11f, 16.12f, 16.13f, 16.14f, 16.15f, 16.16f, 16.17f, 16.18f, 16.19f });//16
        Ai.Add(new List<float> { 17.10f, 17.11f, 17.12f, 17.13f, 17.14f, 17.15f, 17.16f, 17.17f, 17.18f, 17.19f });//17
        Ai.Add(new List<float> { 18.10f, 18.11f, 18.12f, 18.13f, 18.14f, 18.15f, 18.16f, 18.17f, 18.18f, 18.19f });//18
        Ai.Add(new List<float> { 19.10f, 19.11f, 19.12f, 19.13f, 19.14f, 19.15f, 19.16f, 19.17f, 19.18f, 19.19f });//19
        Ai.Add(new List<float> { 20.10f, 20.11f, 20.12f, 20.13f, 20.14f, 20.15f, 20.16f, 20.17f, 20.18f, 20.19f });//20
        Ai.Add(new List<float> { 21.10f, 21.11f, 21.12f, 21.13f, 21.14f, 21.15f, 21.16f, 21.17f, 21.18f, 21.19f });//21
        Ai.Add(new List<float> { 22.10f, 22.11f, 22.12f, 22.13f, 22.14f, 22.15f, 22.16f, 22.17f, 22.18f, 22.19f });//22
        Ai.Add(new List<float> { 23.10f, 23.11f, 23.12f, 23.13f, 23.14f, 23.15f, 23.16f, 23.17f, 23.18f, 23.19f });//23
    }

    //外部调用中间线
    public static void CenterJudgeMove(int unit)
    {
        if (unit == 0) return;
        if (unit < 0)
        {
            CenterJudge.transform.position += new Vector3(unit, 0, 0);
            for (int i = 0; i < -unit; i++)
            {
                for (int j = 0; j < 24; j++)
                {
                    Ai[j].Add(Player[j][Player[j].Count-1]);
                    Ai[j].Sort();
                    Player[j].RemoveAt(Player[j].Count-1);
                    //Debug.Log(Player[j][Player[j].Count + unit]+"+"+ Ai[j][0]);

                }
            }
        }
        if(unit>0)
        {
            CenterJudge.transform.position += new Vector3(unit, 0, 0);
            for (int i = 0; i < unit; i++)
            {
                for (int j = 0; j < 24; j++)
                {
                    Player[j].Add(Ai[j][i]);
                    Ai[j].RemoveAt(i);
                    //Debug.Log(Player[j][Player[j].Count-1]);
                }
            }
        }
        if(CenterJudgeMoved!=null)
        {
            CenterJudgeMoved.Invoke();
        }

    }

    private void ColorCheck()
    {

    }


    public void Save()
    {
        
        PlayerPrefs.SetInt("bestScore", bestScore);
        PlayerPrefs.SetInt("numbersGame", numbersGame);
        PlayerPrefs.SetInt("isMute", isMute);

    }
    public bool IsValidMapPosition(Transform t)
    {
        if(t.tag=="Player")
        {
            foreach (Transform child in t)
            {
                if (child.tag != "Block") continue;
                Vector2 pos = child.position.Round();
                if (IsInsiadeMap(pos) == false) return false;
                if (map[(int)pos.x, (int)pos.y] != null)
                {
                    return false;

                }
            }
            return true;

        }
        else
        {
            foreach (Transform child in t)
            {
                if (child.tag != "Block") continue;
                Vector2 pos = child.position.Round();
                if (IsInsiadeMapAi(pos) == false) return false;
                if (map[(int)pos.x, (int)pos.y] != null)
                {
                    return false;

                }
            }
            return true;
        }

    }
    public bool PlaceShape(Transform t)
    {
        foreach(Transform child in t)
        {
            if (child.tag != "Block") continue;
            Vector2 pos = child.position.Round();
            map[(int)pos.x, (int)pos.y] = child;
        }

       return CheakMap();

    }
    public bool CheakMap()
    {
        int count = 0;
        for(int i = 0; i < 23; i++)
        {
            if (IsRowFall(i))
            {
                DeleteRow(i);
                MoveDownAll(i);
                i--;
                count++;
            }
        }
        if (count > 0)
        {
            isUpdateScore = true;
            score += count * 100;
            if (score > bestScore)
            {
                bestScore = score;
                Save();
            }
        }
        return count > 0;
    }
    public bool IsRowFall(int row)
    {
        for (int i = 0; i < maxColumns-Ai[0].Count; i++)
        {
            if (map[i,row] == null)
                return false;
        }
        return true;
    }
    public void DeleteRow(int row)
    {
        for (int i = 0; i < maxColumns - Ai[0].Count; i++)
        {
            Destroy(map[i, row].gameObject);
            map[i, row] = null;
        }
    }

    public void MoveDownAll(int row)
    {
        for(int i = row; i < 23; i++)
        {
            for(int j = 0; j < maxColumns- Ai[0].Count; j++)
            {
                if (map[j, i] != null)
                {
                    map[j, i - 1] = map[j, i];
                    map[j, i] = null;
                    map[j, i - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }


    //loseControll
    public bool IsLoseControll(Vector2 pos, Vector2[] OtherSide)
    {
        return false;
    }
    //
    public bool IsInsiadeMap(Vector2 pos)
    {
        int[] centerJudgePos = {23,22,21,20,19,18,17,16,15};
        bool posIsInJudge = false;
        foreach (int p in centerJudgePos)
        {
            if (pos.y == p)
            {
                posIsInJudge = true;

            }                
        }
        if (posIsInJudge == true)
        {
            return pos.x >= 0 && pos.y >= 0 && pos.x < maxColumns - Ai[0].Count;
        }
        else
            return pos.x >= 0 && pos.y >= 0 && pos.x < maxColumns;
    }

    public bool IsInsiadeMapAi(Vector2 pos)
    {

        int[] centerJudgePos = { 23, 22, 21, 20, 19, 18, 17, 16, 15 };
        bool posIsInJudge = false;
        foreach (int p in centerJudgePos)
        {
            if (pos.y == p)
            {
                posIsInJudge = true;

            }
        }
        if (posIsInJudge == true)
        {
            return pos.x < maxColumns && pos.y >= 0 && pos.x >= maxColumns - Ai[0].Count;
        }
        else
            return pos.x < maxColumns && pos.y >= 0 && pos.x >= 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
