using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DynamicCarModel : MonoBehaviour, CarModel {
	
	List<PolyNode> path;
	Line collisionLine;
	public float maxWheelAngle = 87f;
	public float carLength = 2;
	public float vel = 10;
	public int collisionSize = 5;
	public float dynF = 1;
	public float dynMass = 1;
	float dynVel = 0;
	
	public List<Obstacle> obstacles;
	
	public DynamicCarModel() {
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
		//	Quaternion test = Quaternion.LookRotation (path[0].pos - GameObject.Find ("Player").car.transform.position);
		//	GameObject.Find ("Player").car.transform.rotation = Quaternion.RotateTowards (GameObject.Find ("Player").car.transform.rotation, test, 9000);
		
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
			
			float distanceToTarget=Vector3.Distance (current, car.transform.position);
			
			
			float neededDistToStop=(Mathf.Pow(dynVel,2)/2*(dynF/dynMass));
			if(distanceToTarget>neededDistToStop){
				dynVel=dynVel+(dynF/dynMass);
			}
			else{
				dynVel=dynVel-(dynF/dynMass);
			}
			
			
			float wheelAngleRad=maxWheelAngle*(Mathf.PI/180);
			float dTheta=(dynVel/carLength)*Mathf.Tan(wheelAngleRad);
			Quaternion theta = Quaternion.LookRotation (current - car.transform.position);
			/*if(Vector3.Distance (current, car.transform.position) < dynVel*Time.deltaTime ) {
					dir = current - car.transform.position;
					Debug.Log("Jump");
					car.transform.position = current;
				}*/
			//else {
			
			if(car.transform.rotation!=theta){
				car.transform.rotation = Quaternion.RotateTowards (car.transform.rotation, theta, dTheta * Time.deltaTime);
			}
			
			
			Vector3 curDir=car.transform.eulerAngles;
			//Debug.Log("Euler Angles:"+car.transform.eulerAngles);
			Vector3 newPos=car.transform.position;
			float angleRad=curDir.y*(Mathf.PI/180);
			newPos.x=newPos.x+(dynVel*Mathf.Sin(angleRad)*Time.deltaTime);
			newPos.z=newPos.z+(dynVel*Mathf.Cos(angleRad)*Time.deltaTime);
			car.transform.position=newPos;
			//car.transform.position = (car.transform.position + newPos);
			
			//If the car is "almost" at the point
			/*
			if(Vector3.Distance (current, car.transform.position) < 5*dynVel*Time.deltaTime && 
			   (newPos.x==current.x || newPos.z==current.z)){
				Debug.Log("Car Almost at point");
				carMadeIt=true;
			}
			*/

			if(Vector3.Distance (current, car.transform.position) < 2) 
				carMadeIt = true;
			
			//}
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
