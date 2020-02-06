using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI {

    
    Canvas uiInterface;

    public Text comboTracker;

    RawImage songImage;
    Text songName;
    Text songSubName;
    Text songDifficulty;

    public InGameUI(Canvas ui) {
        uiInterface = ui;
        comboTracker = uiInterface.transform.Find("comboAmount").GetComponent<Text>();
        songName = uiInterface.transform.Find("songName").GetComponent<Text>();
        songSubName = uiInterface.transform.Find("songSubName").GetComponent<Text>();
        songDifficulty = uiInterface.transform.Find("songDifficulty").GetComponent<Text>();
        songImage = uiInterface.transform.Find("songImage").GetComponent<RawImage>();

        WWW image = new WWW("file:///" + Path.Combine(BeatmapLoader.beatmapsInfo[MenuScript.menuIndexes[0]].filePath,
        BeatmapLoader.beatmapsInfo[MenuScript.menuIndexes[0]].songData.coverImagePath));

        songImage.texture = image.texture;
        songName.text = BeatmapLoader.beatmapsInfo[MenuScript.menuIndexes[0]].songData.songName;
        songSubName.text = BeatmapLoader.beatmapsInfo[MenuScript.menuIndexes[0]].songData.songSubName;
        songDifficulty.text = BeatmapLoader.beatmapsInfo[MenuScript.menuIndexes[0]].songData.difficultyLevels[MenuScript.menuIndexes[1]].difficulty;

    }
}
