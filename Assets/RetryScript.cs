using UnityEngine;
using System.Collections;

public class RetryScript : MonoBehaviour {

	public bool isButtonShown = false;
	void OnGUI() {
		if (isButtonShown == true)
			if (GUI.Button(new Rect(100, 100, 50, 50), "Retry")) {
				Application.LoadLevel("GameScene");
			}
	}
}
