using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Story_Manager : MonoBehaviour
{
    public static Story_Manager story {get; private set;}

    [Header ("Story Activate")]

    public List<Scriptable_Story> scriptableStoryList;//Toute la liste des scriptables
    public Scriptable_Story currentScriptableStory;//Le current scriptable
    public GameObject interfaceStory;//Interface quand on joue une story
    public Image storyImage;//Image dans la story
    private int currentSlideIdx;//l'index de la slide
    private int storyToDeactive;//désactiver la story
    public GameObject buttonPrecedentSlide;//bouton précedent lors de la lecture de la story

    [Header ("Story Gallery")]

    public Transform storyGallery;//la gallery des story
    private int scriptableIdxStory;//
    public List<bool> storyHasBeenDone;//Si la story à été faite ou non

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
        Debug.Log("Here");
        scriptableIdxStory = storyToActive;
        storyToDeactive = storyToActive;
        currentScriptableStory = scriptableStoryList[storyToActive];
        interfaceStory.SetActive(true);
        storyImage.sprite = currentScriptableStory.scriptableStoryImageList[currentSlideIdx];
    }

    public void ActivatingStory(int storyToActive)
    {
        Debug.Log("Here");
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
        Debug.Log("Here");
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
        Debug.Log("Here");
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

    public void ActivateStoryInGallery(int storyToActivateInGallery)
    {
        Debug.Log("Here");
        scriptableIdxStory = storyToActivateInGallery;
        if (storyHasBeenDone[storyToActivateInGallery] == false)
        {
            Save_Manager.saving.StoryIsDone(storyHasBeenDone);
            ActivateButtonGallery();
        }
    }

    //SAVE AND LOAD STORY

    //GALLERY

    public void ActivateButtonGallery()
    {
        Debug.Log("Here");
        if (storyGallery.GetChild(scriptableIdxStory).gameObject.GetComponent<Button>().interactable == false)
        {
            storyGallery.GetChild(scriptableIdxStory).gameObject.GetComponent<Button>().interactable = true;
            Save_Manager.saving.StoryGalleryIsDone(scriptableIdxStory);
        }
    }

    public void LoadStoryStates(List<bool> storyBoolList)
    {
        Debug.Log("Here");
        for (int i = 0; i < scriptableStoryList.Count; i++)
        {
            storyGallery.GetChild(i).gameObject.GetComponent<Button>().interactable = storyBoolList[i];
        }
    }

    //STORY STATE

    public void LoadStoryHasBeenDone(List<bool>  isStoryCheck)
    {
        Debug.Log("Here");
        for (int i = 0; i < storyHasBeenDone.Count; i++)
        {
            storyHasBeenDone[i] = isStoryCheck[i];
        }
    }

}
