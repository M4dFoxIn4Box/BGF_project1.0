using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable_Vumark_GameType", menuName = "Vumark/Vumark Type", order = 1)]

public class Scriptable_Game_Type : ScriptableObject {

    
    enum GameType
    {
        Quizz,
        MiniGame,
        Scan,
    }

	public void Start ()
    {
        
    }
	
    void MiniGame ()
    {
    
    }

    void Scan ()
    {

    }
}
