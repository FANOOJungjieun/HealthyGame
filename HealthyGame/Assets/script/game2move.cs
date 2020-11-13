using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class game2move : MonoBehaviour
{
    // Update is called once per frame
    bool check, com2;
    int nx2, ny2;

    void Update()
    {
        if(check)
        {
            Move(nx2, ny2, com2);
        }
    }

    public void ChangeToHome()
    {
        SceneManager.LoadScene("01.start");
    }


    public void Move(int x2, int y2, bool com)
    {
        check = true;
        nx2 = x2;
        ny2 = y2;
        com2 = com;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(287f * x2 - 436f, 287f * y2 - 270f, 0), 100f);

        if(transform.position == new Vector3(287f * x2 - 436f, 287f * y2 - 270f, 0))
        {
            check = false;
            if(com)
            {
                com2 = false;
                Destroy(gameObject);
            }
        }
    }
}
