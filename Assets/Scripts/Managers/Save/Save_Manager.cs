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
    public List<bool> galleryStoryStates;
    public List<bool> storyAlreadyDone;
    public bool quizzDoneToSave;
    public bool tutoARCameraHasBeenActivated;
    public int scoreToSave;
    public List<int> idxCrateList;

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
                quizzDoneToSave = false;
                tutoARCameraHasBeenActivated = true;
                galleryStoryStates.Add(false);
                storyAlreadyDone.Add(false);
                scoreToSave = 0;
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

    //SAVE QUIZZ 

    public void TutoIsDone(List<bool> tutoIdx)
    {
        galleryTutoStates = tutoIdx;
        Save();
    }

    public void TutoQuizzIsDone(bool quizzTutoIsDone)
    {
        quizzDoneToSave = quizzTutoIsDone;
        Save();
    }

    //SAVE STORY

    public void StoryGalleryIsDone(int galleryStoryIsCompleted)
    {
        galleryStoryStates[galleryStoryIsCompleted] = true;
        Save();
    }

    public void StoryIsDone(List<bool> storyIsOk)
    {
        storyAlreadyDone = storyIsOk;
        Save();
    }

    //SAVE ARCAMERA TUTO

    public void ARCameraTuto(bool galleryTutoARCamera)
    {
        tutoARCameraHasBeenActivated = galleryTutoARCamera;
        Save();
    }

    //SAVE SCORE VUMARK

    public void SavingScore(int scoreIdx)
    {
        scoreToSave = scoreIdx;
        Save();
    }

    //SAVE CHEST STATE

    public void SavingCrateState(List<int> idxCrateState)
    {
        idxCrateList = idxCrateState;
        Save();
    }


    void FixedUpdate ()
    {
        if (Input.GetKeyDown("d"))
        {
            Debug.Log("File Deleted");
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
        data.quizzDoneToSave = quizzDoneToSave;
        data.galleryStoryStates = galleryStoryStates;
        data.tutoARCameraHasBeenActivated = tutoARCameraHasBeenActivated;
        data.storyAlreadyDone = storyAlreadyDone;
        data.scoreToSave = scoreToSave;
        data.idxCrateList = idxCrateList;


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
            quizzDoneToSave = data.quizzDoneToSave;
            galleryStoryStates = data.galleryStoryStates;
            tutoARCameraHasBeenActivated = data.tutoARCameraHasBeenActivated;
            storyAlreadyDone = data.storyAlreadyDone;
            scoreToSave = data.scoreToSave;
            idxCrateList = data.idxCrateList;


            Interface_Manager.Instance.ButtonState(galleryButtonsStates);
            Interface_Manager.Instance.ImageState(mappingImageStates);
            Interface_Manager.Instance.TutoIsDone(quizzDoneToSave);
            Tuto_Manager.tuto.TutoState(galleryTutoStates);
            Tuto_Manager.tuto.LoadMenuTuto(galleryTutoStates);
            Tuto_Manager.tuto.LoadBoolForTuto(tutoARCameraHasBeenActivated);
            Story_Manager.story.LoadStoryStates(galleryStoryStates);
            Story_Manager.story.LoadStoryHasBeenDone(storyAlreadyDone);
            Interface_Manager.Instance.LoadScore(scoreToSave);
            Interface_Manager.Instance.LoadCrateImage(idxCrateList);
        }
    }
}

[Serializable]
class PlayerData
{
    public List<bool> galleryButtonsStates = new List<bool>();
    public List<bool> mappingImageStates = new List<bool>();
    public List<bool> galleryTutoStates = new List<bool>();
    public List<bool> galleryStoryStates = new List<bool>();
    public List<bool> storyAlreadyDone = new List<bool>();
    public bool quizzDoneToSave;
    public bool tutoARCameraHasBeenActivated;
    public int scoreToSave;
    public List<int> idxCrateList = new List<int>();
}
