using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolyData {
	// *Specific case* Nodes
	// 0 -> 3 : top left
	// 4 -> 9 : middle
	// 10 -> 13 : middle right
	// 14 -> 17 : down right
	// 18 -> 22 : down left
	public List<Vector3> nodes;
	public Vector3 start;
	public Vector3 end;
	public List<int> buttons;

	public PolyData() {
		nodes = new List<Vector3> ();
		buttons = new List<int> ();
	}

	/*
	 * For debugging purposes: Only works if class inherits MonoBehavior
	public void printNodes() {
		print ("#Nodes: " + nodes.Count);
		foreach (Vector3 node in nodes) {
			print (node);
		}
	}

	public void printStart() {
		print ("Start:  " + start);
	}

	public void printEnd() {
		print ("End: " + end);
	}

	public void printButtons() {
		print ("#Buttons: " + buttons.Count);
		foreach (int i in buttons) {
			print (i);
		}
	}
	*/
}
