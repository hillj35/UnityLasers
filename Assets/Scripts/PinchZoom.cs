using UnityEngine;
using System.Collections;

public class PinchZoom : MonoBehaviour {
    public float perspectiveZoomSpeed = 0.5f;
    public float orthoZoomSpeed = 0.5f;


	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

            Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, 0.1f);
        }
	}
}
