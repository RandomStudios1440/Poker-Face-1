using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ChipSystem : MonoBehaviour
{
    [SerializeField] ChipSystem shopMenu;
    [SerializeField] TMP_Text chipText;
    private object chips;

    void Update()
    {
        chipText.text = shopMenu.chips.ToString();
    }
    
        
    
}
