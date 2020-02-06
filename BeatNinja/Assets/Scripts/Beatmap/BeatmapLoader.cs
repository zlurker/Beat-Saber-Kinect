using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class BeatmapLoadData {
    public string filePath;
    public BeatmapSongData songData;

    public BeatmapLoadData(string fp) {
        filePath = fp;
    }
}

#region Normal File Format
public class BeatmapSongData {
    public string songName;
    public string songSubName;
    public string coverImagePath;
    public string authorName;
    public float beatsPerMinute;
    public float songTimeOffset;

    public BeatmapLevelData[] difficultyLevels;
    public string[] categoryNames;
    public FileType fType;
}

public class BeatmapLevelData {
    public string difficulty;
    public int difficultyRank;
    public string audioPath;
    public string beatmapPath;
    public float offset;
    public int beatmapCharacteristic;
}
#endregion

#region DAT File Format
public class DATBeatmapSongData {
    public string _songName;
    public string _songSubName;
    public string _authorName;
    public float _beatsPerMinute;
    public float _songTimeOffset;
    public string _songFilename;

    public DATBeatmapLevelSetHolder[] _difficultyBeatmapSets;
}

public class DATBeatmapLevelSetHolder {
    public string _beatmapCharacteristicName;
    public DATBeatmapLevelData[] _difficultyBeatmaps;
}

public class DATBeatmapLevelData {
    public string _difficulty;
    public int _difficultyRank;
    public string _beatmapFilename;
}
#endregion

#region JSON File Format
public class JSONBeatmapSongData {
    public string songName;
    public string songSubName;
    public string authorName;
    public string coverImagePath;

    public JSONBeatmapLevelData[] difficultyLevels;
}

public class JSONBeatmapLevelData {
    public string difficulty;
    public int difficultyRank;
    public string audioPath;
    public string jsonPath;
    public float offset;
}
#endregion


public class BeatmapLoader {
    public static List<BeatmapLoadData> beatmapsInfo;
    public static DirectoryInfo songFolder;
    public static bool initialLoad = false;

    public static void LoadBeatmaps() {
        initialLoad = true;
        beatmapsInfo = new List<BeatmapLoadData>();

        if(songFolder == null)
            songFolder = new DirectoryInfo(Path.Combine(Application.dataPath, "Songs"));

        ScanSubDirectories(songFolder);

        //for (int i = 0; i < beatmapsInfo.Count; i++)
        //Debug.Log(beatmapsInfo[i].songData.difficultyLevels.Length);
    }

    public static void ScanSubDirectories(DirectoryInfo dir) {

        FileInfo[] files = dir.GetFiles("info.*");
        DirectoryInfo[] directories = dir.GetDirectories();


        if(files.Length > 0) {

            using(StreamReader sr = new StreamReader(files[0].FullName)) {
                string info = sr.ReadToEnd();
                BeatmapLoadData bLoadData = new BeatmapLoadData(dir.FullName);
                BeatmapSongData bSD = new BeatmapSongData();
                sr.Close();

                switch(files[0].Extension) {
                    case ".json":
                        JSONBeatmapSongData jbeatmapdata = JsonConvert.DeserializeObject<JSONBeatmapSongData>(info);

                        bSD.categoryNames = new string[] { "Normal" };
                        bSD.authorName = jbeatmapdata.authorName;
                        bSD.songName = jbeatmapdata.songName;
                        bSD.songSubName = jbeatmapdata.songSubName;
                        bSD.coverImagePath = jbeatmapdata.coverImagePath;
                        bSD.difficultyLevels = new BeatmapLevelData[jbeatmapdata.difficultyLevels.Length];

                        for(int i = 0; i < jbeatmapdata.difficultyLevels.Length; i++) {
                            BeatmapLevelData bLD = new BeatmapLevelData();

                            bLD.audioPath = jbeatmapdata.difficultyLevels[i].audioPath;
                            bLD.difficulty = jbeatmapdata.difficultyLevels[i].difficulty;
                            bLD.difficultyRank = jbeatmapdata.difficultyLevels[i].difficultyRank;
                            bLD.beatmapPath = jbeatmapdata.difficultyLevels[i].jsonPath;
                            bLD.offset = jbeatmapdata.difficultyLevels[i].offset;

                            bLD.beatmapCharacteristic = 0;

                            bSD.difficultyLevels[i] = bLD;
                        }

                        bSD.fType = FileType.JSON;
                        break;

                    case ".dat":
                        DATBeatmapSongData dbeatmapdata = JsonConvert.DeserializeObject<DATBeatmapSongData>(info);

                        string[] filenamedata = dbeatmapdata._songFilename.Split('.');

                        if(filenamedata[1].Equals("egg")) {
                            string songBytes = "";
                            FileInfo songFile = new FileInfo(Path.Combine(dir.FullName, dbeatmapdata._songFilename));

                            using(StreamReader songReader = new StreamReader(songFile.FullName)) {
                                songBytes = songReader.ReadToEnd();
                                songReader.Close();
                            }

                            string newPath = filenamedata[0] + ".ogg";
                            Debug.Log(Path.Combine(dir.FullName , newPath));

                            songFile.MoveTo(Path.Combine(dir.FullName, newPath));
                            dbeatmapdata._songFilename = newPath;

                            using(StreamWriter infoUpdate = new StreamWriter(files[0].FullName)) {
                                infoUpdate.Write(JsonConvert.SerializeObject(dbeatmapdata));
                                infoUpdate.Close();
                            }
                        }

                        bSD.authorName = dbeatmapdata._authorName;
                        bSD.beatsPerMinute = dbeatmapdata._beatsPerMinute;
                        bSD.songName = dbeatmapdata._songName;
                        bSD.songSubName = dbeatmapdata._songSubName;

                        FileInfo[] imageFile = dir.GetFiles("*.jpg");

                        if(imageFile.Length > 0)
                            bSD.coverImagePath = imageFile[0].FullName;

                        bSD.categoryNames = new string[dbeatmapdata._difficultyBeatmapSets.Length];

                        List<BeatmapLevelData> dlevellist = new List<BeatmapLevelData>();
                        for(int i = 0; i < dbeatmapdata._difficultyBeatmapSets.Length; i++) {
                            bSD.categoryNames[i] = dbeatmapdata._difficultyBeatmapSets[i]._beatmapCharacteristicName;

                            for(int j = 0; j < dbeatmapdata._difficultyBeatmapSets[i]._difficultyBeatmaps.Length; j++) {
                                BeatmapLevelData levelInst = new BeatmapLevelData();

                                levelInst.audioPath = dbeatmapdata._songFilename;
                                levelInst.beatmapCharacteristic = i;
                                levelInst.beatmapPath = dbeatmapdata._difficultyBeatmapSets[i]._difficultyBeatmaps[j]._beatmapFilename;
                                levelInst.difficulty = dbeatmapdata._difficultyBeatmapSets[i]._difficultyBeatmaps[j]._difficulty;
                                levelInst.difficultyRank = dbeatmapdata._difficultyBeatmapSets[i]._difficultyBeatmaps[j]._difficultyRank;

                                dlevellist.Add(levelInst);
                            }
                        }

                        bSD.difficultyLevels = dlevellist.ToArray();

                        bSD.fType = FileType.DAT;
                        break;

                }

                bLoadData.songData = bSD;
                beatmapsInfo.Add(bLoadData);
            }
        }

        /*files = dir.GetFiles("song.egg");

        if(files.Length > 0) {

            string info;
            using(StreamReader sr = new StreamReader(files[0].FullName)) {
                info = sr.ReadToEnd();
                sr.Close();
            }

            int length = files[0].FullName.Length;
            string path = files[0].FullName.Remove(length - 8);

            files[0].MoveTo(path + "Song.ogg");
        }*/

        for(int i = 0; i < directories.Length; i++)
            ScanSubDirectories(directories[i]);
    }
}
