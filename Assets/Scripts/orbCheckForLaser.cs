using UnityEngine;
using System.Collections;

public class orbCheckForLaser : MonoBehaviour {
    public GameObject endLevel;

    private ChangeMaterial cm;
    private bool laserIn = false;

	// Use this for initialization
	void Start () {
        cm = GetComponent<ChangeMaterial>();
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void LateUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, transform.lossyScale.x / 2);

        bool laserUsedToBeIn = laserIn;
        laserIn = false;

        foreach (Collider c in hitColliders)
        {
            if (c.tag == "Laser" || c.tag == "ReflectedLaser")
            {
                if (!laserUsedToBeIn)
                {
                    endLevel.GetComponent<EndLevel>().ChangeLightIntensity(1);
                    
                }
                cm.SetChange(true);
                laserIn = true;
            }
        }

        if (laserUsedToBeIn && !laserIn)
        {
            endLevel.GetComponent<EndLevel>().ChangeLightIntensity(-1);
            cm.SetChange(false);
        }
    }

    //void OnTriggerEnter (Collider col)
    //{
    //    Debug.Log(col.gameObject.name);
    //    if ((col.gameObject.tag == "Laser" || col.gameObject.tag == "ReflectedLaser") && !laserIn)
    //    {
    //        laserIn = true;
    //        endLevel.GetComponent<EndLevel>().ChangeLightIntensity(1);
    //    }
    //}

    //void OnCollisionExit (Collider col)
    //{
    //    if (col.gameObject.tag == "Laser" || col.gameObject.tag == "ReflectedLaser")
    //    {
    //        endLevel.GetComponent<EndLevel>().ChangeLightIntensity(-1);
    //        laserIn = false;
    //    }
    //}
}
