using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHolder : MonoBehaviour
{
    public Texture[] ts;
    public static Texture[] textures;

    void Start()
    {
        if (!BeatmapLoader.initialLoad)
            textures = ts;
    }

}
