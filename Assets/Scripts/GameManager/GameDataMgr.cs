using QZGameFramework.PersistenceDataMgr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr : Singleton<GameDataMgr>
{
    public MusicData musicData;

    public override void Initialize()
    {
        base.Initialize();
        musicData = JsonDataMgr.Instance.LoadData<MusicData>("MusicData");
        if (musicData == null)
        {
            musicData = new MusicData();
        }
    }

    public void SaveMusicData()
    {
        JsonDataMgr.Instance.SaveData(musicData, "MusicData");
    }
}