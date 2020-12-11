using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class plugin1 : MonoBehaviour
{

    public int walkcount = 0;
    public Text walktext;

    public int data = 0;

    /*plugin */
    //private static AndroidJavaObject plugin = null;
    //private AndroidJavaObject activityContext = null;
    //private AndroidJavaClass activityClass = null;
    //private AndroidJavaClass _plugin = null;
    //private AndroidJavaClass plugin = null;
    /*plugin*/

    

    private float loLim = 0.05F;
    private float hiLim = 0.35F;
    private bool stateH = false;
    private float fHigh = 4.0F;
    private float curAcc = 0F;
    private float fLow = 0.1F;
    private float avgAcc = 0f;

    private string KeyString = "steps";
    public Text money;
    public Text stp;

    public int stepDetector()
    {
        curAcc = Mathf.Lerp(curAcc, Input.acceleration.magnitude, Time.deltaTime * fHigh);
        avgAcc = Mathf.Lerp(avgAcc, Input.acceleration.magnitude, Time.deltaTime * fLow);

        float delta = curAcc - avgAcc;

        if (!stateH)
        {
            if (delta > hiLim)
            {
                stateH = true;
            }
        }
        else
        {
            if (delta < loLim)
            {
                stateH = false;
            }
        }
        avgAcc = curAcc;

        if(stateH)
        {
            return 1;
        } else { return 0; }
    }

    void Start()
    {
        /*plugin*/
        //activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

        //_plugin = new AndroidJavaClass("com.jje.healthygame.MainActivity");
        //plugin = _plugin.CallStatic<AndroidJavaObject>("instance");

        //plugin.Call("setContext", activityContext);
        //plugin.Call("onCreate");
        //plugin = new AndroidJavaClass("com.jje.myplugin.PluginClass");
        /*plugin*/

        if (SceneManager.GetActiveScene().name == "03.walk") money.text = "사용 가능한 재화 : " + PlayerPrefs.GetInt("money", 0);
        //avgAcc = Input.acceleration.magnitude;
    }

    void Update()
    {
        //if (plugin != null)
        //{
        //    data = plugin.Call<int>("getSensorvalue");
        //    walkcount += data;
        // }
        walkcount = PlayerPrefs.GetInt("steps", 0);
        walkcount += stepDetector();

        PlayerPrefs.SetInt("steps", walkcount);
        if(SceneManager.GetActiveScene().name == "03.walk") walktext.text = "사용 가능한 걸음 수 : " + PlayerPrefs.GetInt("steps", 0);
        if(SceneManager.GetActiveScene().name == "01.start") stp.text = PlayerPrefs.GetInt("steps", 0).ToString() ;
    }

}
