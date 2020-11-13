using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static gameManager instance;
    public Text scoretext;
    public Text bscoretext;
    public Text steptext;
    private int score = 0;
    private int best = 0;
    private int step = 0;
    private string KeyString = "BestScore";

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        best = PlayerPrefs.GetInt(KeyString, 0);
        step = PlayerPrefs.GetInt("steps", 0);
        bscoretext.text = "Best Score : " + best;
        if (SceneManager.GetActiveScene().name == "01.start")  steptext.text = "steps : " + step;
    }

    public void Addnum(int scr)
    {
        score += scr;
        scoretext.text = "Score : " + score;

        if(score > best)
        {
            PlayerPrefs.SetInt(KeyString, score);
        }
    }

    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }
}
