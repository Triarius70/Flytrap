using UnityEngine;
using System.Collections;

public class ClickToDestroy : MonoBehaviour {

	protected bool isDead = false;

	void OnMouseUp()
	{
		if (isDead)
			return;

		isDead = true;

		Bug bug = transform.parent.gameObject.GetComponent<Bug> ();
		//bug.speed *= 0.5f;
		bug.speed = 0f;

		//transform.Rotate (Vector3.forward * Random.Range(0, 360));
		
		Animator animator = GetComponent<Animator> ();
		animator.SetBool ("isDead", true);
		Destroy (gameObject, 1);
	}
}
