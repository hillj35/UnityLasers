using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
    public float speed;


	// Use this for initialization
	void Start () {
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
            }
        }
	}
}
