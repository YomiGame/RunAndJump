using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private float PlayerScore;
    private GameObject GameOver;
    private Text GameOverScoreText;
    private Text ScoreText;
    private GameObject GameRun;
    private Text Time;
    private Button runButton;
    private Button jumpButton;
    //Rank
    private GameObject Rank;
    private Text RankText;
    private bool _rankBool;
    //Remember
    private GameObject RememberTip;
    private Text RememberText;
    private Text NameText;
    //Chat
    private GameObject ChatTip;
    private GameObject NoInternet;
    

    public bool RankBool
    {
        get { return _rankBool; }
        set { _rankBool = value; }
    }


    public Button RunButton
    {
        set { RunButton = runButton; }
        get { return runButton; }
    }
    public Button JumpButton
    {
        set { JumpButton = jumpButton; }
        get { return jumpButton; }
    }

    void Start()
    {
        PlayerScore = 0;
        GameOver = GameObject.Find("GameOver");
        ScoreText = GameObject.Find("FarScore").GetComponent<Text>();
        GameOverScoreText = GameObject.Find("OverScore").GetComponent<Text>();
        GameRun = GameObject.Find("GameRun");
        Time = GameObject.Find("Time").GetComponent<Text>();
        GameObject.Find("GameOverButton").GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(0));
        
        runButton = GameObject.Find("Run").GetComponent<Button>();
        jumpButton = GameObject.Find("Jump").GetComponent<Button>();
        
        
        //Rank
        Rank = GameObject.Find("Rank");
        RankText = GameObject.Find("RankText").GetComponent<Text>();
        _rankBool = false;
        GameObject.Find("RankButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            _rankBool = !_rankBool;
            Rank.SetActive(_rankBool);
        });
        GameObject.Find("ClearRankButton").GetComponent<Button>().onClick.AddListener(() => SystemManager.ClearRank(gameObject.GetComponent<GameUI>()));

        
        //Remember
        RememberTip = GameObject.Find("RememberTip");
        GameObject.Find("RememberButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            RememberTip.SetActive(true);
            RememberText.text = PlayerScore.ToString();
        });
        RememberText = GameObject.Find("RememberText").GetComponent<Text>();
        GameObject.Find("RememberExit").GetComponent<Button>().onClick.AddListener(() => RememberTip.SetActive(false));
        GameObject.Find("RememberSave").GetComponent<Button>().onClick.AddListener(() => SystemManager.SaveRank((int) PlayerScore,NameText.text));
        NameText = GameObject.Find("NameText").GetComponent<Text>();
        
        
        //Chat
        ChatTip = GameObject.Find("ChatTip");
        NoInternet = GameObject.Find("noInternet");
        GameObject.Find("ChatButton").GetComponent<Button>().onClick.AddListener(() => ChatTip.SetActive(true));
        GameObject.Find("ChatExit").GetComponent<Button>().onClick.AddListener(() => ChatTip.SetActive(false));
        

        
        //tips reset
        Rank.SetActive(false);
        RememberTip.SetActive(false);
        ChatTip.SetActive(false);
        
        //bashPath
        GameOver.SetActive(false);
    }

    public void ChatRoomOver(bool ChatActive)
    {
        NoInternet.SetActive(ChatActive);
    }

    public void GameOverSetActive(bool Active,float Score)
    {
        GameOver.SetActive(Active);
        GameOverScoreText.text = "You have run :"+Score.ToString()+" mile ";
    }

    public void ScoreUI(bool Active, float Score,int localtime)
    {
        GameRun.SetActive(Active);
        ScoreText.text = Score +"  mile";
        Time.text = "You have < " + localtime + " > min";
        PlayerScore = Score;
    }
    //SetRank
    public void SetDataToRank(List<PlayerRankeData> RankList)
    {
        if (RankList.Count > 0)
        {
            string Rank = "Rank" + "\n";
            for (int i = 0; i < RankList.Count; i++)
            {
                string rankData = "     "+RankList[i].socre + "    " + RankList[i].name+"\n";
                Rank = Rank + (i+1) + rankData;
            }
            RankText.text = Rank;
        }
        else
        {
            string Rank = "Rank" + "\n";
            RankText.text = Rank;
        }

    }

    
}
