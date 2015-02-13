using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public interface CarModel {
	IEnumerator Move ();
	void StartCoroutineMove();
	void SetPath(List<PolyNode> path);
	void SetObstacles(List<Obstacle> obstacles);
}