using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Saber : MonoBehaviour {

    public int mode;

    Rigidbody rb;

    ContactPoint[] contacts;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void OnCollisionEnter(Collision collision) {
        //Debug.Log("Exit Works");
        /*int id;
        if(int.TryParse(collision.gameObject.name, out id)) {
            ProcessBeat(id);
            Debug.Log(id);
            collision.gameObject.SetActive(false);
        }*/
        if(collision.gameObject.tag == "Beat") {
            //Debug.Log("Hit");
            contacts = collision.contacts;
            InGameEvents.instance.RemoveBeat(int.Parse(collision.gameObject.name), collision.transform.position - GetCenterOfPoints(collision.contacts), mode);
        }
        //Destroy(collision.gameObject);
    }

    private void Update() {
        if(contacts != null) {
            for(int i = 0; i < contacts.Length; i++)
                Debug.DrawLine(transform.position, contacts[i].point, Color.red);

            Debug.DrawLine(transform.position, GetCenterOfPoints(contacts), Color.black);
        }
    }

    Vector3 GetCenterOfPoints(ContactPoint[] contacts) {
        Vector3 min = contacts[0].point;
        Vector3 max = contacts[0].point;

        for(int i = 1; i < contacts.Length; i++) {
            for(int j = 0; j < 3; j++) {
                if(min[j] > contacts[i].point[j])
                    min[j] = contacts[i].point[j];

                if(max[j] < contacts[i].point[j])
                    max[j] = contacts[i].point[j];
            }
        }

        return (min + max) / 2;
    }
}
