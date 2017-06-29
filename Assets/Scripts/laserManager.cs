using UnityEngine;
using System.Collections;

public class laserManager : MonoBehaviour {
    public GameObject[] emitters;

    private int frameCount;
    private bool updateLasers;

    // Use this for initialization
    void Start()
    {
        frameCount = 0;
        updateLasers = true;
    }
	
	// Update is called once per frame
	void Update () {
        frameCount++;
        //check all rigidbodies for movement
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Reflective"))
        {
            if (obj.GetComponent<Rigidbody>().IsSleeping() == false)
            {
                updateLasers = true;
            }
                
        }
        if (frameCount % 4 == 0 && updateLasers)
        {
            //destroy previous lasers
            DestroyLasers();

            //find new laser paths
            for (int i = 0; i < emitters.Length; i++)
            {
                foreach(GameObject obj in emitters)
                    obj.SendMessage("SetUpdateLasers", true);

                updateLasers = false;
            }
        }
        else
        {
            //foreach (GameObject obj in emitters)
            //    obj.SendMessage("SetUpdateLasers", false);
        }
	}

    public void DestroyLasers()
    {
        GameObject[] objects;
        objects = GameObject.FindGameObjectsWithTag("ReflectedLaser");
        Debug.Log("Reflected Laser Count: " + objects.Length);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
            obj.SetActive(false);
        }
    }
}
