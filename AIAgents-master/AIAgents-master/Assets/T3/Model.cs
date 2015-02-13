using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public interface Model {
	IEnumerator Move ();
	void StartCoroutineMove();
	void SetPath(List<PolyNode> path);
}