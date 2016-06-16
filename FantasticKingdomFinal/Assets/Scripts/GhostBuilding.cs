using UnityEngine;
using System.Collections;

public class GhostBuilding : MonoBehaviour {
	
	private bool isTriggered = false;

	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals ("Ground"))
			return;
		isTriggered = true;
	}

	void OnTriggerStay(Collider other) {
		if (other.tag.Equals ("Ground"))
			return;
		isTriggered = true;
	}

	void OnTriggerExit(Collider other) {
		if (other.tag.Equals ("Ground"))
			return;
		isTriggered = false;
	}

	public bool IsTriggered(){
		return isTriggered; 
	}
}
