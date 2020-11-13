using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp;
    public string tmphp;
    public GameObject bullet;
    public bool shoot = true;
    public float delay;
    float timer = 0;

    TextAsset m_textasset = null;

    public GameObject pausepanel;

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

    void Update()
    {
        ShootControl();
    }
    
    void Makepara()
    {
        XmlDocument blt = new XmlDocument();
#if UNITY_EDITOR
        blt.Load("./Assets/Resources/player.xml");
#endif

#if UNITY_ANDROID
        m_textasset = (TextAsset)Resources.Load("player", typeof(TextAsset));
        blt.LoadXml(m_textasset.text);
#endif
        XmlNodeList blist = blt.SelectNodes("rows/row");

        foreach (XmlNode bnode in blist)
        {
            tmphp = bnode.SelectSingleNode("hp").InnerText;
        }
        hp = Convert.ToInt32(tmphp);
    }


    void OnMouseDrag()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x,
        Input.mousePosition.y);
        Vector2 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = objPosition;

        //밖으로 빠져나오지못하게

        Vector2 viewPos = Camera.main.WorldToViewportPoint(transform.position); 
        viewPos.x = Mathf.Clamp01(viewPos.x); 
        viewPos.y = Mathf.Clamp01(viewPos.y); //한계거리


        Vector2 worldPos = Camera.main.ViewportToWorldPoint(viewPos); 
        transform.position = worldPos; //업데이트

    }

    void OnTriggerEnter2D(Collider2D ent)
    {
        pausepanel = GameObject.Find("Canvas").transform.Find("stop").gameObject;
        if (ent.gameObject.tag.Equals("enermy"))
        {
            hp--;
            if(hp <=0)
            {
                Destroy(this.gameObject);
                Time.timeScale = 0.0f;
                pausepanel.SetActive(true);
            } else
            {
                StartCoroutine("Hit");
            }
        }
    }

    IEnumerator Hit()
    {
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 90);
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 90);
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);

    }

    void ShootControl()
    {
        if(shoot)
        {
            if (timer > delay && Input.GetMouseButton(0))
            {
                Instantiate(bullet, transform.position, Quaternion.identity);
                timer = 0;
            }
            timer += Time.deltaTime;
        }
    }

}
