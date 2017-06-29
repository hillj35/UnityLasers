using UnityEngine;
using System.Collections;

public class LaserActive : MonoBehaviour
{

    public GameObject Emitter;
    public Vector3 emitAngle;
    public GameObject laserPrefab;
    public GameObject defaultLaser;
    public Color laserColor;
    public bool deathLaser;
    public int laserLayer;
    public int startIntensity;

    private Vector3 currentPosition;
    private Vector3 otherLaserHitLoc;
    private bool emitting = true;
    private float defaultLength;
    private bool hittingChangeableObj = false;
    private GameObject objectHit;
    private bool updateLaser;
    private bool hitByLaser;
    private ArrayList lasers;
    private int bitmask;
    private int intensityDiminishAmount = 10;
    private GameObject[] orbs;

    // Use this for initialization
    void Start()
    {
        emitAngle = Emitter.transform.TransformDirection(Vector3.forward);
        defaultLaser.transform.forward = emitAngle;
        updateLaser = true;
        defaultLaser.GetComponent<SpriteRenderer>().color = laserColor;
        defaultLaser.transform.GetChild(0).GetComponent<SpriteRenderer>().color = laserColor;
        lasers = new ArrayList();
        if (deathLaser)
            bitmask = ~(1 << laserLayer);
        else
            bitmask = 2093057;

        defaultLaser.GetComponent<LaserIntensityManager>().SetLaserIntensity(startIntensity);
    }

    void Update()
    {
        orbs = GameObject.FindGameObjectsWithTag("Orb");
        hitByLaser = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //now we process orbs

    }


    void MakeLaserPath(Vector3 origin, Vector3 direction, GameObject laser)
    {
        if (this.isActiveAndEnabled && emitting)
        {  
            RaycastHit hitInfo;
            //start drawing the laser path
            laser.transform.right = direction;

            bool hitSomething = false;

            SetOrbsActive(false);

            if (Physics.Raycast(origin, direction, out hitInfo, 100.0f, bitmask))
            {
                //found hit

                ResizeLaser(hitInfo.distance, laser, origin);

                if (hitInfo.collider.tag == "Reflective")
                {
                    //reflect a laser
                    ReflectLaser(hitInfo, laser, direction);
                    hitSomething = true;
                }

                else if (hitInfo.collider.tag == "Catcher")
                {
                    Debug.Log("Start");
                    hitInfo.collider.GetComponent<LaserCollide>().laserHit(defaultLaser.GetComponent<SpriteRenderer>().color, laser.GetComponent<LaserIntensityManager>().GetLaserIntensity());
                    objectHit = hitInfo.collider.gameObject;
                    hitSomething = true;
                }

                else if ((hitInfo.collider.tag == "Laser" || hitInfo.collider.tag == "ReflectedLaser") && deathLaser)
                {
                    Debug.Log(hitInfo.collider.name);
                    hitInfo.collider.GetComponent<LaserPassToEmitter>().LaserHit(hitInfo.point, laser.GetComponent<SpriteRenderer>().color);
                    hitInfo.collider.gameObject.SetActive(false);
                    MakeLaserPath(origin, direction, laser);
                }

                if (!hitSomething && objectHit != null)
                {
                    Debug.Log("Stop");
                    objectHit.SendMessage("Stop", laser.GetComponent<SpriteRenderer>().color);
                    objectHit = null;
                }
            }
            SetOrbsActive(true);
            lasers.Add(laser);
        }
    }

    void ResizeLaser(float newLength, GameObject laser, Vector3 origin)
    {
        defaultLength = laser.GetComponent<Renderer>().bounds.size.x;
        Vector3 newScale = new Vector3(newLength, laser.transform.localScale.y, laser.transform.localScale.z);
        laser.transform.localScale = newScale;

        laser.transform.position = origin;
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
        newLaser.SetActive(true);
        newLaser.transform.localScale = new Vector3(.25f, 1, 1);
        newLaser.tag = "ReflectedLaser";

        //set new laser's intensity to previous laser's - const
        newLaser.GetComponent<LaserIntensityManager>().SetLaserIntensity(previousLaser.GetComponent<LaserIntensityManager>().GetLaserIntensity() - intensityDiminishAmount);

        //new laser must also call makeLaserPath
        MakeLaserPath(worldHitLoc, newLaserDirection, newLaser);
    }

    public void SetUpdateLasers(bool val)
    {
        defaultLaser.SetActive(false);
        lasers.Clear();
        emitAngle = Emitter.transform.TransformDirection(Vector3.forward);
        Vector3 direction = emitAngle;
        MakeLaserPath(Emitter.transform.position, direction, defaultLaser);
        defaultLaser.SetActive(true);
        updateLaser = false;
    }

    public void LaserHit(Vector3 position, Color color)
    {
        if (objectHit != null)
        {
            objectHit.SendMessage("Stop", color);
        }
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(false);
            if (laser.tag == "ReflectedLaser")
                Destroy(laser);
        }
    }

    private void SetOrbsActive(bool val)
    {
        foreach (GameObject orb in orbs)
            orb.SetActive(val);
    }
}
