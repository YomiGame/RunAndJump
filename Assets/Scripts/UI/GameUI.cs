using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private GameObject GameOverText;
    private Text GameOverScoreText;
    private Text ScoreText;
    private GameObject GameRun;
    private Text Time;
    private Button runButton;
    private Button jumpButton;

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
        GameOverText = GameObject.Find("GameOverText");

        ScoreText = GameObject.Find("FarScore").GetComponent<Text>();
        GameOverScoreText = GameObject.Find("OverScore").GetComponent<Text>();
        GameRun = GameObject.Find("GameRun");
        Time = GameObject.Find("Time").GetComponent<Text>();
        GameObject.Find("GameOverButton").GetComponent<Button>().onClick.AddListener(RreturnBeginScene);
        runButton = GameObject.Find("Run").GetComponent<Button>();
        jumpButton = GameObject.Find("Jump").GetComponent<Button>();
        GameOverText.SetActive(false);
    }
    

    public void GameOverSetActive(bool Active,float Score)
    {
        GameOverText.SetActive(Active);
        GameOverScoreText.text = "You have run :"+Score.ToString()+" mile ";
        
    }

    public void ScoreUI(bool Active, float Score,int localtime)
    {
        GameRun.SetActive(Active);
        ScoreText.text = Score +"  mile";
        Time.text = "You have < " + localtime + " > min";
    }

    private void RreturnBeginScene()
    {
        SceneManager.LoadScene(0);
    }
}
