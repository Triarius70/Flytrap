using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GUIControl : MonoBehaviour {
    public Text ScoreText;
    public Text TimerText;
    public Text InfoText;
    public Button MenuBtn;
    public GameObject MenuPanel;
    public Image MenuBg;
    public Button Credits;
    public Button MoreGames;
    public Button Skilla;
    public Button Reset;
    public Button Back;
    public GameObject CreditsPanel;
    public Image CreditsBg;
    public Button CreditsBack;
    public Transform[] BonusTransforms;
    public GameObject[] Fails;
    public GameObject[] DisabledFails;
    protected List<GameObject> MenuButtons;
    protected float score = 0f;
    protected float tempScore = 0f;
    protected float scoreLerpProgress;
    protected float scoreLerpTime = 1f;
    protected int FailsShown = 0;

    protected Hashtable ShowByScaling;
    protected Hashtable LoopByScaling;
    protected Hashtable HHide;
    protected string LoopByScalingID = "LoopByScalingTween";
    //protected Vector3 TopCenter
    // Use this for initialization
    void Start () {

        Init();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateScore();

        //if (Input.GetKey(KeyCode.Escape))
        //    System.Diagnostics.Process.GetCurrentProcess().Kill();

    }

    protected void Init()
    {
        InfoText.text = "";
        ScoreText.gameObject.SetActive(false);
        //TimerText.gameObject.SetActive(false);
        
        ShowByScaling = iTween.Hash("scale", new Vector3(0.01f, 0.01f, 1f), "time", 0.7f, "easetype", iTween.EaseType.easeOutBack, "oncomplete", "LoopScaleInfoText", "oncompletetarget", gameObject);
        LoopByScaling = iTween.Hash("name",LoopByScalingID, "scale", new Vector3(0.95f, 0.95f, 1f), "time", 1f, "easetype", iTween.EaseType.easeOutSine, "looptype", iTween.LoopType.pingPong);
        HHide = iTween.Hash("r", 0.0f, "g", 0.0f, "b", 0.0f, "a", 0.0f, "time", 0.0f);
        
        InitButtons();
        HideFailGraphs();
    }

    private void InitButtons()
    {
        
        MenuPanel.SetActive(false);  
        MenuButtons = new List<GameObject>();
        MenuButtons.Add(Credits.gameObject);
        MenuButtons.Add(MoreGames.gameObject);
        MenuButtons.Add(Skilla.gameObject);
        MenuButtons.Add(Reset.gameObject);
        MenuButtons.Add(Back.gameObject);

        Credits.onClick.AddListener( OnCreditsHandler );
        MoreGames.onClick.AddListener(OnMoreGamesHandler);
        Skilla.onClick.AddListener(OnSkillaHandler);
        Reset.onClick.AddListener(OnResetHandler);
        Back.onClick.AddListener(OnBackHandler);

        CreditsPanel.SetActive(false);
        CreditsBack.onClick.AddListener(OnCreditsBackHandler);
    }

    private void OnResetHandler()
    {
        GameControl.Instance.Pause(true);
    }

    private void OnCreditsBackHandler()
    {
        SetPanel(true, MenuPanel, MenuBg.gameObject);
        SetPanel(false, CreditsPanel, CreditsBg.gameObject);
    }

    private void OnBackHandler()
    {
        GameControl.Instance.Pause();
    }

    private void OnSkillaHandler()
    {
        Application.OpenURL("http://www.skillagame.com");
    }

    private void OnMoreGamesHandler()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=7599989440356532008");
    }

    private void OnCreditsHandler()
    {
        SetPanel(false, MenuPanel, MenuBg.gameObject);
        SetPanel(true, CreditsPanel, CreditsBg.gameObject);
    }

    protected void UpdateScore()
    {
        if (scoreLerpProgress < scoreLerpTime)
        {
            scoreLerpProgress += Time.deltaTime; //Use some easing function?
            if (scoreLerpProgress > scoreLerpTime)
                scoreLerpProgress = scoreLerpTime;

            score = Mathf.Ceil(Mathf.Lerp(score, tempScore, scoreLerpProgress));
        }

        ScoreText.text = "SCORE: " + Mathf.CeilToInt(score).ToString();
    }

    //Tween complete methods
    protected void LoopScaleInfoText()
    {
        iTween.ScaleTo(InfoText.gameObject, LoopByScaling);
    }

    protected void SetVisiblity(GameObject go, bool visiblity)
    {
        go.SetActive(visiblity);
    }

    protected void HideInfoText()
    {
        InfoText.text = "";
        iTween.StopByName("LoopByScalingTween");
    }


    //Public methods

    public void ShowCaptured(Vector3 pos, int id)
    {
        F3DPool.instance.Spawn(BonusTransforms[id], pos, Quaternion.identity, null);
    }

    public void ShowStartScreen(bool show)
    {
        Debug.Log("ShowStartScreen");
        if (show)
        {
            InfoText.text = "Touch anywhere to start game";
            iTween.ScaleFrom(InfoText.gameObject, ShowByScaling);
        }
        else
        {
            HideInfoText();
        }
    }

    private void HideFailGraphs()
    {
        for (int i = 0; i < Fails.Length; i++)
        {
            Fails[i].SetActive(false);
            DisabledFails[i].SetActive(false);
        }
    }

    private void ShowFailGraphs()
    {
        for (int i = 0; i < Fails.Length; i++)
        {
            Fails[i].SetActive(false);

            DisabledFails[i].gameObject.SetActive(true);
            iTween.MoveFrom(DisabledFails[i], iTween.Hash("position", new Vector3(DisabledFails[i].transform.position.x, 200, 0f), "time", 1f, "delay", i * 0.05f)); //new Vector3(DisabledFails[i].gameObject.transform.position.x, 200, 0f), 1f);
        }

        FailsShown = -1;
    }

    public void ShowFailGraph()
    {
        FailsShown++;
        GameObject fail = Fails[FailsShown];
        fail.SetActive(true);
        iTween.ScaleFrom(fail, iTween.Hash("scale", new Vector3(0.01f, 0.01f, 1f), "time", 0.7f, "easetype", iTween.EaseType.easeOutBack));
    }

    public void ShowGameplay(bool show, bool reset = false)
    {
     if(show)
        {
            if(reset)
                score = tempScore = 0f;

            ScoreText.gameObject.SetActive(true);
            //TimerText.gameObject.SetActive(true);
            ScoreText.text = "SCORE: " + Mathf.CeilToInt(score).ToString();
            iTween.MoveFrom(ScoreText.gameObject, new Vector3(ScoreText.gameObject.transform.position.x, 200, 0f), 1f);
            //iTween.MoveFrom(TimerText.gameObject, new Vector3(TimerText.gameObject.transform.position.x, 200, 0f), 1f);
            //MenuBtn.gameObject.SetActive(true);
            //iTween.MoveFrom(MenuBtn.gameObject, new Vector3(-200, MenuBtn.gameObject.transform.position.y, 0f), 1f);
            ShowFailGraphs();

            HideInfoText();
        }
     else
        {
            ScoreText.gameObject.SetActive(false);
            TimerText.gameObject.SetActive(false);
        }
    }

    public void SetTimer(float timeInSeconds)
    {
        //int fraction = Mathf.RoundToInt(timeInSeconds * 10);
        //fraction = fraction % 10;
       // TimerText.text = string.Format("{0:00}:{1:00}:{2:00}", Mathf.Floor(timeInSeconds/60), timeInSeconds % 60, fraction);
    }


    public void SetScore(float val)
    {
        tempScore = val;
        scoreLerpProgress = 0f;
    }

    public void SetLevelComplete(bool show, int level = 0)
    {
        if (show)
        {
            InfoText.text = "LEVEL " + level + " COMPLETE!\n<color=#000000ff>TOUCH ANYWHERE TO CONTINUE</color>";
            iTween.ScaleFrom(InfoText.gameObject, ShowByScaling);
        }
        else
        {
            HideInfoText();
        }
    }

    public void SetGameOver()
    {
            InfoText.text = "GAME OVER!\n<color=#000000ff>TOUCH ANYWHERE TO CONTINUE</color>";
            iTween.ScaleFrom(InfoText.gameObject, ShowByScaling);
    }

    public void SetMenu(bool open)
    {

        SetPanel(open, MenuPanel, MenuBg.gameObject);
    }

    protected void SetPanel(bool open, GameObject panel, GameObject menu)
    {
        float BgShowTime = 0.7f;
        float BgHideTime = 0.35f;

        if (open)
        {
            panel.SetActive(true);
            menu.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
            iTween.ScaleTo(menu, iTween.Hash("scale", new Vector3(1f, 1f, 1f), "time", BgShowTime, "ignoretimescale", true, "easetype", iTween.EaseType.easeOutBack));
        }
        else
        {
            iTween.ScaleTo(menu, iTween.Hash("scale", new Vector3(0.01f, 0.01f, 1f), "time", BgHideTime, "ignoretimescale", true, "easetype", iTween.EaseType.easeInBack, "oncomplete", "OnMenuHideComplete", "oncompleteparams",panel, "oncompletetarget", gameObject));
        }
    }

    protected void OnMenuHideComplete(GameObject panel)
    {
        panel.SetActive(false);
    }
}
