using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Screensize : MonoBehaviour
{
    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
