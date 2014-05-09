using UnityEngine;
using System.Collections;
using System;

public class PathEater : MonoBehaviour {

	public GameObject pathEaterSquare;
	public GameObject pathEaterPointer;
	public Path path;
	public bool hasStarted = false;
	private Vector3 basePosition;
	private float xPos = 0f;
	private float pathEaterWidth = 0f;
	void Start () {
		basePosition = this.transform.position;
		pathEaterWidth = pathEaterSquare.GetComponent<SpriteRenderer>().bounds.size.x;
	}

	private void updateMyPosition() {
		Vector3 camPosXY = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
		this.transform.position = camPosXY + basePosition;
		this.transform.position = new Vector3(xPos, this.transform.position.y, this.transform.position.z);
	}
	void Update () {
		updateMyPosition();

		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];
			processInput(touch.position.x, touch.position.y);
		}
		else if (Input.GetMouseButton(0)) {
			processInput(Input.mousePosition.x, Input.mousePosition.y);
		}
		alignWithClosestSegment();
	}

	public float getActualWidth() {
		return pathEaterWidth * 3f;
	}
	public float getLeftWall (){
		return this.transform.position.x - getActualWidth()/2f;
	}
	public float getRightWall(){
		return this.transform.position.x + getActualWidth()/2f;
	}
	static int ticker = 0;
	private void processInput(float x, float y) {
		hasStarted = true;
		Vector3 inputAsWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0f));
		float inputWorldPointX = inputAsWorldPoint.x;

		int frameLimit = 0;
		int counter = 0;
		while (inputWorldPointX > getLeftWall() && inputWorldPointX < getRightWall()) {
			if (counter > frameLimit)
				return;
			counter++;
			if (path.isOutOfSegments()) {
				Debug.Log(ticker.ToString());
				return;
			}
			ticker++;
			Segment segment = alignWithClosestSegment();
			float distanceFromCenter = Math.Abs(this.transform.position.x - inputWorldPointX);
			float percentCloseToMiddle = 1f- distanceFromCenter / (getActualWidth() / 2f);
			float speed = segment.actualSpriteHeight * 1.3f * percentCloseToMiddle ;
			Camera.main.transform.position = Camera.main.transform.position + new Vector3(0f, speed, 0f);
			updateMyPosition();
			path.refreshPathForCamera();
		}
	}

	private Segment alignWithClosestSegment() {
		Segment segment = path.closestSegmentToPoint(this.transform.position);
		if(segment!=null)
			this.xPos = segment.transform.position.x;
		return segment;
	}
}
