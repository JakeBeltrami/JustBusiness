using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.ImageEffects;

public class PostProcessing : MonoBehaviour
{
    private PostProcessLayer postProcessLayer;
    private PostProcessVolume postProcessVolume;
    private PostProcessProfile sharedProfile;
    private ScreenOverlay screenOverlay;

    // Start is called before the first frame update
    void Awake()
    {
        postProcessLayer = Camera.main.GetComponent<PostProcessLayer>();
        postProcessVolume = GameObject.FindGameObjectWithTag("Post Processing").GetComponent<PostProcessVolume>();
        sharedProfile = postProcessVolume.profile;
        screenOverlay = Camera.main.GetComponent<ScreenOverlay>();
    }

    // For Testing
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Mouse0))
    //     {
    //         EnableVignette();
    //     }
    //     if (Input.GetKeyDown(KeyCode.Mouse1))
    //     {
    //         DisableVignette();
    //     }
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         DisablePost();
    //     }
    // }

    public void DisablePost()
    {
        // postProcessLayer.enabled = false;
        // RuntimeUtilities.DestroyVolume(postProcessVolume, false, false);
        // Disable Color Grading
        sharedProfile.RemoveSettings<ColorGrading>();
        // Disable Vignette
        sharedProfile.GetSetting<Vignette>().enabled.Override(false);
        screenOverlay.enabled = false;
    }

    // Change to enable settings if wanting to start with them off
    public void EnablePost()
    {
        postProcessLayer.enabled = true;
        screenOverlay.enabled = true;
    }

    public void EnableVignette()
    {
        sharedProfile.GetSetting<Vignette>().enabled.Override(true);
    }

    public void DisableVignette()
    {
        sharedProfile.GetSetting<Vignette>().enabled.Override(false);
    }
}
