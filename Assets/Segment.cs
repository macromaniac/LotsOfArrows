using UnityEngine;
using System.Collections;

public class Segment : MonoBehaviour {

	public enum TickStatus { Successful, Failure}
	public int segmentNumber;
	private Segment lastSegment;
	public float spriteHeight=0f;
	void Start () {
		//This is only so spriteHeight has value for the uninitialized first piece
		if(spriteHeight==0f)
			spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;
	}

	public TickStatus tick() {
		Vector3 objPointOnScreen = Camera.main.WorldToScreenPoint(transform.position);
		if (objPointOnScreen.y<-20)
			return TickStatus.Failure;
		return TickStatus.Successful;
	}

	public Vector3 segmentEndPoint {
		get {

			//This is the point where the next Segment will start before being translated and rotated
			Vector3 displacementVector =  new Vector3(0,spriteHeight *1.65f,0f);


			float xNew = displacementVector.x * Mathf.Cos(rotationInRadians)
				- displacementVector.y * Mathf.Sin(rotationInRadians);
			float yNew = displacementVector.x * Mathf.Sin(rotationInRadians)
				+ displacementVector.y * Mathf.Cos(rotationInRadians);

			Vector3 rotatedDisplacementVector = new Vector3(xNew, yNew, 0f);

			Vector3 toReturn = this.gameObject.transform.position + rotatedDisplacementVector;
			return toReturn;
		}
	}
	public float actualSpriteHeight {
		get {

			//This is the point where the next Segment will start before being translated and rotated
			Vector3 displacementVector =  new Vector3(0,spriteHeight,0f);


			float xNew = displacementVector.x * Mathf.Cos(rotationInRadians)
				- displacementVector.y * Mathf.Sin(rotationInRadians);
			float yNew = displacementVector.x * Mathf.Sin(rotationInRadians)
				+ displacementVector.y * Mathf.Cos(rotationInRadians);

			Vector3 rotatedDisplacementVector = new Vector3(xNew, yNew, 0f);

			return yNew;
		}
	}

	public float rotationInRadians {
	get {
			return this.gameObject.transform.rotation.eulerAngles.z * 0.0174532925f;
		}
	}
	static float number = 0;
	public void initFromLastSegment(Segment lastSegment) {
		this.lastSegment = lastSegment;
		this.segmentNumber = lastSegment.segmentNumber+1;
		this.gameObject.transform.position = lastSegment.segmentEndPoint;
		spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;
		number+=1f;
		float curAngle=20f*Mathf.Cos(number/20f);
		transform.Rotate(new Vector3(0, 0, curAngle));
	}
	public static Segment genSegment(Segment lastSegment) {
		GameObject gameObject = (GameObject)GameObject.Instantiate(Resources.Load("Segment"));
		Segment segment = gameObject.GetComponent<Segment>();
		segment.initFromLastSegment(lastSegment);
		return segment;
	}
}
