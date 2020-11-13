using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using UnityEngine.SceneManagement;

public class bullet : MonoBehaviour
{

    public float speed;
    public int power;
    public string tmpspeed, tmppower;

    TextAsset m_textasset = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void Awake()
    {
        Makepara();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Makepara();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }


    void Makepara()
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

        speed = Convert.ToSingle(tmpspeed);
        power = Convert.ToInt32(tmppower);
    }


    // Update is called once per frame
    void Update()
    {
        float dy = 4* speed * Time.deltaTime;
        transform.Translate(0, dy, 0);
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }


    void OnTriggerEnter2D(Collider2D ent)
    {
        if (ent.gameObject.tag.Equals("enermy"))
        {
            Destroy(this.gameObject);
        }
    }
}
