using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotationTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public static double Pitch(Vector4 quaternion) {
        double value1 = 2.0 * (quaternion.w * quaternion.x + quaternion.y * quaternion.z);
        double value2 = 1.0 - 2.0 * (quaternion.x * quaternion.x + quaternion.y * quaternion.y);

        double roll = Math.Atan2(value1, value2);

        return roll * (180.0 / Math.PI);
    }

    /// <summary>
    /// Rotates the specified quaternion around the Y axis.
    /// </summary>
    /// <param name="quaternion">The orientation quaternion.</param>
    /// <returns>The rotation in degrees.</returns>
    public static double Yaw(Vector4 quaternion) {
        double value = 2.0 * (quaternion.w * quaternion.y - quaternion.z * quaternion.x);
        value = value > 1.0 ? 1.0 : value;
        value = value < -1.0 ? -1.0 : value;

        double pitch = System.Math.Asin(value);

        return pitch * (180.0 / Math.PI);
    }

    /// <summary>
    /// Rotates the specified quaternion around the Z axis.
    /// </summary>
    /// <param name="quaternion">The orientation quaternion.</param>
    /// <returns>The rotation in degrees.</returns>
    public static double Roll(Vector4 quaternion) {
        double value1 = 2.0 * (quaternion.w * quaternion.z + quaternion.x * quaternion.y);
        double value2 = 1.0 - 2.0 * (quaternion.y * quaternion.y + quaternion.z * quaternion.z);

        double yaw = Math.Atan2(value1, value2);

        return yaw * (180.0 / Math.PI);
    }


    // Update is called once per frame
    void Update () {
        KinectManager manager = KinectManager.Instance;

        if(manager && manager.IsInitialized()) {
            if(manager.IsUserDetected()) {
                long userId = manager.GetPrimaryUserID();
                if(manager.IsJointTracked(userId, (int)KinectInterop.JointType.Head)) {

                    //Quaternion inst = ;
                    //transform.rotation = manager.GetJointOrientation(userId, (int)KinectInterop.JointType.SpineBase, true) * manager.GetJointOrientation(userId, (int)KinectInterop.JointType.SpineMid, true) * manager.GetJointOrientation(userId, (int)KinectInterop.JointType.SpineShoulder, true) * manager.GetJointOrientation(userId, (int)KinectInterop.JointType.ShoulderLeft, true) * manager.GetJointOrientation(userId, (int)KinectInterop.JointType.ElbowLeft, true) * manager.GetJointOrientation(userId, (int)KinectInterop.JointType.WristLeft, true);
                    //innerChild.transform.eulerAngles = manager.GetJointOrientation(userId, (int)KinectInterop.JointType.ElbowLeft, true).eulerAngles;
                }
            }
        }
    }
}
