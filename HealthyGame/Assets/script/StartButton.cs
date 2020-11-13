using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void ChangeToGame() {
        SceneManager.LoadScene("02.game");
    }
    public void ChangeToWalk()
    {
        SceneManager.LoadScene("03.walk");
    }
    public void ChangeTo2048()
    {
        SceneManager.LoadScene("04.game2");
    }

}
