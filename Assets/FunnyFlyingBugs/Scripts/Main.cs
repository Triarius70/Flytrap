using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public uint toSpawn = 8;
	public GameObject[] bugs;

	// Use this for initialization
	void Start () {
		StartCoroutine (SpawnBug());
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator SpawnBug()
	{
		if (bugs.Length < 1) {
			yield break;
		}

		while (true) {
			Instantiate (bugs [Random.Range (0, bugs.Length)], new Vector2 (0, 0), Quaternion.identity);
			yield return new WaitForSeconds (Random.Range (0f, 1f));
		}
	}
}
