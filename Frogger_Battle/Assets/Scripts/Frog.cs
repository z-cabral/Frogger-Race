using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Frog : MonoBehaviour
{
    int _lives;
    public int Lives
    {
        get => _lives;
        set
        {
            _lives = value;
            ScoreGUI.text = "Lives: " + Lives.ToString();
        }
    }

    [SerializeField] TextMeshProUGUI ScoreGUI;

    private void Start()
    {
        Lives = 3;
    }
}
