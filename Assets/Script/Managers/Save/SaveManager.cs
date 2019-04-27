using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
//using EventApp.API;
//using EventApp.API.Models;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    [Serializable]
    public class SaveData
    {
        public DateTime VersionTime;
        public int UserId;
        public bool firstTutoRead;
        public bool gotTeaserCongrats;
        public bool gotMainCongrats;
        public bool teaserEventTutoRead;
        public bool mainEventTutoRead;
        public bool galleryTutoRead;
        public bool badgeTutoRead;
        public bool eventTeaserStarted;
        public bool eventTeaserLocked;
        public bool eventMainStarted;
        public List<bool> quizzAnswered = new List<bool>();
        public List<bool> artefactsUnlocked = new List<bool>();
        public List<bool> badgesUnlocked = new List<bool>();
        public int pokemonScore;
        public int axeScore;
        //TODO: Add game data fields here

        public SaveData()
        {
            VersionTime = DateTime.UtcNow;
            UserId = 1306;
            firstTutoRead = false;
            gotTeaserCongrats = false;
            gotMainCongrats = false;
            teaserEventTutoRead = false;
            mainEventTutoRead = false;
            galleryTutoRead = false;
            badgeTutoRead = false;
            eventTeaserStarted = false;
            eventTeaserLocked = false;
            eventMainStarted = false;
            pokemonScore = 0;
            axeScore = 0;
        }
    }

    public static event Action DataSaved, DataInitialized;

    public static SaveData Data { get; private set; }
    public static bool Initialized;

    public static string FileDataPath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, SAVE_FILENAME);
        }
    }

    public const string SAVE_FILENAME = "game_save.bin";
    
    private void Awake()
    {
        Initialized = false;
    }

    public static void UnlockEventTeaser()
    {
        Data.eventTeaserStarted = true;
        SaveToFile();
    }

    public static void UnlockEventMain ()
    {
        Data.eventMainStarted = true;
        Data.eventTeaserLocked = true;
        SaveToFile();
    }

    static bool LoadLocalData()
    {
        if (File.Exists(FileDataPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(FileDataPath, FileMode.Open);
            Data = (SaveData)bf.Deserialize(file);
            file.Close();
            return true;
        }
        return false;
    }

    public static void ResetSaveData()
    {
        Data = new SaveData();
        int vumarksCount = Interface_Manager.Instance.GetAppTargetsCount();
        for (int i = 0; i < vumarksCount; i++)
        {
            Data.quizzAnswered.Add(false);
            Data.artefactsUnlocked.Add(false);
        }
        for (int j = 0; j < Interface_Manager.Instance.GetBadgesNumber(); j++)
        {
            Data.badgesUnlocked.Add(false);
        }
        //TODO: Reset game data values here
    }

    public static void RetrieveSaveData()
    {
        bool localExists = LoadLocalData();
        if (!localExists)
        {
            ResetSaveData();
            Initialized = true;
            if (DataInitialized != null)
            {
                DataInitialized.Invoke();
            }
        }
        //if(EventAPI.Instance.SessionUser != null && !string.IsNullOrEmpty(EventAPI.Instance.SessionUser.game_data))
        //{
        //    EventAPI.Instance.GetGameData((long httpCode, byte[] data) =>
        //    {
        //        switch (httpCode)
        //        {
        //            case 200:
        //                SaveData onlineData = GetDataFromBytes(data);
        //                if (!localExists || onlineData.VersionTime > Data.VersionTime || onlineData.UserId != Data.UserId)
        //                {
        //                    Data = onlineData;
        //                }
        //                else if (!localExists)
        //                {
        //                    ResetSaveData();
        //                }
        //                Initialized = true;
        //                DataInitialized?.Invoke();
        //                break;
        //            default:
        //                //TODO: Display network error
        //                break;
        //        }
        //    });
        //}
        //else
        //{
        //    ResetSaveData();
        //    Initialized = true;
        //    DataInitialized?.Invoke();
        //}
    }

    static SaveData GetDataFromBytes(byte[] data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(data);
        return (SaveData)bf.Deserialize(ms);
    }

    public static void SaveToFile()
    {
        Data.VersionTime = DateTime.UtcNow;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(FileDataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        bf.Serialize(fs, Data);
        fs.Close();

        if (DataSaved != null)
        {
            DataSaved.Invoke();
        }

        //Interface_Manager.Instance.UpdateUserEnvironment();

        //EventAPI.Instance.PostGameData((long httpCode, string response) =>
        //{
        //    if(httpCode == 200)
        //    {
        //        DataSaved?.Invoke();
        //    }
        //    else
        //    {
        //        //TODO: Display network error
        //    }
        //});
    }
}