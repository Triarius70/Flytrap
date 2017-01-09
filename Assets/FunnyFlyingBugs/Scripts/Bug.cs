using UnityEngine;
using System.Collections;

public class Bug : MonoBehaviour {

	[HideInInspector]
	public float speed = 1f;

    public float minSpeed = 1f;
    public float maxSpeed = 5f;

	protected Vector2 size;
	protected int direction = 1;
	protected GameObject beetle;

	void Start()
	{
		beetle = transform.Find ("beetle").gameObject;

        // SCALE
		float scale = Random.Range (0.5f, 0.7f);
		transform.localScale = new Vector2(scale, scale);

		SpriteRenderer renderer = beetle.GetComponent<SpriteRenderer> ();
		size = renderer.bounds.size;
		renderer.sortingOrder = (int)Mathf.Round(scale * 100f);

		speed = Random.Range (minSpeed, maxSpeed) * scale;
		direction = Random.Range (0, 2) == 1 ? 1 : -1;

		Vector3 pos;
		if (direction == 1) {
			int x = Screen.width + 1;
			int y = Random.Range (0, Screen.height);
			pos = Camera.main.ScreenToWorldPoint (new Vector2 (x, y));	
			pos.z = 0; 
			pos.x += size.x * 0.5f;
		} else {
			transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
			int x = -1;
			int y = Random.Range (0, Screen.height);
			pos = Camera.main.ScreenToWorldPoint (new Vector2 (x, y));	
			pos.z = 0; 
			pos.x -= size.x * 0.5f;			
		}
		transform.position = pos;
	}

	void Update()
	{
		Vector2 pos = transform.position;

		if (direction == 1) {
			pos.x -= Time.deltaTime * speed;
			transform.position = pos;

			Vector3 bound = Camera.main.ScreenToWorldPoint (new Vector2 (0, 0));	
			if (pos.x < (bound.x - size.x)) {
				Destroy (gameObject);
			}
		} else {
			pos.x += Time.deltaTime * speed;
			transform.position = pos;
			
			Vector3 bound = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, 0));	
			if (pos.x > (bound.x + size.x)) {
				Destroy (gameObject);
			}		
		}
	}


	void OnMouseUp()
	{
		speed *= 0.5f;

		//transform.rotation = Random.rotation;
		transform.Rotate (Vector3.forward * Random.Range(0, 360));

		Animator animator = GetComponent<Animator> ();
		animator.SetBool ("isDead", true);
		Destroy (gameObject, 1);
	}


}
