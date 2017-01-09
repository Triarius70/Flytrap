using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CompanyLogoScreen : MonoBehaviour {
    public GameObject logo;
	// Use this for initialization
	void Start () {
        Debug.Log("FadeIn");
        CrossFadeAlpha(logo, 0f, 0f);
        Invoke("Show", 0.2f);
	}

    protected void Show()
    {
        logo.SetActive(true);
        CrossFadeAlpha(logo, 1f, 2f);
        Invoke("Hide", 4f);
    }

    protected void Hide()
    {
        CrossFadeAlpha(logo, 0.0f, 1.5f);
        Invoke("HideComplete", 1.75f);
    }

    protected void HideComplete()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    protected void CrossFadeAlpha(GameObject gameObject, float target, float animationTime)
    {
        foreach (Graphic graphic in gameObject.GetComponentsInChildren<Graphic>())
        {
            graphic.CrossFadeAlpha(target, animationTime, false);
        }
    }
}
