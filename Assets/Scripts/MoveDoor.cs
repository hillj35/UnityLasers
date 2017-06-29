using UnityEngine;
using System.Collections;

public class MoveDoor : MonoBehaviour {

    // Use this for initialization
    public bool open;
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void Move(bool val)
    {
        if (!val)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - this.transform.localScale.x, this.transform.position.z);
            open = false;
        }
        else
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + this.transform.localScale.x, this.transform.position.z);
            open = true;
        }
    }
}
