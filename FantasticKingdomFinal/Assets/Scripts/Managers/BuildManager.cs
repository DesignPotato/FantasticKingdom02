using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildManager : MonoBehaviour {

	public Text icon1Title;
	public Text icon2Title;
	public Text icon3Title;
	public Text icon4Title;
	public Text icon1Cost;
	public Text icon2Cost;
	public Text icon3Cost;
	public Text icon4Cost;

	public Color normalColor;
	public Color normalColor2;

	public Camera cam;

	public int wallCost;
	public int gateCost;
	public int towerCost;
	public int barracksCost;

	public GameObject wallGhost;
	public GameObject gateGhost;
	public GameObject towerGhost;
	public GameObject barracksGhost;
	public GameObject wallGhostPrefab;
	public GameObject gateGhostPrefab;
	public GameObject towerGhostPrefab;
	public GameObject barracksGhostPrefab;

	public GoldPile goldPile;

	public float rotateSpeed = 3;

	private GameObject currentObject;
	public Animator ani;
	private bool building = false;
	private Canvas canvas;
	// Create mat
	private Material correctBuildMat;
	private Material failBuildMat;

	// Use this for initialization
	void Start () {
		icon1Cost.text = wallCost + "G";
		icon2Cost.text = gateCost + "G";
		icon3Cost.text = towerCost + "G";
		icon4Cost.text = barracksCost + "G";
		icon1Title.color = normalColor2;
		// Init mats
		correctBuildMat = new Material(Shader.Find("Legacy Shaders/Diffuse"));
		correctBuildMat.color = Color.green;
		failBuildMat = new Material(Shader.Find("Legacy Shaders/Diffuse"));
		failBuildMat.color = Color.red;

		// Apply mats to all objects and disable physics
		wallGhost.SetActive(false);
		BoxCollider[] bcs = wallGhost.GetComponentsInChildren<BoxCollider>();
		foreach (BoxCollider bc in bcs) {
			bc.isTrigger = true;
		}

		gateGhost.SetActive(false);
		bcs = gateGhost.GetComponentsInChildren<BoxCollider>();
		foreach (BoxCollider bc in bcs) {
			bc.isTrigger = true;
		}

		towerGhost.SetActive(false);
		bcs = towerGhost.GetComponentsInChildren<BoxCollider>();
		foreach (BoxCollider bc in bcs) {
			bc.isTrigger = true;
		}

		barracksGhost.SetActive(false);
		bcs = barracksGhost.GetComponentsInChildren<BoxCollider>();
		foreach (BoxCollider bc in bcs) {
			bc.isTrigger = true;
		}


		currentObject = wallGhost;
		canvas = GetComponent<Canvas>();
		ani = GetComponent<Animator>();
		//canvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(PauseManager.Paused){
			//canvas.enabled = false;
			building = false;
			wallGhost.SetActive(false);
			gateGhost.SetActive(false);
			towerGhost.SetActive(false);
			barracksGhost.SetActive(false);
			return;
		}

		if (CheckBuilding ()) {
			Select ();
			ShowGhost ();
			Rotate ();
			Build ();
		}

	}

	bool CheckBuilding(){
		if (Input.GetKeyDown (KeyCode.B)) {
			if (!building) {
				building = true;
				//canvas.enabled = true;
				currentObject.SetActive(true);
				ani.SetBool ("GoOut", true);
			} else {
				building = false;
				//canvas.enabled = false;
				currentObject.SetActive(false);
				ani.SetBool ("GoOut", false);
			}
		}
		return building;
	}

	void Select(){
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			currentObject.SetActive(false);
			currentObject = wallGhost;
			currentObject.SetActive(true);
			icon1Title.color = normalColor2;
			icon2Title.color = normalColor;
			icon3Title.color = normalColor;
			icon4Title.color = normalColor;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			currentObject.SetActive(false);
			currentObject = gateGhost;
			currentObject.SetActive(true);
			icon1Title.color = normalColor;
			icon2Title.color = normalColor2;
			icon3Title.color = normalColor;
			icon4Title.color = normalColor;
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			currentObject.SetActive(false);
			currentObject = towerGhost;
			currentObject.SetActive(true);
			icon1Title.color = normalColor;
			icon2Title.color = normalColor;
			icon3Title.color = normalColor2;
			icon4Title.color = normalColor;
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			currentObject.SetActive(false);
			currentObject = barracksGhost;
			currentObject.SetActive(true);
			icon1Title.color = normalColor;
			icon2Title.color = normalColor;
			icon3Title.color = normalColor;
			icon4Title.color = normalColor2;
		}
	}

	void Rotate(){
		if (Input.GetKey(KeyCode.E)) {
			currentObject.transform.Rotate (0, -rotateSpeed, 0);
		}
		if(Input.GetKey(KeyCode.Q)) {
			currentObject.transform.Rotate (0, rotateSpeed, 0);
		}
	}

	void Build(){
		GhostBuilding gb = currentObject.GetComponent<GhostBuilding>();
		if (!gb.IsTriggered() && Input.GetMouseButtonDown(0)) {
			if (currentObject == wallGhost) {
				if (goldPile.getGold () > wallCost) {
					goldPile.deductGold (wallCost);
					GameObject wall = (GameObject)Instantiate (wallGhostPrefab, currentObject.transform.position, currentObject.transform.rotation);
				} else {
					// NOT ENOUGH MINERALS
					Debug.Log("NOT ENOUGH MINERALS: " + goldPile.getGold () + ", Need: " + (wallCost + 1));
				}
			} else if (currentObject == gateGhost) {
				if (goldPile.getGold () > gateCost) {
					goldPile.deductGold (gateCost);
					GameObject gate = (GameObject)Instantiate (gateGhostPrefab, currentObject.transform.position, currentObject.transform.rotation);
				} else {
					// NOT ENOUGH MINERALS
					Debug.Log("NOT ENOUGH MINERALS: " + goldPile.getGold () + ", Need: " + (gateCost + 1));
				}
			} else if (currentObject == towerGhost) {
				if (goldPile.getGold () > towerCost) {
					goldPile.deductGold (towerCost);
					GameObject tower = (GameObject)Instantiate (towerGhostPrefab, currentObject.transform.position, currentObject.transform.rotation);
				} else {
					// NOT ENOUGH MINERALS
					Debug.Log("NOT ENOUGH MINERALS: " + goldPile.getGold () + ", Need: " + (towerCost + 1));
				}
			} else if (currentObject == barracksGhost) {
				if (goldPile.getGold () > barracksCost) {
					goldPile.deductGold (barracksCost);
					GameObject barracks = (GameObject)Instantiate (barracksGhostPrefab, currentObject.transform.position, currentObject.transform.rotation);
				} else {
					// NOT ENOUGH MINERALS
					Debug.Log("NOT ENOUGH MINERALS: " + goldPile.getGold () + ", Need: " + (barracksCost + 1));
				}
			}
		}
	}

	void ShowGhost(){
		int layerMask = 1 << 9;
		layerMask = ~layerMask;

		// Set color
		GhostBuilding gb = currentObject.GetComponent<GhostBuilding>();
		if (gb.IsTriggered() || GetCost(currentObject) > goldPile.getGold ()) {
			wallGhost.GetComponent<Renderer>().material = failBuildMat;

			gateGhost.GetComponent<Renderer>().material = failBuildMat;

			Renderer[] rends = towerGhost.GetComponentsInChildren<Renderer>();
			foreach (Renderer rend in rends) {
				Material[] mats = rend.materials;
				for (int i = 0; i < mats.Length; i++) {
					mats [i] = failBuildMat;
				}
				rend.materials = mats;
			}

			rends = barracksGhost.GetComponentsInChildren<Renderer>();
			foreach (Renderer rend in rends) {
				Material[] mats = rend.materials;
				for (int i = 0; i < mats.Length; i++) {
					mats [i] = failBuildMat;
				}
				rend.materials = mats;
			}
		} else {
			wallGhost.GetComponent<Renderer>().material = correctBuildMat;

			gateGhost.GetComponent<Renderer>().material = correctBuildMat;

			Renderer[] rends = towerGhost.GetComponentsInChildren<Renderer>();
			foreach (Renderer rend in rends) {
				Material[] mats = rend.materials;
				for (int i = 0; i < mats.Length; i++) {
					mats [i] = correctBuildMat;
				}
				rend.materials = mats;
			}

			rends = barracksGhost.GetComponentsInChildren<Renderer>();
			foreach (Renderer rend in rends) {
				Material[] mats = rend.materials;
				for (int i = 0; i < mats.Length; i++) {
					mats [i] = correctBuildMat;
				}
				rend.materials = mats;
			}
		}

		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
			if (currentObject == barracksGhost) {
				currentObject.transform.position = new Vector3 (hit.point.x, hit.point.y - 0.5f, hit.point.z);
			} else {
				currentObject.transform.position = hit.point;
			}
			currentObject.transform.position = new Vector3(currentObject.transform.position.x, currentObject.transform.position.y + 0.5f, currentObject.transform.position.z);
		}
	}

	int GetCost(GameObject building){
		if (building.Equals (wallGhost)) {
			return wallCost;
		} else if (building.Equals (gateGhost)) {
			return gateCost;
		} else if (building.Equals (towerGhost)) {
			return towerCost;
		} else if (building.Equals (barracksGhost)) {
			return barracksCost;
		} else
			return 0;
	}
}
