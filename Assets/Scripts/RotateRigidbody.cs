using UnityEngine;
using System.Collections;

public class RotateRigidbody : MonoBehaviour {
    private bool rotate;
    private bool mobileRotate;

	// Use this for initialization
	void Start () {
        mobileRotate = false;
        rotate = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (rotate)
        {
            transform.GetChild(0).gameObject.SetActive(true);

            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                RotateCube(Input.mousePosition);
            }
            else if ((Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer))
            {
                if (Input.touchCount > 0 && !mobileRotate)
                    mobileRotate = true;
                else if (Input.touchCount == 0 && mobileRotate)
                {
                    mobileRotate = false;
                    rotate = false;
                }
                else
                {
                    RotateCube(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y));
                }
            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
	}

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) && rotate && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer))
        {
            setRotate(false);
        }
    }

    public void setRotate(bool val)
    {
        rotate = val;
    }

    private void RotateCube(Vector3 mouseScreenPos)
    {
        //Lock position of cube
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

        //Get cube position in screen coordinates:
        Vector3 cubeScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);

        //compute angle
        mouseScreenPos.x = mouseScreenPos.x - cubeScreenPos.x;
        mouseScreenPos.y = mouseScreenPos.y - cubeScreenPos.y;
        float angle = Mathf.Atan2(mouseScreenPos.y, mouseScreenPos.x) * Mathf.Rad2Deg;

        //set rotation
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, -1 * angle, this.transform.rotation.z);

        //unfreeze position
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    private void lockRigidBodyPos()
    {

    }

}
