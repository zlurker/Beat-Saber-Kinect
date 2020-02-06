using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapCreator : MonoBehaviour {
    //Root of the chart.
    public static Transform beatmapRoot;
    //dataInstance of current beatmap data.
    public static BeatmapData dataInstance;
    //Affects spacing/speed of chart
    public static float speedMultiplier = 2.5f;
    public static float size = 0.4f;
    public static Vector2 dimensions = new Vector2(3, 2);

    //Angle values from beatmap
    public static float[] angleValues = {
        90,
        270,
        180,
        0,
        135,
        45,
        225,
        315
    };

    //Directional cuts of each beat to be matched during saber cut processing.
    public static Vector3[] directionalCuts = {
        new Vector3(0,1,0),
        new Vector3(0,-1,0),
        new Vector3(-1,0,0),
        new Vector3(1,0,0),
        new Vector3(-1,1,0),
        new Vector3(1,1,0),
        new Vector3(-1,-1,0),
        new Vector3(1,-1,0)
    };

    public static Material obsMaterial = Resources.Load("WireframeTransparent") as Material;
    // Use this for initialization
    public static void CreateChart() {
        //Creates a root to hold all the notes/obstacles

        Transform root = new GameObject().transform;
        root.name = "root";
        beatmapRoot = root;


        //Finds the current beatmap
        dataInstance = BeatmapParser.beatmap;

        //Create beats
        CreateBeats();

        //Create obstacles
        CreateObstacles();
    }

    public static Vector3 GetLocalPosition(float x, float y) {
        return new Vector3(x, y, 0) * dimensions * size;
    }

    public static void CreateBeats() {
        //Creates a oringinal copy of the beat       
        GameObject beat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Renderer beatDesign = beat.GetComponent<Renderer>();
        beatDesign.material.shader = Shader.Find("Transparent/Diffuse");

        for(int i = 0; i < dataInstance._notes.Length; i++) {
            Renderer copy = Instantiate(beatDesign);

            copy.tag = "Beat";

            //Sorts beat position, root and scale
            copy.transform.position = ReturnPositionOfObject(dataInstance._notes[i]._lineIndex, dataInstance._notes[i]._lineLayer, dataInstance._notes[i]._time);
            copy.transform.parent = beatmapRoot;
            copy.transform.name = i.ToString();
            copy.transform.localScale *= size * 0.8f;
            //Sort beat design according to its type
            switch(dataInstance._notes[i]._type) {
                case 0:
                    copy.material.color = Color.red;
                    break;

                case 1:
                    copy.material.color = Color.blue;
                    break;

                case 3:
                    copy.material.color = Color.black;
                    break;
            }

            //copy.material.mainTexture = SpriteHolder.textures[0];
            //Sort beat rotation/artwork according to its cut dir
            switch(dataInstance._notes[i]._cutDirection) {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    copy.material.mainTexture = SpriteHolder.textures[0];
                    copy.transform.rotation = Quaternion.Euler(0, 0, angleValues[dataInstance._notes[i]._cutDirection]);
                    break;

                case 8:
                    copy.material.mainTexture = SpriteHolder.textures[1];
                    break;
            }

            dataInstance._notes[i].noteRenderer = copy;
        }

        //Deactivates orginal object so it doesn't appear.
        beatDesign.gameObject.SetActive(false);
    }

    public static void CreateObstacles() {
        //Creates a orginal copy of the obstacles.
        GameObject beat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Renderer beatDesign = beat.GetComponent<Renderer>();

        for(int i = 0; i < dataInstance._obstacles.Length; i++) {
            //Creates copy of original obstacle
            Renderer copy = Instantiate(beatDesign);
            int yPos = 0;

            //Changes yPos to 2 if its type 1 in dataInstance
            if(dataInstance._obstacles[i]._type == 1)
                yPos = 2;

            copy.transform.position = ReturnPositionOfObject(dataInstance._obstacles[i]._lineIndex, yPos, dataInstance._obstacles[i]._time);
            //Makes a new pivot for created clone
            GameObject pivot = new GameObject("Obstacles Pivot");
            pivot.transform.position = copy.transform.position;
            //Sets the offset
            pivot.transform.position -= copy.transform.localScale / 2;
            //Parents copy to pivot
            copy.transform.parent = pivot.transform;

            copy.material = obsMaterial;
            //Color color = beatDesign.material.color;
            //color.a = 0.4f;
            //beatDesign.material.color = color;


            pivot.transform.localScale = new Vector3(dataInstance._obstacles[i]._width * size, 3, dataInstance._obstacles[i]._duration * speedMultiplier);
            copy.transform.position -= new Vector3(0, 0.5f, 0);
            pivot.transform.parent = beatmapRoot;
        }

        beat.SetActive(false);
    }


    public static Vector3 ReturnPositionOfObject(int _lineIndex, int _lineLayer, float time) {
        return new Vector3(_lineIndex * size, _lineLayer * size, time * speedMultiplier);
    }
}
