using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuiBehavior : MonoBehaviour
{
    public static GuiBehavior Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    public void ToggleGuiVisibility(TextMeshProUGUI gui)
    {
        if (gui.gameObject.activeSelf == true)
            gui.gameObject.SetActive(false);
        else
            gui.gameObject.SetActive(true);
    }

    public void UpdateMessageGUI(string msg, TextMeshProUGUI gui, bool toggle)
    {
        gui.text = msg;
        if (toggle == true)
        {
            ToggleGuiVisibility(gui);
        }
    }
}
