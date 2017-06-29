using UnityEngine;
using System.Collections;

public class CameraMovementHandler : MonoBehaviour {

    // Use this for initialization
    public Vector3 TopViewLoc;
    public Vector3 TopViewRot;
    public Vector3 MainViewLoc;
    public Vector3 MainViewRot;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.T)) //top view
        {
            this.transform.position = TopViewLoc;
            this.transform.rotation = Quaternion.Euler(TopViewRot);
        }
        else if (Input.GetKeyDown(KeyCode.M)) //main view
        {
            this.transform.localPosition = MainViewLoc;
            this.transform.rotation = Quaternion.Euler(MainViewRot);
        }
	}
}
