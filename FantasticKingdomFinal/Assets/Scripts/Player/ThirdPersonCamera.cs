using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

    public Canvas PauseMenu;
    PauseManager pauseManager;

    //Camera focal point
    public Transform target;
    public float distance = 4.0f;
    public float bufferup = 0.1f;
    public float bufferright = 0.75f;

    //Look sensitivity
    public float xSensitivity = 250.0f;
    public float ySensitivity = 120.0f;

    //Look bondaries
    public float yMinLimit = -25f;
    public float yMaxLimit = 70f;

    public float xRotation = 0.0f;

    //private float x = 0.0f;
    private float verticalAngle = 0.0f;

    Transform tempTarget;

    void Start () {
        //Set mouse state
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Set fields
        Vector3 angles = transform.eulerAngles;
        xRotation = angles.y;
        verticalAngle = angles.x;
        tempTarget = target;

        //Get pause menu
        if (PauseMenu != null)
            pauseManager = PauseMenu.GetComponent<PauseManager>();
    }
	

	void LateUpdate () {
        if (target)
        {
            //Mouse look controls
            xRotation += Input.GetAxis("Mouse X") * xSensitivity * 0.02f;
            verticalAngle -= Input.GetAxis("Mouse Y") * ySensitivity * 0.02f;

            //Controller look controls
			if(Input.GetAxis("JoyLookHori") > 0.4 || Input.GetAxis("JoyLookHori") < -0.4)
				xRotation += Input.GetAxis("JoyLookHori") * xSensitivity * 0.02f;
			if(Input.GetAxis("JoyLookVert") > 0.4 || Input.GetAxis("JoyLookVert") < -0.4)
				verticalAngle += Input.GetAxis("JoyLookVert") * ySensitivity * 0.02f;

            //Clamp camera angle to within boundaries
            verticalAngle = ClampAngle(verticalAngle, yMinLimit, yMaxLimit);

            //Update camera angle
            Quaternion rotation = Quaternion.Euler(verticalAngle, xRotation, 0);
            Vector3 position = rotation * new Vector3(bufferright, 0.0f, -distance) + target.position + new Vector3(0.0f, bufferup, 0.0f);

            //Set camera to updated rotation
            transform.rotation = rotation;
            transform.position = position;
        }

        if (pauseManager != null)
        {
            if (PauseManager.Paused)
            {
                //Open pause menu, and allow interaction
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                target = null;
            }
            else
            {
                //Close pause menu, and lock curser
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                target = tempTarget;
            }
                
        }
    }

    //Restricts angle between provided angles
    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
