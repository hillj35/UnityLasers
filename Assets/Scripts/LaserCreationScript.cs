using UnityEngine;
using System.Collections;

public class LaserCreationScript : MonoBehaviour {

    public GameObject laser;

	// Use this for initialization
	void Start () {
        //sets laser position to the position of the emiter
        laser.transform.position = this.transform.position;
        laser.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
