using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//Script which runs the main game level.
public class InGameEvents : MonoBehaviour {

    public static InGameEvents instance;

    public BeatObjectClearer objectClearer;
    public InGameUI scoreTracker;

    public static AudioSource songPlayer;
    float offset;
    private bool playerHeightInitialised;
    private Vector3 iniPos;

    private Canvas ui;

    private int currCombo;
    private int currTrackedBeat;
    // Use this for initialization
    void Start() {
        instance = this;

        ui = FindObjectOfType<Canvas>();
        songPlayer = GetComponent<AudioSource>();
        objectClearer = new BeatObjectClearer();
        scoreTracker = new InGameUI(ui);

        //Creates the beatmap
        BeatmapParser.Parse();
        BeatmapCreator.CreateChart();

        //Imports the music
        WWW song = new WWW("file:///" + Path.Combine(BeatmapLoader.beatmapsInfo[MenuScript.menuIndexes[0]].filePath,
            BeatmapLoader.beatmapsInfo[MenuScript.menuIndexes[0]].songData.difficultyLevels[MenuScript.menuIndexes[1]].audioPath));

        while(!song.isDone) ;
        songPlayer.clip = song.GetAudioClip(false);

        songPlayer.Play();
    }

    public void RemoveBeat(int id, Vector3 movement, int saberType) {

        
        if(!BeatmapParser.beatmap._notes[id].noteDestroyed) {
            Debug.Log("InitialMovement:" + movement);

            bool dirWrong = false;
            movement.x /= Mathf.Abs(movement.x);
            movement.y /= Mathf.Abs(movement.y);
            movement.z = 0;

            for(int i = 0; i < 2; i++)
                if(movement[i] != 0)
                    if(movement[i] != BeatmapCreator.directionalCuts[BeatmapParser.beatmap._notes[id]._cutDirection][i])
                        dirWrong = true;

            if(!dirWrong) {
                ProcessBeat(id);
                Debug.Log("Beat hit!");
            } else
                HandleCombo(ComboType.Reset);

            Debug.LogFormat("MovementGenerated:{0}, GivenDirection:{1}, BeatID:{2}", movement, BeatmapCreator.directionalCuts[BeatmapParser.beatmap._notes[id]._cutDirection], id);

            objectClearer.DestroyBeat(id);
            BeatmapCreator.dataInstance._notes[id].noteDestroyed = true;
        }
    }

    void Update() {

        if(!playerHeightInitialised)
            AdjustHeight();

        float magnitude = ((songPlayer.time) / 60) * BeatmapParser.beatmap._beatsPerMinute * BeatmapCreator.speedMultiplier;
        BeatmapCreator.beatmapRoot.transform.position = new Vector3(iniPos.x, iniPos.y, iniPos.z - magnitude);

        objectClearer.Update();
        TrackBeats();
    }

    void AdjustHeight() {

        KinectManager manager = KinectManager.Instance;

        if(manager && manager.IsInitialized()) {
            if(manager.IsUserDetected()) {
                long userId = manager.GetPrimaryUserID();
                if(manager.IsJointTracked(userId, (int)KinectInterop.JointType.SpineBase)) {

                    iniPos = manager.GetJointPosition(userId, (int)KinectInterop.JointType.SpineMid) - BeatmapCreator.GetLocalPosition(0.5f, 0);

                    // Moves stepchart to its correct place.
                    BeatmapCreator.beatmapRoot.transform.position = iniPos;

                    // Releases canvas.
                    ui.transform.SetParent(null);
                    ui.transform.position = iniPos + new Vector3(0, 0, 250);
                    ui.transform.rotation = new Quaternion();

                    playerHeightInitialised = true;
                }
            }
        }
    }

    #region Beat Processing Code

    int ProcessBeat(int beatId) {
        int judgement = -1;


        judgement = ScoreWindow.JudgementRange(RhythmGameMath.GetTimeFromBPM(BeatmapParser.beatmap._notes[beatId]._time, BeatmapParser.beatmap._beatsPerMinute) - InGameEvents.songPlayer.time);

        switch(ScoreWindow.scoreWindow[judgement].n) {
            case "Perfect":
            case "Great":
            case "Good":
                HandleCombo(ComboType.Add);
                break;

            case "Bad":
                HandleCombo(ComboType.Reset);
                break;
        }


        return judgement;
    }

    void TrackBeats() {

        if(currTrackedBeat < BeatmapParser.beatmap._notes.Length)
            if(RhythmGameMath.GetTimeFromBPM(BeatmapParser.beatmap._notes[currTrackedBeat]._time, BeatmapParser.beatmap._beatsPerMinute) + 0.5f < InGameEvents.songPlayer.time) {
                if(!BeatmapParser.beatmap._notes[currTrackedBeat].noteDestroyed)
                    HandleCombo(ComboType.Reset);

                currTrackedBeat++;
            }
    }

    void HandleCombo(ComboType comboType) {
        switch(comboType) {
            case ComboType.Add:
                currCombo++;
                break;

            case ComboType.No_Change:

                break;

            case ComboType.Reset:
                currCombo = 0;
                break;
        }

        scoreTracker.comboTracker.text = currCombo.ToString();
    }

    #endregion
}
