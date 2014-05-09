using System;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
	public PathEater pathEater;
	public Path path;
	public RetryScript retry;
	private float gameTimeStart=0f;
	void Start() {
	}

	private float finalGameTime = 0f;
	void Update() {
		if (pathEater.hasStarted == true && gameTimeStart == 0f)
			gameTimeStart = Time.time;
		if (path.isOutOfSegments()) {
			finalGameTime = gameTime;
			retry.isButtonShown = true;
		}
	}
	public float gameTime {
		get {
			if (gameTimeStart == 0f)
				return 0f;
			if (finalGameTime != 0f)
				return finalGameTime;
			return Time.time - gameTimeStart;
		}
	}
}
