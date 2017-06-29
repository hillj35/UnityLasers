using UnityEngine;
using System.Collections;

public class LaserIntensityManager : MonoBehaviour {
    private int laserIntensity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	  
	}

    public void SetLaserIntensity(int val)
    {
        laserIntensity = val;
    }

    public int GetLaserIntensity()
    {
        return laserIntensity;
    }
}
