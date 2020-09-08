using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;


public class SystemManager : MonoBehaviour
{
    private static SystemManager _instance = null;
    private SystemManager()
    {
        
    }
    
    //cube
    private Vector3 cubePosition;
    private CubeMove cubeMoveCS;
    
    private Scene localScene;
    public Vector3 CubePosition
    {
        set { CubePosition = cubePosition; }
        get { return cubePosition; }
    }

    //CreateManager
    public static SystemManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new SystemManager();
        }
        return _instance;
    }
    //UI
    private GameUI _gameUi;

    private GameObject BeginButton;
    private Text WaitText;

    private string LoadSchedule;

    private bool internetboolen;


    private bool _rankActive;

    private bool _rememberInbool;
    //Scene
    private AsyncOperation localasync;
    
    //time
    private int totaltime;//通过时间
    
    //save
    private static DoLeaderBoard _leaderBoard;
    private List<PlayerRankeData> _rankDataList;
    void Start()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
        
        DontDestroyOnLoad(this.gameObject);
        _rankActive = false;//default RankActive
        _rememberInbool = true;//default rememberInputBool
        internetboolen = true;//暂时
    }
    //Global
    private bool gameOver;

    public bool GameOver
    {
        set { GameOver = gameOver; }
        get { return gameOver; }
    }

    // Update is called once per frame
    void Update()
    {
        localScene = SceneManager.GetActiveScene();
        if (localScene.buildIndex == 0)
        {
            if (BeginButton == null)
            {
                BeginButton = GameObject.Find("BeginButton");
                BeginButton.GetComponent<Button>().onClick.AddListener(BeginTheGame);
            }

            if (WaitText == null)
            {
                WaitText = GameObject.Find("WaitText").GetComponent<Text>();
            }

            if (localasync == null) { return; }
            float targetValue = localasync.progress;
            if (localasync.progress >= 0.9f)
            {
                //值最大为0.9
                targetValue = 1.0f;
                localasync.allowSceneActivation = true;
            }


            LoadSchedule = ((int)(targetValue * 100)).ToString() + "%";
            WaitText.text = "Waiting >>>>>" + LoadSchedule;
            

        }else if (localScene.buildIndex == 1)
        {
            if (cubeMoveCS == null)
            {
                cubeMoveCS = GameObject.Find("Cube").GetComponent<CubeMove>();
            }
            if(_leaderBoard == null)
            {
                _leaderBoard = gameObject.GetComponent<DoLeaderBoard>();
                _rankDataList = _leaderBoard.RankJson();
            }
            if (_gameUi == null)
            {
                _gameUi = GameObject.Find("Manager").GetComponent<GameUI>();
                _gameUi.JumpButton.onClick.AddListener(cubeMoveCS.BoxJump);
                _gameUi.RunButton.onClick.AddListener(cubeMoveCS.BoxRun);
            }
            cubePosition = cubeMoveCS.CubeVector3;
            bool localgameOver = cubeMoveCS.GameOver;
            _gameUi.GameOverSetActive(localgameOver,cubePosition.x);
            _gameUi.ScoreUI(!localgameOver,cubePosition.x,totaltime);
            _gameUi.ChatRoomOver(internetboolen);
            //rank
            if (_gameUi.RankBool == _rankActive)
            {
                _rankActive = !_gameUi.RankBool;
                _gameUi.SetDataToRank(_rankDataList);
            }
            
            

        }


    }

    private void BeginTheGame()
    {
        StartCoroutine(AsyncLoading());
        BeginButton.SetActive(false);
        totaltime = 30;
        gameOver = false;
    }

    
    IEnumerator AsyncLoading()

    {

        //异步加载场景

        localasync = SceneManager.LoadSceneAsync(1);   

        //阻止当加载完成自动切换

        localasync.allowSceneActivation = false;      



        yield return localasync;      

    }

    public void BeginRecordTime()
    {
        StartCoroutine(Record_Time());
    }
    IEnumerator Record_Time()
    {
        
        yield return new WaitForSecondsRealtime(1f);
        
        if (totaltime > 0 )
        {
            totaltime--;
            StartCoroutine(Record_Time());
        }
        else
        {
            gameOver = true;
        }
    }
    

    public static void SaveRank(int score,string name)
    {
        _leaderBoard.SaveToBoard(score, name);
    }

    public static void ClearRank(GameUI gameUi)
    {
        _leaderBoard.ClearRankJsonFile();
        _leaderBoard.UpdataRankJson();
        gameUi.SetDataToRank(_leaderBoard.RankJson());
        
    }

}
