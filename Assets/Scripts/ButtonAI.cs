using UnityEngine;
using System.Collections;

public class ButtonAI : MonoBehaviour {

    // Use this for initialization
    public float toggleWeight;
    public GameObject objectAttached;
    public string message;
    public Material toggledColor;
    public Material nonToggledColor;

    private bool toggled = false;
    private float totalMass;
    private Renderer rend;

	void Start () {
        totalMass = 0;
        rend = this.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckToggle();
	}

    public void OnTriggerEnter(Collider other)
    {
        totalMass += other.GetComponent<Rigidbody>().mass;
    }

    public void OnTriggerExit(Collider other)
    {
        totalMass -= other.GetComponent<Rigidbody>().mass;
    }

    private void CheckToggle()
    {
        if (!toggled)
        {
            if (totalMass >= toggleWeight)
            {
                objectAttached.SendMessage(message, true);
                toggled = true;
                rend.material = toggledColor;
            }
        }
        else
        {
            if (totalMass < toggleWeight)
            {
                objectAttached.SendMessage(message, false);
                toggled = false;
                rend.material = nonToggledColor;
            }
        }
    }


}
