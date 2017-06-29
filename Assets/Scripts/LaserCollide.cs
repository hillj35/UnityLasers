using UnityEngine;
using System.Collections;

public class LaserCollide : MonoBehaviour {

    public Color catcherColor;
    public GameObject attached;
    public GameObject[] colorChangeObjs;
    public string message;
    public bool negate;

    private int lastHitIntensity = 0;


    private bool toggled;

	// Use this for initialization
	void Start () {
        toggled = false;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void laserHit(Color laserColor, int intensity)
    {
        Vector4 color1 = (Vector4)laserColor;
        Vector4 color2 = (Vector4)catcherColor;

        if (color1.magnitude == color2.magnitude)
        {
            //we in there
            if (toggled == false)
            {
                toggled = true;
                lastHitIntensity = intensity;
                attached.SendMessage(message, lastHitIntensity);
                
                SendColorChangeMessages(true);
            }
        }
    }

    public void Stop(Color laserColor)
    {
        if (Equals(laserColor, catcherColor))
        {
            toggled = false;
            attached.SendMessage(message, -1 * lastHitIntensity);
            SendColorChangeMessages(false);
        }
    }

    private void SendColorChangeMessages(bool hit)
    {
        foreach (GameObject obj in colorChangeObjs)
        {
            obj.GetComponent<ChangeMaterial>().SetChange(hit);
        }
    }
}
