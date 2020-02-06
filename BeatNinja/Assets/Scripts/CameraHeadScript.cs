using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeadScript : MonoBehaviour {
    KinectManager manager;
    // Use this for initialization
    void Start () {
        manager = KinectManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {
        
        if(manager && manager.IsInitialized()) {
            if(manager.IsUserDetected()) {
                long userId = manager.GetPrimaryUserID();
                if(manager.IsJointTracked(userId, (int)KinectInterop.JointType.Head)) {
                    
                    transform.position = manager.GetJointPosition(userId, (int)KinectInterop.JointType.Head);
                    transform.rotation = manager.GetJointOrientation(userId, (int)KinectInterop.JointType.Head, true);
                }
            }
        }
    }
}
