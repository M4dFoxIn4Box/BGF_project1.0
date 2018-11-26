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
            Debug.Log(currentIdxStory);
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
        Debug.Log(currentIdxStory);
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
