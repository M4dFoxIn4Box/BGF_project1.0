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
    //public List<bool> tutoStates;
    //public GameObject[] rewardToSpawn;
    //public string[] funFactToDisplay;

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
                //tutoStates.Add(false);
                Save();
            }
        }
        else
        {
            Load();
        }

        //Tuto_Manager.Tuto.AwakeTuto(tutoStates);
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

    //public void TutoToDestroy(int tutoIdx)
    //{
    //    tutoStates[tutoIdx] = true;
    //    Save();
    //}

    //public void RewardSave(GameObject[] rewardToSave, string[] funFactToSave)
    //{
    //    rewardToSpawn = rewardToSave;
    //    funFactToDisplay = funFactToSave;
    //    Save();
    //}

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
        Debug.Log("DELETE");
    }

    IEnumerator CHIEN()
    {
        Debug.Log("Loading");
        yield return new WaitForSeconds(2);
        Load();

    }
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.data");

        PlayerData data = new PlayerData();
        data.galleryButtonsStates = galleryButtonsStates;
        data.mappingImageStates = mappingImageStates;
        //data.tutoStates = tutoStates;
        //data.rewardToSpawn = rewardToSpawn;
        //data.funFactToDisplay = funFactToDisplay;

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
            //tutoStates = data.tutoStates;
            //rewardToSpawn = data.rewardToSpawn;
            //funFactToDisplay = data.funFactToDisplay;

            Interface_Manager.Instance.ButtonState(galleryButtonsStates);
            Interface_Manager.Instance.ImageState(mappingImageStates);
            //Tuto_Manager.Tuto.LoadingTuto();
        }
    }
}

[Serializable]
class PlayerData
{
    public List<bool> galleryButtonsStates = new List<bool>();
    public List<bool> mappingImageStates = new List<bool>();
    //public List<bool> tutoStates = new List<bool>();
    //public GameObject[] rewardToSpawn;
    //public string[] funFactToDisplay;
}
