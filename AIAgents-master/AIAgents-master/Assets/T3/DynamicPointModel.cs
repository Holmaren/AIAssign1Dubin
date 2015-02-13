using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicPointModel : MonoBehaviour, Model {

	public float accMax;
	public float dynFx ;
	public float dynFy ;
	public float accX;
	public float accY;
	public float dynM ;
	List<PolyNode> path;

	public DynamicPointModel() {
	}
	
	public void SetPath(List<PolyNode> path) {
		this.path = path;
	}
	
	public void StartCoroutineMove() {
		StartCoroutine ("Move");
	}
	
	public IEnumerator Move() {
		int index = 0;
		float dynVel = 0;
		Vector3 dynPVel = new Vector3 (1,0,1);
		Vector3 current = path[index].pos;
		Vector3 dir;
		while (true) {
			// bug: Can't always move to the exact position of it's destination
			// result: spins around the target
			if(GameObject.Find ("Player").transform.position == current) {
				index++;
				if(index >= path.Count)
					yield break;
				current = path[index].pos;
				print (current);
				//dynVel=0;
			}
			dir=Vector3.Normalize(current-GameObject.Find ("Player").transform.position);
			float distToSlow=(Mathf.Pow(dynVel,2)-4.0f)/(2*accMax);
			float distanceToTarget=Vector3.Distance (current, GameObject.Find ("Player").transform.position);

			if(distanceToTarget>distToSlow){
				dynVel=dynVel+accMax;
			}
			else{
				dynVel=dynVel-accMax;
			}
			
			dynPVel.x=dynPVel.x*dynVel+dynVel*dir.x;
			dynPVel.z=dynPVel.z*dynVel+dynVel*dir.z;
			
			
			dynPVel=Vector3.Normalize(dynPVel);
			
			//Debug.Log ("DynVel:"+dynPVel);
			
			GameObject.Find ("Player").transform.position=GameObject.Find ("Player").transform.position+dynPVel;
			
			
			yield return null;

			/*
			if (Vector3.Distance (current, GameObject.Find ("Player").GameObject.Find ("Player").transform.position) < velX * Time.deltaTime) {
				dir = current - GameObject.Find ("Player").GameObject.Find ("Player").transform.position;
				GameObject.Find ("Player").GameObject.Find ("Player").transform.position = current;
				yield return new WaitForSeconds (0.5f);
			}
			else {
				velX = velX + accX;
				dir = Vector3.Normalize (current - GameObject.Find ("Player").GameObject.Find ("Player").transform.position);

				//Since the vector is normalized it should be the same velocity for both
				dir.x = (dir.x * velX) * Time.deltaTime;
				dir.z = (dir.z * velX) * Time.deltaTime;
				
				GameObject.Find ("Player").GameObject.Find ("Player").transform.position = (GameObject.Find ("Player").GameObject.Find ("Player").transform.position + dir);
			}
			yield return null;
*/
		}
	}
}


