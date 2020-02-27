using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public Player playerScript;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI levelUI;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        string score = (playerScript.GetScore()).ToString();

        scoreUI.SetText("Score " + score);
        
    }
}
