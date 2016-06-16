// Code By Stefan Peacock, 300309611 , VUW MDDN 343 , Fantastic Kindom
// Mostly sourced from unity3d Documentation on LookRotation

using UnityEngine;
using System.Collections;


public class onHitOrientation : MonoBehaviour {
	public Transform target;
	void Start(){
		target = GameObject.FindWithTag ("MainCamera").transform;
	}
	void Update () {
		Vector3 relativePos = target.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		transform.rotation = rotation;
	}
}
