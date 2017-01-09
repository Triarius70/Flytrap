using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using com.rmc.projects.event_dispatcher;
//using System;
using System.Collections.Generic;
using System;

public enum GameState
{
    Gameplay,
    StartScreen,
    Menu,
    Wait
}

public struct CreatureParams
{
    public List<Transform> CreatureTypes;
    public int NumberOfCreatures;
}

public class GameControl : EventDispatcherBase {

    public List<LevelData> LevelDataList;
    protected LevelData levelData;
    public static GameControl Instance;
    protected int LevelIndex = -1;
    protected int Score = 0;
    protected int LevelScore = 0;
    protected int CapturesUsed = 0;
    protected TouchDragControl TouchDragControl;
    protected F3DPool ObjectPool;
    protected GUIControl GuiControl;
    protected int GameTimerID;
    protected float GameTimerTick = 0.04f;
    protected float GameTime;
    public GameState State = GameState.Wait;
    public GameState OnPausedState;
    protected int SpecialMod = 2;
    protected int Fails = 0;
    protected int FailMax = 3;
    protected bool LevelPassed = true;

    //public methods
    void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus)
            Pause();
    }

    public void Pause(bool reset = false)
    {
        Debug.Log("Pause");
        if (Time.timeScale == 1.0f)
        {
            TouchDragControl.TouchEnabled = false;
            GuiControl.SetMenu(true);
            OnPausedState = State;
            State = GameState.Menu;
            Time.timeScale = 0.0f;
            
        }
        else
        {
            if(reset)
            {
                ResetGame();
                return;
            }

            if(OnPausedState == GameState.Gameplay)
             TouchDragControl.TouchEnabled = true;

            GuiControl.SetMenu(false);
            State = OnPausedState;
            Debug.Log(State);
            Time.timeScale = 1.0f;
        }
    }

    // Use this for initialization
    void Start () {
        Instance = this;
        ObjectPool = GetComponent<F3DPool>();
        TouchDragControl = FindObjectOfType<TouchDragControl>();
        GuiControl = FindObjectOfType<GUIControl>(); 
        Invoke("Init", 1f);
    }

    protected void ResetGame()
    {
        Time.timeScale = 1.0f;
        Score = 0;
        LevelIndex = -1;
        LevelPassed = true;
        State = GameState.StartScreen;
        GuiControl.ShowStartScreen(true);
        eventDispatcher.dispatchEvent(new SoundEvent(SoundEvent.ON_RESTART));
        GuiControl.SetMenu(false);
    }

    public void OnStartGameRequest()
    {
         if (LevelPassed)
        {
            NextLevel();
        }
        else
        {
            Score -= LevelScore;
        }    

            StartGame();              
    }

    protected void Init()
    {
        TouchDragControl.eventDispatcher.addEventListener(CaptureEvent.ON_CAPTURE_START, OnCaptureStartHandler);
        TouchDragControl.eventDispatcher.addEventListener(CaptureEvent.ON_TAP, OnTapHandler);
        AssetManager.Instance.eventDispatcher.addEventListener(CaptureEvent.ON_CAPTURE_SUCCESS, OnCaptureCompleteHandler);
        AssetManager.Instance.eventDispatcher.addEventListener(CaptureEvent.ON_CAPTURE_FAIL, OnCaptureCompleteHandler);
        AssetManager.Instance.eventDispatcher.addEventListener(GameEvent.ON_LEVEL_COMPLETE, OnLevelCompleteHandler);
        AssetManager.Instance.eventDispatcher.addEventListener(CreatureEvent.ON_SPECIAL_START, OnSpecialStartHandler);
        AssetManager.Instance.eventDispatcher.addEventListener(CreatureEvent.ON_SPECIAL_COMPLETE, OnSpecialCompleteHandler);
        State = GameState.StartScreen;
        GuiControl.ShowStartScreen(true);
        eventDispatcher.dispatchEvent(new SoundEvent(SoundEvent.ON_INTRO_START));
    }



    private void OnTapHandler(IEvent iEvent)
    {
        if(State == GameState.StartScreen)
        {
            Debug.Log(State);
            OnStartGameRequest();
        }
            
    }

    protected void EnableTouch()
    {
        TouchDragControl.TouchEnabled = !TouchDragControl.TouchEnabled;
    }

    private void OnSpecialCompleteHandler(IEvent iEvent)
    {
        eventDispatcher.dispatchEvent(new SoundEvent(SoundEvent.ON_SPECIAL_COMPLETE));
    }

    private void OnSpecialStartHandler(IEvent iEvent)
    {
        eventDispatcher.dispatchEvent(new SoundEvent(SoundEvent.ON_SPECIAL_START));
    }

    protected void StartGame()
    {
        State = GameState.Gameplay;
        CreatureParams cp;
        LevelScore = 0;
        levelData = LevelDataList[LevelIndex];

        if (levelData.Type == LevelType.Basic)
        {
            cp = GetCreatureParams(levelData);
        }
        else if(levelData.Type == LevelType.Random)
        {
            cp = GetRandomCreatureParams(levelData);
        }
        else
        {
            cp = GetRandomCreatureParams(levelData);
        }

        AssetManager.Instance.CreatureTransforms = cp.CreatureTypes.ToArray();
        AssetManager.Instance.CreaturesNum = cp.NumberOfCreatures;

        Debug.Log("types:" + cp.CreatureTypes.Count + " creaturesPerType:" + cp.NumberOfCreatures);

        AssetManager.Instance.StartGame();
        GameTime = 0f;
        GameTimerID = F3DTime.time.AddTimer(GameTimerTick, OnTimer);
        GuiControl.SetScore(Score);
        GuiControl.ShowGameplay(true);
        eventDispatcher.dispatchEvent(new SoundEvent(SoundEvent.ON_LEVEL_START));
        LevelPassed = false;
        Fails = 0;
        Invoke("EnableTouch", 1f);
    }

    protected void NextLevel()
    {
        LevelIndex++;
        if (LevelIndex >= LevelDataList.Count)
            LevelIndex = 0;
    }

    protected CreatureParams GetCreatureParams(LevelData data)
    {
        CreatureParams cp = new CreatureParams { };
        cp.CreatureTypes = data.AvailableCreatures;
        cp.NumberOfCreatures = data.NumberOfCreaturesPerType;

        return cp;
    }

    protected CreatureParams GetRandomCreatureParams(LevelData data)
    {
        CreatureParams cp = new CreatureParams { };

        int NumberOfCreatureTypes = data.NumOfRandomCreatures;
        int NumberOfCreatures = data.NumberOfCreaturesPerType;

        List<Transform> CreatureTypes = new List<Transform>();

        while (CreatureTypes.Count < NumberOfCreatureTypes)
        {
            Transform CreatureTransform = AssetManager.Instance.AllCreatureTransforms[Mathf.FloorToInt(UnityEngine.Random.Range(0f, AssetManager.Instance.AllCreatureTransforms.Count))];

            if (!CreatureTypes.Contains(CreatureTransform))
            {
                CreatureTypes.Add(CreatureTransform);
            }
        }

        cp.CreatureTypes = CreatureTypes;
        cp.NumberOfCreatures = data.NumberOfCreaturesPerType;
        return cp;
    }


    protected CreatureParams GetFreeRandomCreatureParams(LevelData data)
    {
        CreatureParams cp = new CreatureParams { };

        int NumberOfCreatureTypes = Mathf.CeilToInt(UnityEngine.Random.Range(2f, AssetManager.Instance.AllCreatureTransforms.Count / 2));
        int NumberOfCreatures = Mathf.CeilToInt(UnityEngine.Random.Range(2f, 10f));
        List<Transform> CreatureTypes = new List<Transform>();

        while (CreatureTypes.Count < NumberOfCreatureTypes)
        {
            Transform CreatureTransform = AssetManager.Instance.AllCreatureTransforms[Mathf.FloorToInt(UnityEngine.Random.Range(0f, AssetManager.Instance.AllCreatureTransforms.Count))];

            if (!CreatureTypes.Contains(CreatureTransform))
            {
                CreatureTypes.Add(CreatureTransform);
            }
        }

        cp.CreatureTypes = data.AvailableCreatures;
        cp.NumberOfCreatures = data.NumberOfCreaturesPerType;
        return cp;
    }

    private void OnLevelCompleteHandler(IEvent iEvent)
    {
        LevelPassed = true;     
        F3DTime.time.RemoveTimer( GameTimerID );       
        eventDispatcher.dispatchEvent(new SoundEvent(SoundEvent.ON_LEVEL_COMPLETE));
        EnableTouch();
        State = GameState.Wait;
        Invoke("ShowLevelCompleted", 1.0f);
        Invoke("RestartMusic", 8.0f);
    }

    private void ShowLevelCompleted()
    {
        GuiControl.SetLevelComplete(true, levelData.LevelNum);
        State = GameState.StartScreen;
    }

    private void RestartMusic()
    {
        eventDispatcher.dispatchEvent(new SoundEvent(SoundEvent.ON_RESTART));
    }

    private void GameOver()
    {
        F3DTime.time.RemoveTimer(GameTimerID);
        eventDispatcher.dispatchEvent(new SoundEvent(SoundEvent.ON_GAME_OVER));
        EnableTouch();
        State = GameState.Wait;
        Invoke("ShowGameOver", 1.0f);
        Invoke("RestartMusic", 8.0f);
    }

    private void ShowGameOver()
    {
        GuiControl.SetGameOver();
        State = GameState.StartScreen;
    }

    private void OnCaptureStartHandler(IEvent iEvent)
    {
        eventDispatcher.dispatchEvent(new SoundEvent(SoundEvent.ON_CAPTURE_START));   
    }

    private void OnCaptureCompleteHandler(IEvent iEvent)
    {
        CaptureEvent bEvent = iEvent as CaptureEvent;
        if( iEvent.type == CaptureEvent.ON_CAPTURE_SUCCESS )
        {
            int pointsToAdd = bEvent.NumOfCaptured * bEvent.NumOfCaptured * 1000;

            if (bEvent.Special)
                pointsToAdd *= SpecialMod;

            Score += pointsToAdd;
            LevelScore += pointsToAdd;
            GuiControl.SetScore(Score);
            int bonusID = bEvent.NumOfCaptured - 2;
            if (bonusID > 5)
                bonusID = 5;

            GuiControl.ShowCaptured(bEvent.PosOfNew, bonusID);
        }
        else
        {
            Fails++;
            GuiControl.ShowFailGraph();
            if (Fails >= FailMax)
                GameOver();
        }


            eventDispatcher.dispatchEvent(new SoundEvent( SoundEvent.ON_CAPTURE_COMPLETE, bEvent.NumOfCaptured, bEvent.Special ));
    }

    protected void OnTimer()
    {
        GameTime += GameTimerTick;
        
        GuiControl.SetTimer(GameTime);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }
}
