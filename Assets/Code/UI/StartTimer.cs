using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class StartTimer : MonoBehaviour
{
    public GameGrid gameGrid;
    public TextMeshProUGUI text;


    // Update is called once per frame
    void Update()
    {
        if (!gameGrid.gameStarted)
        {
            int value = (int)gameGrid.gameStartDelay;
            this.text.text = value == 0 ? "Vai :)" : value.ToString();
        }
        else
        {
            text.enabled = false;
        }
    }

}
