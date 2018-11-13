using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Save_Manager : MonoBehaviour
{
    //public static Save_Manager saving;
    public int rewardNumbers;

    public static Save_Manager saving { get; private set; }

    [Header ("Save")]

    public List<bool> galleryButtonsStates;
    public List<bool> mappingImageStates;
    public List<bool> galleryTutoStates;

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


    void Start()// Regarde si le fichier de sauvegarde existe
    {
        if (!File.Exists(Application.persistentDataPath + "/playerInfo.data"))
        {
            for (int i = 0; i < rewardNumbers; i++)
            {
                galleryButtonsStates.Add(false);
                mappingImageStates.Add(false);
                galleryTutoStates.Add(false);
                Save();
            }
        }
        else
        {
            Load();
        }

    }

    public void SetToTrue (int buttonIdx) // Sauvegarder le bouton qui s'est activé
    {
        galleryButtonsStates[buttonIdx] = true;
        Save();
    }

    public void ImageToTrue(int imageIdx) // Sauvegarder le bouton qui s'est activé
    {
        mappingImageStates[imageIdx] = true;
        Save();
    }

    public void TutoIsDone(List<bool> tutoIdx)
    {
        galleryTutoStates = tutoIdx;
        Save();
    }


    void FixedUpdate ()
    {
        if (Input.GetKeyDown("d"))
        {
            ResetClearList();
        }
	}

    public void ResetClearList()
    {
        File.Delete(Application.persistentDataPath + "/playerInfo.data");
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.data");

        PlayerData data = new PlayerData();
        data.galleryButtonsStates = galleryButtonsStates;
        data.mappingImageStates = mappingImageStates;
        data.galleryTutoStates = galleryTutoStates;


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
            mappingImageStates = data.mappingImageStates;
            galleryTutoStates = data.galleryTutoStates;

            Interface_Manager.Instance.ButtonState(galleryButtonsStates);
            Interface_Manager.Instance.ImageState(mappingImageStates);
            Tuto_Manager.tuto.TutoState(galleryTutoStates);

        }
    }
}

[Serializable]
class PlayerData
{
    public List<bool> galleryButtonsStates = new List<bool>();
    public List<bool> mappingImageStates = new List<bool>();
    public List<bool> galleryTutoStates = new List<bool>();
}
