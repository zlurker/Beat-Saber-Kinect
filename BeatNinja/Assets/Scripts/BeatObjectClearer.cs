using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatObjectClearer {

    List<Renderer> beatsToDestroy = new List<Renderer>();

    public void DestroyBeat(int beat) {
        BeatmapCreator.dataInstance._notes[beat].noteRenderer.transform.SetParent(null);
        beatsToDestroy.Add(BeatmapCreator.dataInstance._notes[beat].noteRenderer);

    }

    public void Update() {

        for(int i = beatsToDestroy.Count - 1; i >= 0; i--) {
            beatsToDestroy[i].transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            beatsToDestroy[i].material.color -= new Color(0, 0, 0, 0.05f);

            if(beatsToDestroy[i].material.color.a <= 0) {
                beatsToDestroy[i].gameObject.SetActive(false);
                beatsToDestroy.RemoveAt(i);
            }
        }
    }

}
