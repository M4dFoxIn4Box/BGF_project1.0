using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeState : MonoBehaviour
{
    public bool isTargetToScan = false;
    public bool unlockBonus = false;
    public Image challengeGauge;
    public GameObject challengeStars;
    public Text scoreText;
    private bool isCompleted;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateChallengeGauge (int playerBestScore, int scoreToReach)
    {
        challengeGauge.fillAmount = (playerBestScore * 1f) / (scoreToReach * 1f);
        scoreText.text = playerBestScore.ToString() + " / " + scoreToReach.ToString();
    }

    public void SetChallengeCompleted ()
    {
        challengeGauge.fillAmount = 1f;
        challengeStars.SetActive(true);
        isCompleted = true;
        if (isTargetToScan)
        {
            scoreText.text = "1 / 1";
        }
    }

    public bool IsChallengeCompleted()
    {
        return isCompleted;
    }

    public bool UnlockBonus ()
    {
        return unlockBonus;
    }
}
