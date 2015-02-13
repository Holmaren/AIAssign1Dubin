using UnityEngine;
using System.Collections;

public class TestLineCollision : MonoBehaviour {

	Line[] lines = null;
	// Use this for initialization
	void Start () {
		lines = new Line[2];
		Line a = new Line (new Vector3 (-1, 1, 0), new Vector3 (1, 1, 0));
		Line b = new Line (new Vector3 (0, 1, -1), new Vector3 (0, 1, 1));
		lines [0] = a;
		lines [1] = b;

		bool ab = a.intersect (b);
		bool ba = b.intersect (a);
		print ("ab: " + ab);
		print ("ba: " + ba);
	}


	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		if (lines != null) {
			foreach(Line line in lines) {
				Gizmos.DrawLine (line.point1, line.point2);
			}
		}
	}
}
