//this script is for things that won't exist in play mode or in build
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Experimental.Rendering.Universal;

[ExecuteInEditMode]
public class EditModeGlobalLight : MonoBehaviour
{
    public Light2D InGameLight2D;
    public Light2D EditModeLight2D;

    private void Update()
    {
        #if UNITY_EDITOR
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            EditModeLight2D.enabled = false;
            InGameLight2D.enabled = true;
        }
        else
        {
            InGameLight2D.enabled = false;
            EditModeLight2D.enabled = true;
        }
        #endif
        #if !UNITY_EDITOR
        Destroy(this.gameObject);
        #endif
    }
}
