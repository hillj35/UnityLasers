using UnityEngine;
using System.Collections;

//Used by an Emitter to draw laser paths.

public class LaserPathFinding : MonoBehaviour {
    public GameObject Emitter;
    public Vector3 emitAngle;
    public GameObject laserPrefab;
    public GameObject defaultLaser;
    public Color laserColor;

    private Vector3 currentPosition;
    private Vector3 otherLaserHitLoc;
    private bool emitting = true;
    private float defaultLength;
    private bool hittingChangeableObj = false;
    private GameObject objectHit;
    private bool updateLaser;
    private bool hitByLaser;
    private ArrayList laserList;

    // Use this for initialization
    void Start () {
        emitAngle = Emitter.transform.TransformDirection(Vector3.forward);
        defaultLaser.transform.forward = emitAngle;
        updateLaser = true;
        defaultLaser.GetComponent<SpriteRenderer>().color = laserColor;
        defaultLaser.transform.GetChild(0).GetComponent<SpriteRenderer>().color = laserColor;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void LaserPathFind(GameObject[] laserArray)
    {
        //finds the paths the lasers should take before collisions with other lasers are factored into account
        //returns an array of all lasers (lasers remain deactivated until later)
        defaultLaser.SetActive(false);

        emitAngle = Emitter.transform.TransformDirection(Vector3.forward);
        MakeLaserPath(Emitter.transform.position, emitAngle, defaultLaser);

        laserArray = (GameObject[])laserList.ToArray();
    }

    private void MakeLaserPath(Vector3 origin, Vector3 direction, GameObject laser)
    {
        RaycastHit hitInfo;

        //Sets laser direction
        laser.transform.right = direction;

        //Find where the laser will hit
        if (Physics.Raycast(origin, direction, out hitInfo, 100.0f))
        {
            //found a hit
            laserList.Add(ResizeLaser(hitInfo.distance, laser, origin));

            if (hitInfo.collider.tag == "Reflective")
            {
                //we must create a new laser reflected from this laser
                ReflectLaser(hitInfo, laser, direction);
            }
        }
    }

    private GameObject ResizeLaser(float newLength, GameObject laser, Vector3 origin)
    {
        defaultLength = laser.GetComponent<Renderer>().bounds.size.x;
        Vector3 newScale = new Vector3(newLength, laser.transform.localScale.y, laser.transform.localScale.z);
        laser.transform.localScale = newScale;
        laser.transform.position = origin;
        return laser;
    }

    void ReflectLaser(RaycastHit hitInfo, GameObject previousLaser, Vector3 sourceDirection)
    {
        //set up, get normal of hit object, then create a direction vector for reflected laser
        Vector3 hitObjectNormal = hitInfo.normal;
        Vector3 newLaserDirection = Vector3.Reflect(sourceDirection, hitObjectNormal);

        //get point of hit on reflective object (world coordinates)
        Vector3 worldHitLoc = hitInfo.point;

        //reflect laser must create a new laser, as a child of the emitter
        GameObject newLaser = Instantiate(defaultLaser);
        newLaser.transform.right = newLaserDirection;
        newLaser.SetActive(false);
        newLaser.transform.localScale = new Vector3(.25f, 1, 1);
        newLaser.tag = "ReflectedLaser";

        //new laser must also call makeLaserPath
        MakeLaserPath(worldHitLoc, newLaserDirection, newLaser);
    }
}
