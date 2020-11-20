using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;


public class mystat : MonoBehaviour
{
    public Text power;
    public Text speed;
    public Text hp;

    TextAsset m_textasset = null;
    TextAsset m_textasset2 = null;
  
    // Start is called before the first frame update
    void Start()
    {
        sett();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        sett();
    }


    void sett()
    {
        XmlDocument blt = new XmlDocument();
        XmlDocument plr = new XmlDocument();

#if UNITY_EDITOR
        blt.Load("Assets/Resources/bullet.xml");
        plr.Load("Assets/Resources/player.xml");
#endif

#if UNITY_ANDROID
        m_textasset = (TextAsset)Resources.Load("bullet", typeof(TextAsset));
        m_textasset2 = (TextAsset)Resources.Load("player", typeof(TextAsset));
        blt.LoadXml(m_textasset.text);
        plr.LoadXml(m_textasset2.text);
#endif

        XmlNodeList blist = blt.SelectNodes("rows/row");
        XmlNodeList plist = plr.SelectNodes("rows/row");

        foreach (XmlNode bnode in blist)
        {
            speed.text = bnode.SelectSingleNode("speed").InnerText;
            power.text = bnode.SelectSingleNode("power").InnerText;
        }

        foreach (XmlNode pnode in plist)
        {
            hp.text = pnode.SelectSingleNode("hp").InnerText;
        }


    }

}
