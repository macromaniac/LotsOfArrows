using UnityEngine;
using System.Collections;
using System;

public class PathEater : MonoBehaviour {

	public GameObject pathEaterSquare;
	public GameObject pathEaterPointer;
	public Path path;
	public bool hasStarted = false;
	private Vector3 basePosition;
	private float pathEaterWidth = 0f;
	private float speed = .05f;
	void Start () {
		basePosition = this.transform.position;
		pathEaterWidth = pathEaterSquare.GetComponent<SpriteRenderer>().bounds.size.x;
	}

	private void updateMyPosition(float xPos) {
		Vector3 camPosXY = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
		this.transform.position = camPosXY + basePosition;
		this.transform.position = new Vector3(xPos, this.transform.position.y, this.transform.position.z);
	}
	private void updateMyPosition() {
		updateMyPosition(this.transform.position.x);
	}
	public void tick () {

		updateMyPosition();
		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];
			processInput(touch.position.x, touch.position.y);
		}
		else if (Input.GetMouseButton(0)) {
			processInput(Input.mousePosition.x, Input.mousePosition.y);
		}
		Camera.main.transform.position = Camera.main.transform.position + new Vector3(0f, speed, 0f);
		updateMyPosition();
		path.refreshPathForCamera();
		speed -= .001f;
		if (speed <= 0.1f)
			speed = .1f;
	}

	public float getActualWidth() {
		return pathEaterWidth;
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
		updateMyPosition(inputWorldPointX);

		int frameLimit = 0;
		int counter = 0;
		Segment segment = path.closestSegmentToPoint(this.transform.position);
		float closestSegmentX= float.MaxValue;
		if (segment != null) {
			closestSegmentX = segment.transform.position.x;
		}
		else {
			return;
		}

		if (closestSegmentX > getLeftWall() && closestSegmentX < getRightWall()) {
			if (counter > frameLimit)
				return;
			counter++;
			if (path.isOutOfSegments()) {
				Debug.Log(ticker.ToString());
				return;
			}
			ticker++;
			float distanceFromCenter = Math.Abs(this.transform.position.x - inputWorldPointX);
			float percentCloseToMiddle = 1f- distanceFromCenter / (getActualWidth() / 2f);
			speed += segment.actualSpriteHeight/99f; //BINARY FOR NOW
			//(For non-binary : ) * percentCloseToMiddle ;
		}
	}
}
