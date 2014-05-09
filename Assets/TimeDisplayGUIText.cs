using UnityEngine;
using System.Collections;

public class TimeDisplayGUIText : MonoBehaviour {

	public Game game;
	void Start () {
	}
	void Update() {
		guiText.text = game.gameTime.ToString("0.0");
	}
}
