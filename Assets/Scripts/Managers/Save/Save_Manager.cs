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

    //MAIN GALLERY
    public List<bool> galleryButtonsStates;

    //MAP
    public List<bool> mappingImageStates;

    //TUTO
    public List<bool> galleryTutoStates;

    //STORY
    public List<bool> galleryStoryStates;
    public List<bool> storyAlreadyDone;

    //CHEST MAIN MENU
    public List<int> idxCrateList;

    //QUIZZ
    public bool quizzDoneToSave;

    //AR CAMERA
    public bool tutoARCameraHasBeenActivated;

    //SCORE
    public int scoreToSave;


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

    public void ImageToTrue(int imageIdx) // Sauvegarder l'image de la map qui s'est activé
    {
        mappingImageStates[imageIdx] = true;
        Save();
    }

    //SAVE QUIZZ 

    public void TutoIsDone(List<bool> tutoIdx) // Sauvegarder si le tuto à déjà été fait 
    {
        galleryTutoStates = tutoIdx;
        Save();
    }

    public void TutoQuizzIsDone(bool quizzTutoIsDone) // Sauvegarder si le quizz à déjà été fait
    {
        quizzDoneToSave = quizzTutoIsDone;
        Save();
    }

    //SAVE STORY

    public void StoryGalleryIsDone(int galleryStoryIsCompleted) // Sauvegarder le bouton de la galerie de la story
    {
        galleryStoryStates[galleryStoryIsCompleted] = true;
        Save();
    }

    public void StoryIsDone(List<bool> storyIsOk) //Sauvegarder quand la story à été validé
    {
        storyAlreadyDone = storyIsOk;
        Save();
    }

    //SAVE ARCAMERA TUTO

    public void ARCameraTuto(bool galleryTutoARCamera) //Sauvegarder le tuto de la cam
    {
        tutoARCameraHasBeenActivated = galleryTutoARCamera;
        Save();
    }

    //SAVE SCORE VUMARK

    public void SavingScore(int scoreIdx) // Sauvegarder le score
    {
        scoreToSave = scoreIdx;
        Save();
    }

    //SAVE CHEST STATE

    public void SavingCrateState(List<int> idxCrateState) //Sauvegarder l'état du coffre dans le main menu
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

            if(idxCrateList != null)
            {
                Interface_Manager.Instance.LoadCrateImage(idxCrateList);
            }
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
