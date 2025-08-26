using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Viroo.Interactions.XRInteractionToolkit;
using Wave.Native;

public class MR_Passthrough : MonoBehaviour
{
    public bool inMR = false;

    private Camera cam;
    private Color savedColor;
    private CameraClearFlags savedClearFlags;

    // Toggle between modes
    public void ToggleMixedReality()
    {
        if (inMR)
        {
            SetFullImmersiveVR();
        }
        else
        {
            SetMixedReality();
        }
    }

    public void SetMixedReality()
    {
        ShowPassthroughUnderlay();
    }

    public void SetFullImmersiveVR()
    {
        HidePassthroughUnderlay();
    }

    // Unity Start Event Initialization
    IEnumerator Start()
    {
        cam = Camera.main;
        savedColor = cam.backgroundColor;
        savedClearFlags = cam.clearFlags;

        yield return null;

        if (inMR)
            ShowPassthroughUnderlay();
    }

    // internal private implementations

    void ShowPassthroughUnderlay()
    {
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.white * 0;

        // HTC provided methods to handle Passthrough functionality
        Interop.WVR_SetPassthroughImageQuality(WVR_PassthroughImageQuality.PerformanceMode);
        Interop.WVR_ShowPassthroughUnderlay(true);

        HideInMR[] hideInMRs = GameObject.FindObjectsOfType<HideInMR>(true);
        foreach (var hideInMR in hideInMRs)
        {
            hideInMR.gameObject.SetActive(false);
        }

        inMR = true;
    }

    void HidePassthroughUnderlay()
    {
        cam.clearFlags = savedClearFlags;
        cam.backgroundColor = savedColor;

        Interop.WVR_ShowPassthroughUnderlay(false); //Hide Passthrough Underlay

        HideInMR[] hideInMRs = GameObject.FindObjectsOfType<HideInMR>(true);
        foreach (var hideInMR in hideInMRs)
        {
            hideInMR.gameObject.SetActive(true);
        }
        inMR = false;
    }
}

