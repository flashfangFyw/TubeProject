using System;
using System.Collections.Generic;

namespace UnityEngine.XR.iOS
{
	public class UnityARHitTestExample : MonoBehaviour
	{
		public Transform m_HitTransform;
        public GameObject[] objList;
        public UnityARGeneratePlane plane;
        private bool touchHitFlag = true;

        private void SetObjList(bool flag)
        {
            if (objList != null)
            {
                foreach (var obj in objList)
                {
                    obj.SetActive(flag);
                }
            }
        }
        bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
            if (hitResults.Count > 0) {
                foreach (var hitResult in hitResults) {
                    Debug.Log ("Got hit!");
                    m_HitTransform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
                    m_HitTransform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
                    SetObjList(true);
                    if (plane) plane.unityARAnchorManager.Destroy();
                    touchHitFlag = false;
                     Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
                    return true;
                }
            }
            return false;
        }
        void Start()
        {
            SetObjList(false);
        }
        // Update is called once per frame
        void Update () {
            UpdateUITouch();
            if (!touchHitFlag) return;
			if (Input.touchCount > 0 && m_HitTransform != null)
			{
				var touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
				{
					Vector3 screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

                    // prioritize reults types
                    ARHitTestResultType[] resultTypes = {
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                        // if you want to use infinite planes use this:
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
                        ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    }; 
					
                    foreach (ARHitTestResultType resultType in resultTypes)
                    {
                        if (HitTestWithResultType (point, resultType))
                        {
                            return;
                        }
                    }
				}
			}
		}
        private Ray ray;
        RaycastHit hit;
        private void UpdateUITouch()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                //ray = Camera.main.ScreenPointToRay(touch.position);// screenPosition);
                ray = Camera.main.ScreenPointToRay(touch.position);// screenPosition);
                                                                   //Graphics.ray
                Debug.Log("Raycast=" + Physics.Raycast(ray, out hit, 100));
                if (Physics.Raycast(ray, out hit, 100))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.green);
                    Debug.Log(" hit.collider.gameObject.name=" + hit.collider.gameObject.name);
                    Debug.Log(" hit.collider.gameObject.transform.localScale=" + hit.collider.gameObject.transform.localScale);
                    if (hit.collider.gameObject.name=="add")
                    {
                        hit.collider.gameObject.transform.parent.BroadcastMessage("AddParam", SendMessageOptions.DontRequireReceiver);
                    }
                    else
                    {
                        hit.collider.gameObject.transform.parent.BroadcastMessage("ReduceParam", SendMessageOptions.DontRequireReceiver);
                    }
                   
                }
            }
        }
	
	}
}

