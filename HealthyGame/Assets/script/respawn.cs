using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawn : MonoBehaviour
{
    public bool spawn = false;
    public float timesize = 0;
    public GameObject enemy;
    public GameObject enemy2;

    void rspawn()
    {
        float x = Random.Range(10f, 1000f);

        if(spawn == true)
        {
            GameObject enem = (GameObject)Instantiate(enemy, new Vector2(x, 2500f), Quaternion.identity);
        }
    }

    void rspawn2()
    {
        float x = Random.Range(10f, 1000f);

        if (spawn == true)
        {
            GameObject enem = (GameObject)Instantiate(enemy2, new Vector2(x, 2500f), Quaternion.identity);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("rspawn", 3, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsInvoking()) InvokeRepeating("rspawn", 3, 1);

        timesize += Time.deltaTime;
        if(timesize > 10.0f)
        {
            CancelInvoke("rspawn");
            if(!IsInvoking())
            {
                InvokeRepeating("rspawn2", 3, 1);
            }
        }
    }
}
