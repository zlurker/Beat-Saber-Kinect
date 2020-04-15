using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Saber : MonoBehaviour {

    public int mode;
    public GameObject swordTip;
    public float threshold; 

    Rigidbody rb;

    Vector3 prevPos;
    Vector3 currDir;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        threshold = 0.8f;
    }

    private void OnCollisionEnter(Collision collision) {
        //Debug.Log("Exit Works");
        /*int id;
        if(int.TryParse(collision.gameObject.name, out id)) {
            ProcessBeat(id);
            Debug.Log(id);
            collision.gameObject.SetActive(false);
        }*/
        if(collision.gameObject.tag == "Beat")
            InGameEvents.instance.RemoveBeat(int.Parse(collision.gameObject.name), swordTip.transform.position - prevPos, mode);

        //Destroy(collision.gameObject);
    }

    void Update() {
        //float totalMovement = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(prevPos.x)) + Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(prevPos.y)) + Mathf.Abs(Mathf.Abs(transform.position.z) - Mathf.Abs(prevPos.z));

        float totalMovement = (transform.position - prevPos).magnitude;

        if(totalMovement >= threshold)
            prevPos = swordTip.transform.position;

        Debug.DrawLine(swordTip.transform.position, prevPos, Color.red);


    }
}
