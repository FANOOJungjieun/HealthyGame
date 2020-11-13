using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet2 : MonoBehaviour
{

    public float speed = 170f;
    public int power = 2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float dy = speed * Time.deltaTime;
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
