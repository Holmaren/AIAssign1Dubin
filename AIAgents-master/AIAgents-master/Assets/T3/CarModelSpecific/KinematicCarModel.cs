using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class KinematicCarModel : MonoBehaviour, CarModel {
	
	List<PolyNode> path;
	Line collisionLine;
	public float maxWheelAngle = 87f;
	public float carLength = 2;
	public float vel = 10;
	public int collisionSize = 5;

	public List<Obstacle> obstacles;
	
	public KinematicCarModel() {
	}

	public void SetObstacles(List<Obstacle> obstacles) {
		this.obstacles = obstacles;
	}
	
	public void SetPath(List<PolyNode> path) {
		this.path = path;
	}
	
	public void StartCoroutineMove() {
		StartCoroutine ("Move");
	}

	float DegToRad(float degree){
		return (Mathf.PI * degree) / 180;
	}

	Vector3 current = new Vector3(0,0,0);
	Vector3 box = new Vector3(0,0,0);
	bool firstStop = true;
	public IEnumerator Move() {
		// Cheat align the vehicle in accordance to the path at spawn time
		//	Quaternion test = Quaternion.LookRotation (path[0].pos - GameObject.Find ("Player").transform.position);
		//	GameObject.Find ("Player").transform.rotation = Quaternion.RotateTowards (GameObject.Find ("Player").transform.rotation, test, 9000);
		
		GameObject frontWheel = GameObject.Find ("FrontWheel");
		GameObject car = GameObject.Find ("Player");
		
		int index = 0;
		current = path[index].pos;
		
		bool carMadeIt = false;

		while (true) {	
			if(carMadeIt == true) {
				index++;
				if(index >= path.Count){
					index = 0;
					yield break;
				}
				current = path[index].pos;
				carMadeIt = false;
				
				if(firstStop) {
					// Continue a little bit
					index--;
					current = path[index].pos + car.transform.forward*3;
					firstStop = false;
				} else {
					firstStop = true;
				}
			}
			
			Vector3 dir = current - GameObject.Find ("Player").transform.position;


			collisionLine = new Line(GameObject.Find ("Player").transform.position, 
			                         GameObject.Find ("Player").transform.position + Vector3.Normalize(dir)*collisionSize);

			if(IntersectsWithAnyLine(collisionLine)) {
				print ("Collision");

				Vector3 newDir = NewDirection (collisionLine);

				if(newDir == Vector3.zero) {
					print ("I am stuck");
					vel = -20;
					yield break;

				}
				else {
					print ("Resolved");
					current += newDir;
				}
			}

			float wheelAngleRad=maxWheelAngle*(Mathf.PI/180);
			float dTheta=(vel/carLength)*Mathf.Tan(wheelAngleRad);
			Quaternion theta = Quaternion.LookRotation (current - GameObject.Find ("Player").transform.position);
			
			if(GameObject.Find ("Player").transform.rotation != theta){
				GameObject.Find ("Player").transform.rotation = Quaternion.RotateTowards (GameObject.Find ("Player").transform.rotation, theta, dTheta * Time.deltaTime);
			}
			
			Vector3 curDir=GameObject.Find ("Player").transform.eulerAngles;
			
			Vector3 newPos=GameObject.Find ("Player").transform.position;
			float angleRad=curDir.y*(Mathf.PI/180);
			newPos.x=newPos.x+(vel*Mathf.Sin(angleRad)*Time.deltaTime);
			newPos.z=newPos.z+(vel*Mathf.Cos(angleRad)*Time.deltaTime);
			GameObject.Find ("Player").transform.position=newPos;
			
			if(Vector3.Distance (current, GameObject.Find ("Player").transform.position) < 2){
				carMadeIt = true;
			}
			yield return null;
		}
	}
	
	public Vector3 NewDirection (Line collisionLine){
		Line tmp = collisionLine;
		int speed = 0;
		speed++;

		tmp = collisionLine;
		tmp.point2 += Vector3.forward * speed;
		if (!IntersectsWithAnyLine (tmp)) {
			print ("forward");
			return Vector3.forward;
		}
		
		tmp = collisionLine;
		tmp.point2 += Vector3.back * speed;
		if (!IntersectsWithAnyLine (tmp)) {
			print ("back");
			return Vector3.back;
		}
		
		tmp = collisionLine;
		tmp.point2 += Vector3.left * speed;
		if (!IntersectsWithAnyLine (tmp)) {
			print ("left");
			return Vector3.left;
		}
		
		tmp = collisionLine;
		tmp.point2 += Vector3.right * speed;
        if (!IntersectsWithAnyLine (tmp)) {
            print ("right");
            return Vector3.right;
        }
        
        return Vector3.zero;
    }
    
    public bool IntersectsWithAnyLine(Line myLine) {
		foreach (Obstacle obs in obstacles) {
			foreach (Line line in obs.edges) {
			//	if (myLine.point1 == line.point1 || myLine.point1 == line.point2)
			//		continue;
			//	if (myLine.point2 == line.point1 || myLine.point2 == line.point2)
			//		continue;
				
				if (myLine.intersect (line)) {
					//print (myLine.point1 + ", " + myLine.point2 + " vs " + line.point1 + ", " + line.point2);
					return true;
				}
			}
		}
		return false;
	}

	public void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine (collisionLine.point1, collisionLine.point2);

		Gizmos.color = Color.magenta;
		Gizmos.DrawCube (current, Vector3.one);
	}
}
