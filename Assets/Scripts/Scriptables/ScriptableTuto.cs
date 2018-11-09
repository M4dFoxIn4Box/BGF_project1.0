using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable_Tuto", menuName = "Scriptable/Tuto", order = 1)]

public class ScriptableTuto : ScriptableObject {

    public string tutoTitle;
    public Sprite[] tutoImageBoard;
    public string[] tutoTextBoard;
    public int numberOfSlides;

}
