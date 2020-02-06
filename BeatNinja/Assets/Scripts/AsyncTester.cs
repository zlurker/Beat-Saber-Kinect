using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncTester : MonoBehaviour {

    int test;
	// Use this for initialization
    async void Start () {
        await Test();    
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("UpdateValue: " + test);
        
	}


    async Task Test() {
        while(true) {
            test++;
            await Task.Run(() => { Debug.Log("TestFuncValue: " + test); });
        }
    }
}
