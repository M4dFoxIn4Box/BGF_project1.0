﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Save_Manager : MonoBehaviour
{
    //public static Save_Manager saving;

    public static Save_Manager saving { get; private set; }

    private int buttonIdx;
    private List<bool> galleryButtonsStates;

    private void Awake()
    {
        if(saving == null)
        {
            DontDestroyOnLoad(gameObject);
            saving = this;
        }
        else if (saving != this)
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        if (buttonIdx == 0)
        {
            for (int i = 0; i < 42; i++)
            {
                galleryButtonsStates.Add(false);
                Save();
            }
        }
        else
        {
            Load();
        }
    }

    public void SetToTrue (int buttonIdx)
    {
        galleryButtonsStates[buttonIdx] = true;
        Save();
    }

    void Update ()
    {
		
	}

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.data");

        PlayerData data = new PlayerData();
        data.galleryButtonsStates = galleryButtonsStates;
        data.buttonIdx = buttonIdx;

        bf.Serialize(file, data);
        file.Close();

    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.data"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.data", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            galleryButtonsStates = data.galleryButtonsStates;
            Interface_Manager.Instance.ButtonState(galleryButtonsStates);
        }
    }
}

[Serializable]
class PlayerData
{
    public List<bool> galleryButtonsStates = new List<bool>();
    public int buttonIdx;
}
