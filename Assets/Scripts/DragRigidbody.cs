using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class DragRigidbody : MonoBehaviour
    {
        const float k_Spring = 50.0f;
        const float k_Damper = 5.0f;
        const float k_Drag = 5.0f;
        const float k_AngularDrag = 5.0f;
        const float k_Distance = 0.2f;
        const bool k_AttachToCenterOfMass = false;

        private SpringJoint m_SpringJoint;
        private GameObject objHit;
        private float holdTime = 0.3f; //time in seconds mouse/finger must be held down to drag
        private float currHoldTime;

        private void Update()
        {
            // Make sure the user pressed the mouse down
            if (Input.touchCount != 1 && (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer))
            {
                currHoldTime = 0;
                return;
            }

            if (Input.GetMouseButtonUp(0) && objHit != null && Application.platform == RuntimePlatform.WindowsEditor && currHoldTime < holdTime)
            {
                //tapped obj, should rotate it
                //objHit.GetComponent<RotateRigidbody>().setRotate(true);
            }

            else if (!Input.GetMouseButton(0))
            {
                currHoldTime = 0;
                return;
            }
            else if (!Input.GetMouseButtonDown(0))
            {
                return;
            }


            var mainCamera = FindCamera();

            // We need to actually hit an object
            RaycastHit hit = new RaycastHit();
            if (
                !Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                                 mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
                                 Physics.DefaultRaycastLayers))
            {
                objHit = null;
                return;
            }
            // We need to hit a rigidbody that is not kinematic
            if (!hit.rigidbody || hit.rigidbody.isKinematic)
            {
                objHit = null;
                return;
            }

            objHit = hit.collider.gameObject;
            Debug.Log(objHit.name);

            if (!m_SpringJoint)
            {
                var go = new GameObject("Rigidbody dragger");
                Rigidbody body = go.AddComponent<Rigidbody>();
                m_SpringJoint = go.AddComponent<SpringJoint>();
                body.isKinematic = true;
            }

            m_SpringJoint.transform.position = hit.point;
            m_SpringJoint.anchor = Vector3.zero;

            m_SpringJoint.spring = k_Spring;
            m_SpringJoint.damper = k_Damper;
            m_SpringJoint.maxDistance = k_Distance;
            m_SpringJoint.connectedBody = hit.rigidbody;

            StartCoroutine("DragObject", hit.distance);
        }


        private IEnumerator DragObject(float distance)
        {
            var oldDrag = m_SpringJoint.connectedBody.drag;
            var oldAngularDrag = m_SpringJoint.connectedBody.angularDrag;
            m_SpringJoint.connectedBody.drag = k_Drag;
            m_SpringJoint.connectedBody.angularDrag = k_AngularDrag;
            var mainCamera = FindCamera();

            //m_SpringJoint.connectedBody.constraints = RigidbodyConstraints.FreezeRotation;

            RaycastHit rh = new RaycastHit();
            float start = Time.realtimeSinceStartup;
            while (currHoldTime < holdTime && Input.GetMouseButton(0))
            {
                currHoldTime += Time.realtimeSinceStartup - start;
                start = Time.realtimeSinceStartup;
                Debug.Log(currHoldTime.ToString());
                yield return null;
            }

            bool dragged = false;

            if (Input.GetMouseButton(0))
            {
                dragged = true;
                while (Input.GetMouseButton(0)) //dragging rigidbody
                {
                    //lock position/rotation of other rigidbodies
                    foreach (Rigidbody obj in GameObject.FindObjectsOfType<Rigidbody>())
                    {
                        obj.constraints = RigidbodyConstraints.FreezeAll;
                    }

                    //unlock one we're dragging
                    m_SpringJoint.connectedBody.constraints = RigidbodyConstraints.FreezeRotation;

                    //store rigidbody's layer then ignore raycast
                    int oldLayer = m_SpringJoint.connectedBody.gameObject.layer;
                    m_SpringJoint.connectedBody.gameObject.layer = 2;

                    //raycast for location of floor
                    Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                                        mainCamera.ScreenPointToRay(Input.mousePosition).direction, out rh, 100, 1 << 20);

                    //set back to old layer
                    m_SpringJoint.connectedBody.gameObject.layer = oldLayer;

                    //setting positions
                    Vector3 position = new Vector3(rh.point.x, rh.point.y + 1, rh.point.z);
                    m_SpringJoint.transform.position = position;
                    Vector3 rigidBodyPos = m_SpringJoint.connectedBody.position;
                    m_SpringJoint.connectedBody.MovePosition(new Vector3(rigidBodyPos.x, position.y, rigidBodyPos.z));

                    yield return null;
                }

                //unlock position/rotation of other rigidbodies
                foreach (Rigidbody obj in GameObject.FindObjectsOfType<Rigidbody>())
                {
                    obj.constraints = RigidbodyConstraints.None;
                }
            }

            else if (!dragged)
            {
                if (objHit != null && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer))
                {
                    objHit.GetComponent<RotateRigidbody>().setRotate(true);
                    objHit = null;
                }
                else if (objHit != null && (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer))
                {
                    objHit.GetComponent<RotateRigidbody>().setRotate(true);
                    objHit = null;
                }
            }
            
            if (m_SpringJoint.connectedBody)
            {
                m_SpringJoint.connectedBody.drag = 0;
                m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
                m_SpringJoint.connectedBody.constraints = RigidbodyConstraints.None;
                m_SpringJoint.connectedBody = null;
            }
            
        }


        private Camera FindCamera()
        {
            if (GetComponent<Camera>())
            {
                return GetComponent<Camera>();
            }

            return Camera.main;
        }
    }
}
