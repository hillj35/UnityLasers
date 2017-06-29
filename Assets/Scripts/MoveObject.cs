using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour {

    Vector3 screenSpace;
    Vector3 offset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMouseDown()
    {
        screenSpace = Camera.main.WorldToScreenPoint(transform.position);

        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
    }

    public void OnMouseDrag()
    {
        Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

        this.GetComponent<Rigidbody>().MovePosition(curPosition);
    }
}
