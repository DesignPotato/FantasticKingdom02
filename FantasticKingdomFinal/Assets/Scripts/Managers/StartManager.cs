using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartManager : MonoBehaviour {

	public GameObject quitCheck;
	public Texture2D cursorTexture;

    public GameObject tutorial;

	public void Start(){
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        if (tutorial)
            tutorial.SetActive(false);
	}

	public void StartGame()
	{
		SceneManager.LoadScene (1, LoadSceneMode.Single);
	}

    public void LoadTutorial()
    {
        Debug.Log("Fuvk");
        Debug.Log(tutorial);
        if (tutorial)
        {
            Debug.Log("Fuyyyvk");
            tutorial.SetActive(true);
        }
    }

	public void checkQuit(bool enabled){
		quitCheck.SetActive (enabled);
	}

	public void Quit()
	{
		#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
