using UnityEngine;
using System.Collections;

public class BonusItem : MonoBehaviour {
    protected Vector2 InitialScale;
    protected SpriteRenderer render;
    public float ShowPhase1Time = 2f;
    public float ShowPhase2Time = 1.0f;
    public iTween.EaseType ShowPhase1Ease;
    public iTween.EaseType ShowPhase2Ease;
    // Use this for initialization
    void Awake () {
        InitialScale = transform.localScale;
        render = gameObject.GetComponent<SpriteRenderer>();
	}

    // OnSpawned called by pool manager 
    public void OnSpawned()
    {
        iTween.ScaleFrom(gameObject, iTween.Hash("scale", new Vector3(0.01f, 0.01f, 1f), "time", ShowPhase1Time, "easetype", ShowPhase1Ease, "oncomplete", "OnShowComplete", "oncompletetarget", gameObject));      
        iTween.FadeTo(gameObject, iTween.Hash("alpha", 0f, "delay",ShowPhase1Time - ShowPhase2Time, "time", ShowPhase2Time, "easetype", ShowPhase2Ease));
        iTween.MoveTo(gameObject, iTween.Hash("position", Vector3.zero, "time", ShowPhase1Time - 0.04f, "easetype", ShowPhase1Ease));
    }

    protected void OnShowComplete()
    {
       // OnHideComplete();
        F3DPool.instance.Despawn(transform);
    }

    //protected void OnHideComplete()
    //{
    //    F3DPool.instance.Despawn(transform);
    //}

    // OnDespawned called by pool manager 
    public void OnDespawned()
    {
        //Debug.Log("OnDespawned");
        iTween.FadeTo(gameObject, 1f, 0f);
        transform.localScale = InitialScale;
    }
}
