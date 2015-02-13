using UnityEngine;
using System.Collections;

public class PolyReadTest : MonoBehaviour {

	PolyData polyData = null;
	// Use this for initialization
	void Awake () {
		PolyMapLoader map = new PolyMapLoader ("x", "y", "goalPos", "startPos", "button");	
		polyData = map.polyData;
		/*
		 * Debugging
		map.polyData.printNodes ();
		map.polyData.printStart ();
		map.polyData.printEnd ();
		map.polyData.printButtons ();
		*/
	}
	
	void OnDrawGizmos() {
		if (polyData != null) {
			for(int i = 0; i <= 22; i++) {
				Gizmos.color = Color.white;
				Gizmos.DrawCube (polyData.nodes[i], Vector3.one);
			}

			Gizmos.color = Color.green;
			Gizmos.DrawCube (polyData.start, Vector3.one);
			
			Gizmos.color = Color.red;
			Gizmos.DrawCube (polyData.end, Vector3.one);
		}
	}	
}
	