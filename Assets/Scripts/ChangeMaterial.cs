using UnityEngine;
using System.Collections;

public class ChangeMaterial : MonoBehaviour {

    // Use this for initialization
    public Material startColor;
    public Material endColor;
    public bool change = false;
    public float durationSec = 3;

    private float currDuration = 0;
    private Renderer rend;
    private Color startEmit;
    private Color endEmit;
    private bool fullyOn;

	void Start () {
        rend = this.GetComponent<Renderer>();
        //startColor = rend.material;
        rend.material.EnableKeyword("_EMISSION");
        endColor.EnableKeyword("_EMISSION");
        startEmit = rend.material.GetColor("_EmissionColor");
        endEmit = endColor.GetColor("_EmissionColor");
	}
	
	// Update is called once per frame
	void Update () {
        //check if we change colors
        if (change && currDuration < durationSec)
        {
            fullyOn = false;
            ColorChangeForward();
            if (GetComponent<FollowPath>())
            {
                GetComponent<FollowPath>().SetMoving(false);
            }
        }
        else if (!change && currDuration > 0)
        {
            fullyOn = false;
            ColorChangeBack();
            if (GetComponent<FollowPath>())
            {
                GetComponent<FollowPath>().SetMoving(false);
            }
        }
        else if (currDuration >= durationSec)
        {
            fullyOn = true;
            if (GetComponent<FollowPath>())
            {
                GetComponent<FollowPath>().SetMoving(true);
            }
        }
	}

    public void ColorChangeForward()
    {
        Debug.Log("Color Changing Forward");
        currDuration += Time.deltaTime;
        if (currDuration >= durationSec)
            currDuration = durationSec;
        rend.material.Lerp(startColor, endColor, currDuration / durationSec);
    }

    public void ColorChangeBack()
    {
        Debug.Log("Color Changing Back...");
        currDuration -= Time.deltaTime;
        if (currDuration < 0)
            currDuration = 0;
        rend.material.Lerp(startColor, endColor, currDuration / durationSec);
    }

    public void SetChange(bool val)
    {
        change = val;
    }

    public bool GetFullyOn()
    {
        return fullyOn;
    }
}
