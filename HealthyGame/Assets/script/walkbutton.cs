using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using UnityEditor;
using UnityEngine.UI;

public class walkbutton : MonoBehaviour
{
    public static walkbutton instance;
    public string tmppower;
    public string tmpspeed;
    public int power;
    public float speed;


    public string tmphp;
    public int hp;

    public Text texthp;
    public Text textspeed;
    public Text textpower;
    public Text mon;

    public Text cntpower;
    public Text cntspeed;
    public Text cnthp;

    TextAsset m_textasset = null;

    // Start is called before the first frame update
    void Start()
    {
        cnthp.text = "" + PlayerPrefs.GetInt("cnthp", 10);
        cntspeed.text = "" + PlayerPrefs.GetInt("cntspeed", 10);
        cntpower.text = "" + PlayerPrefs.GetInt("cntpower", 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void translate()
    {
        int walknum = PlayerPrefs.GetInt("steps", 0);
        int moneynum = PlayerPrefs.GetInt("money", 0);
        moneynum += walknum;
        PlayerPrefs.SetInt("steps", 0);
        PlayerPrefs.SetInt("money", moneynum);

        mon.text = "사용 가능한 재화 : " + moneynum;
    }

    public void bspeedup()
    {
        int tmp = PlayerPrefs.GetInt("money", 0);
        int cnttmp = PlayerPrefs.GetInt("cntspeed", 10);
        if(tmp >= cnttmp)
        {
            tmp -= cnttmp;
            cnttmp += 20;
            PlayerPrefs.SetInt("money", tmp);
            PlayerPrefs.SetInt("cntspeed", cnttmp);
            mon.text = "사용 가능한 재화 : " + tmp;
            cntspeed.text = "" + cnttmp;
            bulletchange(2);
        }
    }

    public void bdamageup()
    {
        int tmp = PlayerPrefs.GetInt("money", 0);
        int cnttmp = PlayerPrefs.GetInt("cntpower", 10);
        if (tmp >= cnttmp)
        {
            tmp -= cnttmp;
            cnttmp += 20;
            PlayerPrefs.SetInt("money", tmp);
            PlayerPrefs.SetInt("cntpower", cnttmp);
            mon.text = "사용 가능한 재화 : " + tmp;
            cntpower.text = "" + cnttmp;
            bulletchange(1);
        }
    }

    public void hpup()
    {
        int tmp = PlayerPrefs.GetInt("money", 0);
        int cnttmp = PlayerPrefs.GetInt("cnthp", 10);
        if (tmp >= cnttmp)
        {
            tmp -= cnttmp;
            cnttmp += 20;
            PlayerPrefs.SetInt("money", tmp);
            PlayerPrefs.SetInt("cnthp", cnttmp);
            mon.text = "사용 가능한 재화 : " + tmp;
            cnthp.text = "" + cnttmp;
            playerchange();
        }
    }

    void playerchange()
    {
        XmlDocument blt = new XmlDocument();
#if UNITY_EDITOR
        blt.Load("Assets/Resources/player.xml");
#endif

#if UNITY_ANDROID
        m_textasset = (TextAsset)Resources.Load("player", typeof(TextAsset));
        blt.LoadXml(m_textasset.text);
#endif
        XmlElement root = blt.DocumentElement;

        XmlNodeList blist = blt.SelectNodes("rows/row");

        foreach (XmlNode bnode in blist)
        {
            tmphp = bnode.SelectSingleNode("hp").InnerText;
        }

        XmlNode acc = blist[0];

        hp = Convert.ToInt32(tmphp);
        hp++;
        acc.SelectSingleNode("hp").InnerText = Convert.ToString(hp);

        texthp.text = Convert.ToString(hp);

#if UNITY_EDITOR
        blt.Save("Assets/Resources/player.xml");
#endif

#if UNITY_ANDROID
        blt.Save(m_textasset.text);
#endif

    }

    void bulletchange(int i)
    {
        XmlDocument blt = new XmlDocument();
#if UNITY_EDITOR
        blt.Load("Assets/Resources/bullet.xml");
#endif

#if UNITY_ANDROID
        m_textasset = (TextAsset)Resources.Load("bullet", typeof(TextAsset));
        blt.LoadXml(m_textasset.text);
#endif
        XmlElement root = blt.DocumentElement;

        XmlNodeList blist = blt.SelectNodes("rows/row");

        foreach (XmlNode bnode in blist)
        {
            tmpspeed = bnode.SelectSingleNode("speed").InnerText;
             tmppower = bnode.SelectSingleNode("power").InnerText;
        }

        XmlNode acc = blist[0];

        if (i == 1)
        { //총알 대미지 증가
            power = Convert.ToInt32(tmppower);
            power++;
            acc.SelectSingleNode("power").InnerText = Convert.ToString(power);

            textpower.text = Convert.ToString(power);

        } else if (i ==2)
        {
            speed = Convert.ToSingle(tmpspeed);
            speed += 50;
            acc.SelectSingleNode("speed").InnerText = Convert.ToString(speed);

            textspeed.text = Convert.ToString(speed);
        }

#if UNITY_EDITOR
        blt.Save("Assets/Resources/bullet.xml");
#endif

#if UNITY_ANDROID
        blt.Save(m_textasset.text);
#endif
    }

}
