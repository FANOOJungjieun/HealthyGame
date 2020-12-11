using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy2 : MonoBehaviour
{
    public int hp = 2;
    public float mvspd = 20;

    // Start is called before the first frame update

    void mov()
    {
        float dy = 3*(mvspd) * Time.deltaTime; //시간맞추기
        this.gameObject.transform.Translate(0, -1 * dy, 0); // 3차원좌표이니 z=0
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mov();   
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D ent)
    {
        if (ent.gameObject.tag.Equals("bullet"))
        {
            hp -= GameObject.Find("bullet(Clone)").GetComponent<bullet>().power;

            if (hp <= 0)
            {
                Destroy(this.gameObject);
                gameManager.instance.Addnum(50);
            }
        }
    }
}
