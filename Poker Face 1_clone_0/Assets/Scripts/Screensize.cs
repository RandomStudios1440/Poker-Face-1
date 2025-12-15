using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.PlasticSCM.Editor.WebApi;

public class Screensize : MonoBehaviour
{
    public Dropdown Resolutiondropdown;

    Resolution[] resolutions;
    public void Start()
    {
        resolutions = Screen.resolutions;

        Resolutiondropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

            Resolutiondropdown.AddOptions(options);
            Resolutiondropdown.value = currentResolutionIndex;
            Resolutiondropdown.RefreshShownValue();
        

        Resolutiondropdown.AddOptions(options);

    }

    public void setResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
