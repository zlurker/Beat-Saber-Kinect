using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComboType {
    Add,Reset,No_Change
}

public class ScoreWindow {
    public string n;
    public float wP;
    public ComboType cT;
    public int cS;

    public static ScoreWindow[] scoreWindow = new ScoreWindow[] {
        //new ScoreWindow("Miss",0.0f,ComboType.Reset),
        new ScoreWindow("Perfect", 0.1f),
        new ScoreWindow("Great", 0.15f),
        new ScoreWindow("Good", 0.2f),
        new ScoreWindow("Bad", 0.25f)
    };

    public ScoreWindow(string name, float windowPeroid) {
        n = name;
        wP = windowPeroid;
    }

    public static int JudgementRange(float diff) {

        int wSelected = 0;

        for(int i = 0; i < scoreWindow.Length; i++)
            if(Mathf.Abs(diff) < scoreWindow[i].wP) {
                wSelected = i;
                break;
            }

        scoreWindow[wSelected].cS++;
        return wSelected;
    }
}

/*public static class InGameData {

    public static int currentCombo;

    


    
}*/
