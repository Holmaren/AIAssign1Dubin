using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KinematicPointModel : MonoBehaviour, Model {

	List<PolyNode> path;
	public int vel;

	public KinematicPointModel() {
		vel = 20;
    }

	public void SetPath(List<PolyNode> path) {
		this.path = path;
	}

	public void StartCoroutineMove() {
		StartCoroutine ("Move");
	}

	public IEnumerator Move() {
		int index = 0;
		Vector3 current = path[index].pos;
		while (true) {
			if(GameObject.Find ("Player").transform.position == current) {
				index++;
				if(index >= path.Count)
					yield break;
				current = path[index].pos;
			}
			GameObject.Find ("Player").transform.position = Vector3.MoveTowards (GameObject.Find ("Player").transform.position, current, vel * Time.deltaTime);
			yield return null;
		}
	}
}
