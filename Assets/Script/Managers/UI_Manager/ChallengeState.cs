using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeState : MonoBehaviour
{

    public Image challengeGauge;
    public GameObject challengeStars;
    private bool isCompleted;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetChallengeCompleted ()
    {
        challengeGauge.fillAmount = 1f;
        challengeStars.SetActive(true);
        isCompleted = true;
    }

    public bool IsChallengeCompleted()
    {
        return isCompleted;
    }
}
