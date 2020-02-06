using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointHierachyTest : MonoBehaviour {

    public KinectInterop.JointType joint;
    private float timeCount = 0.0f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        KinectManager manager = KinectManager.Instance;

        if(manager && manager.IsInitialized()) {
            if(manager.IsUserDetected()) {
                long userId = manager.GetPrimaryUserID();
                if(manager.IsJointTracked(userId, (int)joint)) {

                    //Quaternion inst = ;
                    //Quaternion comp = Quaternion.FromToRotation(new Vector3(floorPlane.X, floorPlane.Y, floorPlane.Z), Vector3.up);
                    Quaternion comp = Quaternion.FromToRotation(new Vector3(0, 0, 1), Vector3.up);
                    Quaternion inst = manager.GetJointOrientation(userId, (int)joint, true);
                    Windows.Kinect.Vector4 test = new Windows.Kinect.Vector4();
                    test.X = inst.x;

                    test.Y = inst.y;
                    test.Z = inst.z;
                    test.W = inst.w;
 
                    transform.rotation =  inst * Quaternion.AngleAxis(90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));

                    transform.localPosition = manager.GetJointPosition(userId, (int)joint);
                    //transform.rotation = WristLeft * Quaternion.AngleAxis(90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
                    //innerChild.transform.eulerAngles = manager.GetJointOrientation(userId, (int)KinectInterop.JointType.ElbowLeft, true).eulerAngles;
                }
            }
        }
    }

    private Quaternion VToQ(Windows.Kinect.Vector4 kinectQ, Quaternion comp) {
        return Quaternion.Inverse(comp) * (new Quaternion(-kinectQ.X, -kinectQ.Y, kinectQ.Z, kinectQ.W));
    }
}
