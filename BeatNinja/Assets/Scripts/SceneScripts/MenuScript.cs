using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//Script that runs the menu.
public class MenuScript : MonoBehaviour {

    public static int[] menuIndexes;
    public Text songName;
    public Text artist;
    public Text difficulty;

	void Start () {
        if (!BeatmapLoader.initialLoad)
        {
            BeatmapLoader.LoadBeatmaps();
            menuIndexes = new int[2];
        }
        
        UpdateUI();
	}
	
    public void ChangeSong(int index)
    {
        menuIndexes[0] = CheckLimit(menuIndexes[0] + index, BeatmapLoader.beatmapsInfo.Count);
        menuIndexes[1] = 0;
        UpdateUI();
    }

    public void ChangeLevel(int index)
    {
        menuIndexes[1] = CheckLimit(menuIndexes[1]+index, BeatmapLoader.beatmapsInfo[menuIndexes[0]].songData.difficultyLevels.Length);
        UpdateUI();
    }

    public void StartGame()
    {
        Debug.Log("Working");
        SceneManager.LoadScene("LevelScene");
    }

    int CheckLimit(int currValue, int max)
    {
        if (currValue < 0)
            return max - 1;

        if (currValue >= max)
            return 0;

        return currValue;
    }

    public void UpdateUI()
    {
        BeatmapSongData dataInst = BeatmapLoader.beatmapsInfo[menuIndexes[0]].songData;
        songName.text = dataInst.songName;
        artist.text = dataInst.authorName;
        difficulty.text = dataInst.difficultyLevels[menuIndexes[1]].difficulty;
    }
}
