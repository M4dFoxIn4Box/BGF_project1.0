using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Story_Manager : MonoBehaviour
{
    public static Story_Manager story {get; private set;}

    [Header ("Story Gallery")]

    //public List<int> idxStory;
    //public List<bool> storyList;

    [Header("Story Old Gallery")]

    //public int currentIdxStory = 0;

    //public List<Transform> currentStoryActivate = new List<Transform>();

    //public Transform currentStory = null;

    //public GameObject arrowRight;
    //public GameObject arrowLeft;
    //public GameObject activeArrows;

    [Header ("Story Activate")]

    public Scriptable_Story currentScriptableStory;
    public GameObject interfaceStory;
    //public Text storyTitleText;
    public Image storyImage;
    //public Text storyText;
    private int currentSlideIdx;
    public List<bool> storyHasBeenDone;
    public List<Scriptable_Story> scriptableStoryList;
    private int storyToDeactive;
    private int scriptableIdxStory;
    public Transform storyGallery;
    public GameObject buttonPrecedentSlide;

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

    public void ActivatingStoryGallery(int storyToActive)
    {
        scriptableIdxStory = storyToActive;
        storyToDeactive = storyToActive;
        currentScriptableStory = scriptableStoryList[storyToActive];
        interfaceStory.SetActive(true);
        storyImage.sprite = currentScriptableStory.scriptableStoryImageList[currentSlideIdx];
    }

    public void ActivatingStory(int storyToActive)
    {
        scriptableIdxStory = storyToActive;
        storyToDeactive = storyToActive;
        if (storyHasBeenDone[storyToActive] == false)
        {
            currentScriptableStory = scriptableStoryList[storyToActive];
            interfaceStory.SetActive(true);
            storyImage.sprite = currentScriptableStory.scriptableStoryImageList[currentSlideIdx];
        }
    }

    public void MoveToNextSlide()
    {
        if (currentSlideIdx == currentScriptableStory.numberOfSlides - 1)
        {
            currentSlideIdx = 0;
            interfaceStory.SetActive(false);
            storyHasBeenDone[storyToDeactive] = true;
            Save_Manager.saving.StoryIsDone(storyHasBeenDone);
            buttonPrecedentSlide.SetActive(false);
            ActivateButtonGallery();
        }
        else
        {
            buttonPrecedentSlide.SetActive(true);
            currentSlideIdx++;
            storyImage.sprite = currentScriptableStory.scriptableStoryImageList[currentSlideIdx];
        }
    }

    public void MoveToPrecedentSlide()
    {
        if (currentSlideIdx != 0)
        {
            currentSlideIdx--;
            storyImage.sprite = currentScriptableStory.scriptableStoryImageList[currentSlideIdx];
            if (currentSlideIdx == 0)
            {
                buttonPrecedentSlide.SetActive(false);
            }
        }
    }

    //public void ShowTuto(Transform storyToShow)
    //{
    //    if (currentStory != null)
    //    {
    //        currentStory.gameObject.SetActive(false);
    //        currentStoryActivate[currentIdxStory].gameObject.SetActive(false);
    //        currentIdxStory = 0;
    //        currentStoryActivate[currentIdxStory].gameObject.SetActive(true);
    //        arrowLeft.SetActive(false);
    //        arrowRight.SetActive(true);
    //    }

    //    currentIdxStory = 0;
    //    currentStoryActivate.Clear();
    //    currentStory = storyToShow;
    //    storyToShow.gameObject.SetActive(true);
    //    activeArrows.gameObject.SetActive(true);

    //    for (int i = 0; i < currentStory.childCount; i++)
    //    {
    //        currentStoryActivate.Add(currentStory.transform.GetChild(i));
    //    }
    //}

    //public void UnShowTuto()
    //{
    //    currentStory.gameObject.SetActive(false);
    //    activeArrows.gameObject.SetActive(false);
    //    currentStory = null;
    //    currentIdxStory = 0;
    //}

    //public void NextStorySlide()
    //{ 

    //    if(currentIdxStory < currentStoryActivate.Count -1)
    //    {
    //        currentStoryActivate[currentIdxStory].gameObject.SetActive(false);
    //        currentIdxStory++;
    //        currentStoryActivate[currentIdxStory].gameObject.SetActive(true);
    //        arrowLeft.SetActive(true);
    //    }

    //    if (currentIdxStory == currentStoryActivate.Count - 1)
    //    {
    //        arrowRight.SetActive(false);
    //    }
    //}

    //public void PrecedentStorySlide()
    //{
    //    if (currentIdxStory > 0)
    //    {
    //        currentStory.GetChild(currentIdxStory).gameObject.SetActive(false);
    //        currentIdxStory--;
    //        currentStory.GetChild(currentIdxStory).gameObject.SetActive(true);
    //        arrowRight.SetActive(true);
    //    }

    //    if (currentIdxStory == 0)
    //    {
    //        arrowLeft.SetActive(false);
    //    }
    //}

    //SAVE AND LOAD STORY

    //GALLERY

    public void ActivateButtonGallery()
    {
        if (storyGallery.GetChild(scriptableIdxStory).gameObject.GetComponent<Button>().interactable == false)
        {
            storyGallery.GetChild(scriptableIdxStory).gameObject.GetComponent<Button>().interactable = true;
            Save_Manager.saving.StoryGalleryIsDone(scriptableIdxStory);
        }
    }

    public void LoadStoryStates(List<bool> storyBoolList)
    {
        for (int i = 0; i < scriptableStoryList.Count; i++)
        {
            storyGallery.GetChild(i).gameObject.GetComponent<Button>().interactable = storyBoolList[i];
        }
    }

    //STORY STATE

    public void LoadStoryHasBeenDone(List<bool>  isStoryCheck)
    {
        for (int i = 0; i < storyHasBeenDone.Count; i++)
        {
            storyHasBeenDone[i] = isStoryCheck[i];
        }
    }

}
