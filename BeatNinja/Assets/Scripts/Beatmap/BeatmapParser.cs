using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

[System.Serializable]
public class BeatmapData {
    public float _beatsPerMinute;
    public BPMChangeData[] _BPMChanges;
    public NotesData[] _notes;
    public ObstacleData[] _obstacles;
    public BookmarkData[] _bookmarks;
}

public class DATBeatmapData {
    public BPMChangeData[] _BPMChanges;
    public NotesData[] _notes;
    public ObstacleData[] _obstacles;
    public BookmarkData[] _bookmarks;
}

public class JSONBeatmapData {
    public float _beatsPerMinute;
    public NotesData[] _notes;
    public ObstacleData[] _obstacles;
}

public class BPMChangeData {
    public float _time;
}

[System.Serializable]
public class NotesData {
    public float _time;
    public int _lineIndex;
    public int _lineLayer;
    public int _type;
    public int _cutDirection;
    public Renderer noteRenderer;
    public bool noteDestroyed;
}

[System.Serializable]
public class ObstacleData {
    public float _time;
    public int _lineIndex;
    public int _type;
    public float _duration;
    public int _width;
}

public class BookmarkData {
}

public enum FileType {
    JSON, DAT
}

public static class BeatmapParser {

    public static BeatmapData beatmap;

    public static void Parse() {
        string filePath = Path.Combine(BeatmapLoader.beatmapsInfo[MenuScript.menuIndexes[0]].filePath,
            BeatmapLoader.beatmapsInfo[MenuScript.menuIndexes[0]].songData.difficultyLevels[MenuScript.menuIndexes[1]].beatmapPath);

        FileType fT = BeatmapLoader.beatmapsInfo[MenuScript.menuIndexes[0]].songData.fType;

        beatmap = new BeatmapData();

        using(StreamReader reader = new StreamReader(filePath)) {
            string text = reader.ReadToEnd();

            switch(fT) {
                case FileType.JSON:
                    JSONBeatmapData jbeatmap = JsonConvert.DeserializeObject<JSONBeatmapData>(text);

                    beatmap._beatsPerMinute = jbeatmap._beatsPerMinute;
                    beatmap._notes = jbeatmap._notes;
                    beatmap._obstacles = jbeatmap._obstacles;
                    break;

                case FileType.DAT:
                    DATBeatmapData dbeatmap = JsonConvert.DeserializeObject<DATBeatmapData>(text);

                    beatmap._beatsPerMinute = BeatmapLoader.beatmapsInfo[MenuScript.menuIndexes[0]].songData.beatsPerMinute;
                    beatmap._BPMChanges = dbeatmap._BPMChanges;
                    beatmap._notes = dbeatmap._notes;
                    beatmap._obstacles = dbeatmap._obstacles;
                    beatmap._bookmarks = dbeatmap._bookmarks;
                    break;
            }

            if(beatmap._BPMChanges != null || beatmap._bookmarks != null)
                Debug.LogError("BPMCHANGES: " + beatmap._BPMChanges.Length + ", BOOKMARKS: " + beatmap._bookmarks.Length);

            reader.Close();
        }
    }
}
