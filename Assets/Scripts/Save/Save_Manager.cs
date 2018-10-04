using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Save_Manager : MonoBehaviour
{
    List<bool> galleryButtonsStates;
    public static Save_Manager saving;

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


    void Start ()
    {
       /* if(galleryButtonsStates < 42)
        {
            List<bool> galleryButtonsStates;
            for (int i = 0; i < 42; i++)
            {
                galleryButtonsStates.Add(false);
                Save();
            }
        }*/

        
	}

	void Update () {
		
	}

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.data", FileMode.Open);

        SaveData data = new SaveData();

    }
}

[Serializable]
class SaveData
{
    public List<bool> galleryButtonsStates = new List<bool>();
}
