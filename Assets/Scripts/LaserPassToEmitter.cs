using UnityEngine;
using System.Collections;

public class LaserPassToEmitter : MonoBehaviour {
    public GameObject Emitter;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void LaserHit(Vector3 position, Color color)
    {
        Emitter.GetComponent<LaserActive>().LaserHit(position, color);
    }
}
