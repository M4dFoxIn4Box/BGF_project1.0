﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Active_Fake_AR : MonoBehaviour {

    public int animationInteractableIdx;

    public void OnEnable()
    {
        Fake_AR_Manager.FakeAR.FakeARToSpawn(animationInteractableIdx);
    }
}
