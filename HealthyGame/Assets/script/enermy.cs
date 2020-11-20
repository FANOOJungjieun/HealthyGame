using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enermy : MonoBehaviour
{
    public int hp = 1;
    public float mvspd = 10;
    // Start is called before the first frame update

    //목적: 등장위치에서 계속 밑으로이동함.

    void mov()
    {
        float dy = 3 * (mvspd) * Time.deltaTime; //시간맞추기
        this.gameObject.transform.Translate(0, -1 * dy,0); // 3차원좌표이니 z=0
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
        if (ent.gameObject.tag.Equals("player"))
        {
            if(GameObject.Find("bullet(Clone)")) hp -= GameObject.Find("bullet(Clone)").GetComponent<bullet>().power;

            if(hp <= 0)
            {
                Destroy(this.gameObject);
                gameManager.instance.Addnum(10);
            }
        }
    }


}
