using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable_Reward", menuName = "Scriptable/Reward", order = 1)]

public class ScriptableReward : ScriptableObject {

    public GameObject rewardToSpawn;
    public string funFactToDisplay;
}
