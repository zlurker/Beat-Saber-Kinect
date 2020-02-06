using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGameMath{

    public static float GetTimeFromBPM(float currBeat, float bpm) {
        return (currBeat / bpm) * 60;
    }

    public static float ReflectedAngle(float angle) {
        angle -= 180;
        angle = angle >= 0 ? angle : 360 - angle;
        return angle;
    }
}
