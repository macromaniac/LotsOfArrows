using UnityEngine;
using System.Collections;

public class CamTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//camera.transform.position = camera.transform.position + new Vector3(0, .05f);
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
}
