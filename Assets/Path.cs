using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path : MonoBehaviour {
	//The starting Segment is the first segment of the path, it eventually becomes unloaded
	//It is mostly for path positioning at the start and as a visual indicator.
	// use firstSegment to get the first segment that is loadable
	public Segment startingSegment;
	private List<Segment> segments;

	private const int segmentsLoadedInBuffer=100;
	private const int segmentsTotal = 1000;
	void Start () {
		segments = new List<Segment>();
		segments.Add(startingSegment);
	}
	
	void Update () {
		refreshPathForCamera();
	}
	public void refreshPathForCamera() {
		for(int i=0;i<segments.Count;++i){
			Segment segment = segments[i];
			if (segment.tick() == Segment.TickStatus.Failure) {
				segments.RemoveAt(i);
				Destroy(segment.gameObject);
			}
		}
		loadSegments();

	}

	public bool isOutOfSegments() {  return segments.Count == 0; }
	public Segment lastSegment { get { return segments[segments.Count - 1]; } }
	private Segment firstSegment { get { return segments[0]; } }
	private void loadSegments() {
		while (segments.Count < segmentsLoadedInBuffer
			&& segments.Count>0 && lastSegment.segmentNumber < segmentsTotal) {
					Segment oldLastSegment = lastSegment;
					segments.Add(Segment.genSegment(oldLastSegment));
		}
	}
	public Segment closestSegmentToPoint(Vector3 point) {
		Segment toReturn =null;
		float minDistance = float.MaxValue;
		foreach (Segment s in segments) {

			float magnitude = Mathf.Abs(s.transform.position.y - point.y);
			if (magnitude < minDistance) {
				toReturn = s;
				minDistance = magnitude;
			}
		}
		return toReturn;
	}
}
