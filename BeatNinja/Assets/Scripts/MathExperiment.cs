using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathExperiment : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += CalculateVelocity(210);

    }

    Vector3 CalculateVelocity(float angle) {
        int quarter = Mathf.FloorToInt(angle / 90);
        Vector3 velo = new Vector3();

        float anRad = AngleToRad(angle,quarter);

        switch(quarter) {
            case 0:
            case 4:
                velo = new Vector3(Mathf.Sin(anRad), Mathf.Cos(anRad));
                break;
            case 1:
                velo = new Vector3(Mathf.Cos(anRad), -Mathf.Sin(anRad));
                break;
            case 2:
                velo = new Vector3(-Mathf.Sin(anRad), -Mathf.Cos(anRad));
                break;
            case 3:
                velo = new Vector3(-Mathf.Cos(anRad), Mathf.Sin(anRad));
                break;
        }

        return velo;
    }

    float AngleToRad(float angle, int quarter) {
        angle -= quarter * 90;
        return angle * Mathf.Deg2Rad;
    }
}
