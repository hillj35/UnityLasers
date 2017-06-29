using UnityEngine;
using System.Collections;

public class FollowPath : MonoBehaviour {
    public GameObject[] waypoints;
    public bool moving;
    public float speed;
    public bool toggle;
    public float pause; //time in seconds between switching directions in path

    private int waypointCount;
    private int currentWaypoint = 1;
    private bool forward = true;
    private bool paused = false;
    private float pauseTimer = 0.0f;

	// Use this for initialization
	void Start () {
        waypointCount = waypoints.Length;
	}
	
	// Update is called once per frame
	void Update () {
	    if (moving && !paused)
        {
            GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, speed));

            if (transform.position == waypoints[currentWaypoint].transform.position)
                changeWaypoint();   
        }
        
        else if (paused && moving)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer > pause)
            {
                pauseTimer = 0;
                paused = false;
            }
        }

  	}

    private void changeWaypoint()
    {
        if (forward)
        {
            if (currentWaypoint < waypoints.Length - 1)
                currentWaypoint++;
            else
            {
                currentWaypoint--;
                forward = false;
                paused = true;
            }
        }
        else
        {
            if (currentWaypoint > 0)
            {
                currentWaypoint--;
            }
            else
            {
                currentWaypoint++;
                forward = true;
                paused = true;
            }
        }
    }

    public void SetMoving(bool val)
    {
        moving = val;
    }
}
