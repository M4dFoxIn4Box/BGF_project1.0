using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMatRenderMode : MonoBehaviour
{
    public Material matToModify;
    //public float vanishingSpeed = 1f;
    //private bool vanish = false;
    //private bool isVanishing = false;
    public float alphaValue = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (vanish && isVanishing)
        //{
        //    alphaValue -= vanishingSpeed * Time.deltaTime;
        //    if (alphaValue < 0)
        //    {
        //        alphaValue = 0;
        //        isVanishing = false;
        //    }
        //}
        //else if (!vanish && isVanishing)
        //{
        //    alphaValue += vanishingSpeed * Time.deltaTime;
        //    if (alphaValue > 1)
        //    {
        //        alphaValue = 1;
        //        SwitchToOpaque();
        //        isVanishing = false;
        //    }
        //}
    }

    public void SwitchToFadeAndVanish()
    {
        matToModify.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        matToModify.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        matToModify.SetInt("_ZWrite", 0);
        matToModify.DisableKeyword("_ALPHATEST_ON");
        matToModify.EnableKeyword("_ALPHABLEND_ON");
        matToModify.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        matToModify.renderQueue = 3000;
        //vanish = true;
        //isVanishing = true;
    }

    //public void Reveal ()
    //{
        //vanish = false;
        //isVanishing = false;
    //}

    public void SwitchToOpaque()
    {
        matToModify.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        matToModify.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        matToModify.SetInt("_ZWrite", 1);
        matToModify.DisableKeyword("_ALPHATEST_ON");
        matToModify.DisableKeyword("_ALPHABLEND_ON");
        matToModify.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        matToModify.renderQueue = -1;
    }
}
