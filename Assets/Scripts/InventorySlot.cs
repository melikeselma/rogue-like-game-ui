using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI labeltext;
    public TextMeshProUGUI raritytext;


    public void ClearSlot()
    {
        icon.enabled = false;
        labeltext.enabled = false;
        raritytext.enabled = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
