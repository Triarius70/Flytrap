using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour {
    public GameObject FloatingText;
	// Use this for initialization
	void Start () {
        if (FloatingText != null)
            iTween.ScaleTo(FloatingText, iTween.Hash("name","IntroTween","scale", new Vector3(0.925f, 0.925f, 1f), "time", 1f, "easetype", iTween.EaseType.easeOutSine, "looptype", iTween.LoopType.pingPong));
	}
	
	// Update is called once per frame
	void Update () {
	  if(Input.anyKey)
        {
            iTween.StopByName("IntroTween");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
	}
}
