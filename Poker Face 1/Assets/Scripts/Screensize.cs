using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Screensize : MonoBehaviour
{
    public Dropdown resolutionDropdown;

    Resolution[] resolutions;
    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height; 
            options.Add(option);
        }
    }
    public void SetFullScreen (bool isFullscreen)

    {
        Screen.fullScreen = isFullscreen;
    }
}
