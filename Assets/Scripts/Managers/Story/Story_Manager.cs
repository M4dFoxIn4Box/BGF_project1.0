using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Story_Manager : MonoBehaviour
{
    public static Story_Manager story {get; private set;}

    [Header ("Story Gallery")]

    public Transform storyGallery;
    public List<int> idxStory;
    public List<bool> storyList;

    [Header("Story List")]

    public int currentIdxStory = 0;

    public List<Transform> currentStoryActivate = new List<Transform>();

    public Transform currentStory = null;

    public GameObject arrowRight;
    public GameObject arrowLeft;
    public GameObject activeArrows;

    private void Awake()
    {
        if (story == null)
        {
            DontDestroyOnLoad(gameObject);
            story = this;
        }
        else if (story != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        

    }

    //SCRIPTABLE STORY 

    public void ActivatingTuto(int storyToActive)
    {
        idxTuto = storyToActive;
        tutoToDeactive = storyToActive;
        if (tutoHasBeenDone[storyToActive] == false)
        {
            currentScriptableTuto = tutoList[storyToActive];
            tutoTitleTxt.text = currentScriptableTuto.tutoTitle;
            menuTuto.SetActive(true);
            tutoImg.sprite = currentScriptableTuto.tutoImageBoard[currentSlideIdx];
            tutoTxt.text = currentScriptableTuto.tutoTextBoard[currentSlideIdx];
        }
    }

    public void MoveToNextSlide()
    {
        if (currentSlideIdx == currentScriptableTuto.numberOfSlides - 1)
        {
            currentSlideIdx = 0;
            menuTuto.SetActive(false);
            tutoHasBeenDone[tutoToDeactive] = true;
            Save_Manager.saving.TutoIsDone(tutoHasBeenDone);
            buttonPrecedentSlide.SetActive(false);

            if (tutoGallery.GetChild(idxTuto).gameObject.GetComponent<Button>().interactable == false)
            {
                tutoGallery.GetChild(idxTuto).gameObject.GetComponent<Button>().interactable = true;
            }

            if (currentScriptableTuto == tutoList[tutoQuizzIdx] && activateARCamera)
            {
                Interface_Manager.Instance.OpenARCamera();
                activateARCamera = false;
                Save_Manager.saving.ARCameraTuto(activateARCamera);
            }

        }
        else
        {
            buttonPrecedentSlide.SetActive(true);
            currentSlideIdx++;
            tutoImg.sprite = currentScriptableTuto.tutoImageBoard[currentSlideIdx];
            tutoTxt.text = currentScriptableTuto.tutoTextBoard[currentSlideIdx];
        }
    }

    public void MoveToPrecedentSlide()
    {
        if (currentSlideIdx != 0)
        {
            currentSlideIdx--;
            tutoImg.sprite = currentScriptableTuto.tutoImageBoard[currentSlideIdx];
            tutoTxt.text = currentScriptableTuto.tutoTextBoard[currentSlideIdx];
            if (currentSlideIdx == 0)
            {
                buttonPrecedentSlide.SetActive(false);
            }
        }
    }

    public void ShowTuto(Transform storyToShow)
    {
        if (currentStory != null)
        {
            currentStory.gameObject.SetActive(false);
            currentStoryActivate[currentIdxStory].gameObject.SetActive(false);
            currentIdxStory = 0;
            currentStoryActivate[currentIdxStory].gameObject.SetActive(true);
            arrowLeft.SetActive(false);
            arrowRight.SetActive(true);
        }

        currentIdxStory = 0;
        currentStoryActivate.Clear();
        currentStory = storyToShow;
        storyToShow.gameObject.SetActive(true);
        activeArrows.gameObject.SetActive(true);

        for (int i = 0; i < currentStory.childCount; i++)
        {
            currentStoryActivate.Add(currentStory.transform.GetChild(i));
        }
    }

    public void UnShowTuto()
    {
        currentStory.gameObject.SetActive(false);
        activeArrows.gameObject.SetActive(false);
        currentStory = null;
        currentIdxStory = 0;
    }

    public void NextStorySlide()
    { 

        if(currentIdxStory < currentStoryActivate.Count -1)
        {
            currentStoryActivate[currentIdxStory].gameObject.SetActive(false);
            currentIdxStory++;
            currentStoryActivate[currentIdxStory].gameObject.SetActive(true);
            arrowLeft.SetActive(true);
        }

        if (currentIdxStory == currentStoryActivate.Count - 1)
        {
            arrowRight.SetActive(false);
        }
    }

    public void PrecedentStorySlide()
    {
        if (currentIdxStory > 0)
        {
            currentStory.GetChild(currentIdxStory).gameObject.SetActive(false);
            currentIdxStory--;
            currentStory.GetChild(currentIdxStory).gameObject.SetActive(true);
            arrowRight.SetActive(true);
        }

        if (currentIdxStory == 0)
        {
            arrowLeft.SetActive(false);
        }
    }

    public void ActivateButtonGallery(int idxStory)
    {
        if (storyGallery.GetChild(idxStory).gameObject.GetComponent<Button>().interactable == false)
        {
            storyGallery.GetChild(idxStory).gameObject.GetComponent<Button>().interactable = true;
            Save_Manager.saving.StoryIsDone(idxStory);
        }
    }


    public void LoadStoryStates(List<bool> storyBoolList)
    {
        for (int i = 0; i < idxStory.Count; i++)
        {
            storyGallery.GetChild(i).gameObject.GetComponent<Button>().interactable = storyBoolList[i];
        }
    }
}
