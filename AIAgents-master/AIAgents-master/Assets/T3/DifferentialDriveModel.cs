using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DifferentialDriveModel : MonoBehaviour, Model {
	
	List<PolyNode> path;
	public float vel;
	public float angleVel;

	public DifferentialDriveModel() {
		vel = 1f;
		angleVel = 57f;
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
			Vector3 dir;
			Quaternion theta = Quaternion.LookRotation (current - GameObject.Find ("Player").transform.position);
			if(Vector3.Distance (current, GameObject.Find ("Player").transform.position) < vel*Time.deltaTime && theta==GameObject.Find ("Player").transform.rotation) {
				dir = current - GameObject.Find ("Player").transform.position;
				//Debug.Log("Jump");
				GameObject.Find ("Player").transform.position = current;
				index++;
				if(index < path.Count)
					current = path[index].pos;
				else
					yield break;
			}
			else {
				
				if(GameObject.Find ("Player").transform.rotation!=theta){
					GameObject.Find ("Player").transform.rotation = Quaternion.RotateTowards (GameObject.Find ("Player").transform.rotation, theta, angleVel * Time.deltaTime);
				}
				else{
					dir = Vector3.Normalize (current - GameObject.Find ("Player").transform.position);					
					dir.x = dir.x * (  vel * Time.deltaTime);
					dir.z = dir.z * ( vel * Time.deltaTime);
					GameObject.Find ("Player").transform.position = (GameObject.Find ("Player").transform.position + dir);
				}
			}
			yield return null;		
		}
	}
}
