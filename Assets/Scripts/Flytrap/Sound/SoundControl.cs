using UnityEngine;
using System.Collections;
using Fabric;
using System;
using com.rmc.projects.event_dispatcher;
using System.Collections.Generic;

public class SoundControl : EventDispatcherBase
{
    public List<GameObject> SoundHolders;
    protected GameObject SoundSet;
    protected int CurrentSoundID = -1;
    protected int NumOfsoundSets = 2;
    protected FabricManager FM;
    protected GameControl GC;
    protected float timeScale = 1.0f;
    protected bool FirstRound = true;
    protected bool LevelPlaying = false;
	// Use this for initialization
	void Start () {
        FM = Fabric.FabricManager.Instance;
        GC = FindObjectOfType<GameControl>();
        Init();
	}

    private void Init()
    {
        GC.eventDispatcher.addEventListener(SoundEvent.ON_LEVEL_START, OnLevelStartHandler);
        GC.eventDispatcher.addEventListener(SoundEvent.ON_INTRO_START, OnIntroStartHandler);
        GC.eventDispatcher.addEventListener(SoundEvent.ON_LEVEL_COMPLETE, OnLevelCompleteHandler);
        GC.eventDispatcher.addEventListener(SoundEvent.ON_SPECIAL_START, OnSpecialStartHandler);
        GC.eventDispatcher.addEventListener(SoundEvent.ON_SPECIAL_COMPLETE, OnSpecialCompleteHandler);
        GC.eventDispatcher.addEventListener(SoundEvent.ON_CAPTURE_START, OnCaptureStartHandler);
        GC.eventDispatcher.addEventListener(SoundEvent.ON_CAPTURE_COMPLETE, OnCaptureCompleteHandler);
        GC.eventDispatcher.addEventListener(SoundEvent.ON_GAME_OVER, OnGameOverHandler);
        GC.eventDispatcher.addEventListener(SoundEvent.ON_RESTART, OnRestartHandler);
        Loudness.OfListener = 3f;
    }

    void Update()
    {
        if(timeScale != Time.timeScale)
        {
            timeScale = Time.timeScale;
            Pause();
        }
    }

    void Pause()
    {    
        if(timeScale == 0.0f)
        {
            FM.Pause(true);
        }
        else
        {
            FM.Pause(false);
        }
    }



    private void OnCaptureCompleteHandler(IEvent iEvent)
    {
        SoundEvent se = iEvent as SoundEvent;
        int SoundID;
        if(se.NumOfCaptured > 1)
        {
            SoundID = se.NumOfCaptured;

            if (SoundID > 5)
                SoundID = 6;

            Fabric.EventManager.Instance.PostEvent("Capture", EventAction.SetSwitch, "Success" + SoundID);

            if (se.Special)
                Fabric.EventManager.Instance.PostEvent("SpecialSuccess");
        }
        else
        {
            SoundID = Mathf.RoundToInt( UnityEngine.Random.Range(1f, 2f) );
            Fabric.EventManager.Instance.PostEvent("Capture", EventAction.SetSwitch, "Fail" + SoundID);
        }
        Debug.Log(iEvent.type);
    }

    private void OnCaptureStartHandler(IEvent iEvent)
    {
        Debug.Log(iEvent.type);
        Fabric.EventManager.Instance.PostEvent("Capture", EventAction.SetSwitch, "Tick");
        Fabric.EventManager.Instance.PostEvent("Capture");
    }

    private void OnSpecialCompleteHandler(IEvent iEvent)
    {
        Debug.Log(iEvent.type);
        Fabric.EventManager.Instance.PostEvent("Loop", EventAction.SetSwitch, "BasicLoop");
    }

    private void OnSpecialStartHandler(IEvent iEvent)
    {
        Fabric.EventManager.Instance.PostEvent("Loop", EventAction.SetSwitch, "SpecialLoop");
        Debug.Log(iEvent.type);
    }

    private void AddMusic()
    {
       if(SoundSet != null)
        {
            Destroy(SoundSet);
            Debug.Log("soundset destroyed");
        }

        CurrentSoundID++;
        if (CurrentSoundID >= NumOfsoundSets)
            CurrentSoundID = 0;

        SoundSet = Instantiate(SoundHolders[CurrentSoundID], Vector3.zero, Quaternion.identity) as GameObject;
        SoundSet.transform.parent = transform;

        Fabric.EventManager.Instance.PostEvent("Loop");
    }

    private void OnLevelCompleteHandler(IEvent iEvent)
    {
        Fabric.EventManager.Instance.PostEvent("Loop", EventAction.SetSwitch, "Silent");
        Fabric.EventManager.Instance.PostEvent("LevelComplete");
        LevelPlaying = false;
        Debug.Log(iEvent.type);
    }

    private void OnGameOverHandler(IEvent iEvent)
    {
        Fabric.EventManager.Instance.PostEvent("Loop", EventAction.SetSwitch, "Silent");
        Fabric.EventManager.Instance.PostEvent("GameOver");
        LevelPlaying = false;
        Debug.Log(iEvent.type);
    }

    private void OnLevelStartHandler(IEvent iEvent)
    {
        if(!FirstRound)
         AddMusic();

        FirstRound = false;
        LevelPlaying = true;
        Fabric.EventManager.Instance.PostEvent("Loop", EventAction.SetSwitch, "BasicLoop");
        Debug.Log(iEvent.type);
    }

    private void OnIntroStartHandler(IEvent iEvent)
    {
        AddMusic();
        //Fabric.EventManager.Instance.PostEvent("Loop");
        Debug.Log(iEvent.type);
    }

    private void OnRestartHandler(IEvent iEvent)
    {
        if (!LevelPlaying)
        {
            FirstRound = true;
            AddMusic();
        }
            
    }

    private void OnBeatCallBack(double timeOffset)
    {
        //Debug.Log("Beat");
    }

    private void OnBarCallBack(double timeOffset)
    {
        //Debug.Log("Bar");
    }
}
