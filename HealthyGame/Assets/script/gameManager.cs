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
    public Text steptext, Score2048, scorematch3;
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
        bscoretext.text = best.ToString();
        if (SceneManager.GetActiveScene().name == "01.start") {
            steptext.text = step.ToString();
            scorematch3.text = PlayerPrefs.GetInt("BestMatch3", 0).ToString();
        }

    }

    public void Addnum(int scr)
    {
        score += scr;
        scoretext.text = score.ToString();

        if(score > best)
        {
            PlayerPrefs.SetInt(KeyString, score);
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "01.start") Score2048.text = PlayerPrefs.GetInt("BestScore2048").ToString();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
