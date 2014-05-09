using UnityEngine;
using System.Collections;

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
	//TODO:Make this all a part of the pointer, no need to have it in patheater code
	private void updatePointerPosition() {
		Vector3 pos = pathEaterPointer.transform.position;
		pos.y = this.transform.position.y;
		pathEaterPointer.transform.position = pos;
	}
	private void updatePointerPosition(float x) {
		updatePointerPosition();
		Vector3 pos = pathEaterPointer.transform.position;
		float xPosWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(x,0f)).x;
		pathEaterPointer.transform.position=new Vector3(xPosWorldPoint,
			pos.y,pos.z);

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
		updatePointerPosition();
	}

	public float getLeftWall (){
		return this.transform.position.x - pathEaterWidth/2f;
	}
	public float getRightWall(){
		return this.transform.position.x + pathEaterWidth/2f;
	}
	static int ticker = 0;
	private void processInput(float x, float y) {
		updatePointerPosition(x);
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
			float speed = segment.actualSpriteHeight*1.5f;
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
