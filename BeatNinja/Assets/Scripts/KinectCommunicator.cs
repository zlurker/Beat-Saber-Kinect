using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectCommunicator : MonoBehaviour {

    public List<int[]> handSets;
    public GameObject handObject;
    Transform[] createdHandObject;
    public Transform mainObj;
    float initialY = 0;
    // Use this for initialization
    void Start() {
        handSets = new List<int[]>();

        handSets.Add(new int[] { (int)KinectInterop.JointType.HandLeft, (int)KinectInterop.JointType.ElbowLeft });
        handSets.Add(new int[] { (int)KinectInterop.JointType.HandRight, (int)KinectInterop.JointType.ElbowRight });

        createdHandObject = new Transform[handSets.Count];

        for(int i = 0; i < createdHandObject.Length; i++) {
            createdHandObject[i] = Instantiate(handObject).transform;
            createdHandObject[i].SetParent(mainObj);
        }
    }

    // Update is called once per frame
    void Update() {

        KinectManager manager = KinectManager.Instance;
        if(manager && manager.IsInitialized()) {
            if(manager.IsUserDetected()) {
                long userId = manager.GetPrimaryUserID();




                for(int i = 0; i < handSets.Count; i++) {
                    Vector3 handPos = manager.GetJointPosition(userId, handSets[i][0]);
                    Vector3 diff = handPos - manager.GetJointPosition(userId, handSets[i][1]);

                   

                    
                    // Mirrors hand position.
                    handPos.x *= -1;
                    diff.x *= -1;

                    // Gives a greater range of motion.
                    handPos.x *= 1.2f;
                    handPos.z *= 1.2f;
                    //diff.x *= 2.3f;




                    //handPos.y -= initialY *1.2f;
                    //diff.y -= initialY;

                    createdHandObject[i].localPosition = handPos;
                    createdHandObject[i].LookAt(createdHandObject[i].position + diff);
                }
            }
        }
    }
}
