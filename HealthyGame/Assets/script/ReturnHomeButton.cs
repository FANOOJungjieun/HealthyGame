using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnHomeButton : MonoBehaviour
{
    private bool pausecheck = false;
    private GameObject normalpanal;
    private GameObject pausepanal;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "02.game")
        {
            normalpanal = GameObject.Find("Canvas").transform.Find("normal").gameObject;
            pausepanal = GameObject.Find("Canvas").transform.Find("stop").gameObject;
            pausepanal.SetActive(false);
        }
    }

    // Start is called before the first frame update
    public void ChangeToHome()
    {
        SceneManager.LoadScene("01.start");
    }

    public void retrybutton()
    {
        Time.timeScale = 1.0f;
        //Application.LoadLevel("02.game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        pausepanal.SetActive(false);
    }

    public void pauseButton()
    {
        if (!pausecheck)
        {
            Time.timeScale = 0;
            pausepanal.SetActive(true);
            //normalpanal.SetActive(false);
        } else
        {
            Time.timeScale = 1.0f;
            pausepanal.SetActive(false);
            //normalpanal.SetActive(true);
        }

        pausecheck = !pausecheck;
    }


}
