using UnityEngine;
using System.Collections;

public class UnityEditorControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }	    
	}

    void Quit()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
