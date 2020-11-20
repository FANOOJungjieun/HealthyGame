
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class game2 : MonoBehaviour
{
    public GameObject[] n;
    GameObject[,] mat = new GameObject[4, 4];
    public GameObject end;
    public Text Score, Bscore, Plus, Money;

    Vector3 startpos, nextpos;
    int x, y, i, j, k, l, scorenum;
    bool check, mov, dead, moneycount;
    public bool destroy;

    Vector2 touchPos;
    RaycastHit2D hitInformation;


    // Start is called before the first frame update
    void Start()
    {
        TileSpawn();
        TileSpawn(); //타일 2개 등장
        Bscore.text = PlayerPrefs.GetInt("BestScore2048").ToString();
        Money.text = "Money : " + PlayerPrefs.GetInt("money").ToString();
        destroy = false;
        moneycount = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit(); // 어플리케이션 종료

        if (dead) return;
        if((Input.touchCount==1 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            check = true;

            if(Input.GetMouseButtonDown(0))
            {
                startpos = Input.mousePosition;
            } else
            {
                startpos = (Vector3)Input.GetTouch(0).position;
            }

            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hitInformation = Physics2D.Raycast(touchPos, Vector2.zero, 0f); //카메라를 통해 위치지정

        } // or뒤는 pc용. touchcount = 터치중인 손가락의 수

        if (destroy)
        {
            int walkcount = PlayerPrefs.GetInt("money", 0);

            if(walkcount >= 1)
            {
                if (moneycount)
                {
                    walkcount = walkcount - 1;
                    PlayerPrefs.SetInt("money", walkcount);
                    moneycount = false;
                }
                Money.text = "Money : " + PlayerPrefs.GetInt("money").ToString();

                if (hitInformation.collider != null)
                {
                    //Debug.Log(hitInformation.collider.name);
                    GameObject touchedObject = hitInformation.transform.gameObject;
                    Destroy(touchedObject);
                    destroy = false;
                }
            }
        }

        if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0))
        {
            if(Input.GetMouseButton(0))
            {
                nextpos = Input.mousePosition - startpos;
            } else
            {
                nextpos = (Vector3)Input.GetTouch(0).position - startpos;
            }

            if (nextpos.magnitude < 100) return;
            nextpos.Normalize();

            if(check) //반복실행 제어하기
            {
                check = false;
                if (nextpos.x > -0.5f && nextpos.x < 0.5f && nextpos.y > 0) //up
                {
                    for(x = 0; x<=3; x++)
                    {
                        for (y =0; y<=2; y++)
                        {
                            for (i = 3; i >= y + 1; i--) Combine(x, i - 1, x, i);
                        }
                    }
                }
                else if (nextpos.x > -0.5f && nextpos.x < 0.5f && nextpos.y < 0) //down
                {
                    for(x=0; x<=3; x++)
                    {
                        for (y = 3; y>=1; y--)
                        {
                            for (i = 0; i <= y - 1; i++) Combine(x, i + 1, x, i);
                        }
                    }
                }
                else if (nextpos.y > -0.5f && nextpos.y < 0.5f && nextpos.x > 0) //right
                {
                    for (y = 0; y <= 3; y++)
                    {
                        for (x = 0; x <= 2; x++)
                        {
                            for (i = 3; i >= x + 1; i--) Combine(i - 1, y, i, y);
                        }
                    }

                }
                else if (nextpos.y > -0.5f && nextpos.y < 0.5f && nextpos.x < 0)
                {
                    for(y=0; y<=3; y++)
                    {
                        for(x=3; x>=1; x--)
                        {
                            for(i=0; i<= x-1; i++) Combine(i + 1, y, i, y);

                        }
                    }
                }
                else return;

                if(mov)
                {
                    mov = false;
                    TileSpawn();
                    k = 0; l = 0;

                    if(scorenum > 0)
                    {
                        Plus.text = "+" + scorenum.ToString() + "    ";
                        Plus.GetComponent<Animator>().SetTrigger("PlusBack");
                        Plus.GetComponent<Animator>().SetTrigger("Plus");
                        Score.text = (int.Parse(Score.text) + scorenum).ToString();
                        if (PlayerPrefs.GetInt("BestScore2048", 0) < int.Parse(Score.text)) PlayerPrefs.SetInt("BestScore2048", int.Parse(Score.text));
                        Bscore.text = PlayerPrefs.GetInt("BestScore2048").ToString();
                        scorenum = 0;
                    }
                    for(x = 0; x < 4; x++)
                    {
                        for (y = 0; y<4; y++)
                        {
                            if (mat[x, y] == null)
                            {
                                k++; // 빈 타일 수 
                                continue;
                            }
                            if (mat[x, y].tag == "Combine") mat[x, y].tag = "Untagged";
                        }
                    }

                    if(k == 0)
                    {
                        for(y=0; y<4; y++)
                        {
                            for( x=0; x<3; x++)
                            {
                                if (mat[x, y].name == mat[x + 1, y].name) l++;
                            }
                        }

                        for (x = 0; x < 4; x++)
                        {
                            for (y = 0; y < 3; y++)
                            {
                                if (mat[x, y].name == mat[x, y+1].name) l++;
                            }
                        }


                        if (l == 0)
                        {
                            dead = true;
                            end.SetActive(true);
                            return;
                        }
                    }
                }
            }
        } // or뒤는 pc용. touchcount = 터치중인 손가락의 수

    }
    public void Retry() // 재시작
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ItemButton()
    {
        destroy = true;
        moneycount = true;
    }

    void TileSpawn()
    {
        while(true)
        {
            x = Random.Range(0, 4);
            y = Random.Range(0, 4);

            if (mat[x, y] == null) break;
        }
        mat[x, y] = Instantiate(Random.Range(0, 8) > 0 ? n[0] : n[1], new Vector3(287f * x - 436f, 287f * y - 270f, 0), Quaternion.identity);
        //mat[x, y].GetComponent<Animator>().SetTrigger("Spawn"); // 애니메이션 삽입

    }

    void Combine(int x1, int y1, int x2, int y2)
    {
        if(mat[x2,y2] == null && mat[x1,y1] != null)
        {
            mov = true;
            mat[x1, y1].GetComponent<game2move>().Move(x2,y2,false);
            mat[x2, y2] = mat[x1, y1];
            mat[x1, y1] = null;
        } // 이동

        if(mat[x2, y2] != null && mat[x1, y1] != null && mat[x1,y1].name == mat[x2,y2].name && mat[x1,y1].tag != "Combine" && mat[x2,y2].tag != "Combine")
        {
            mov = true;
            for(j = 0; j<=16; j++)
            {
                if (mat[x2, y2].name == n[j].name + "(Clone)") { break; }
            }

            mat[x1, y1].GetComponent<game2move>().Move(x2, y2, true);
            Destroy(mat[x2, y2]);
            mat[x1, y1] = null;
            mat[x2, y2] = Instantiate(n[j + 1], new Vector3(287f * x2 - 436f, 287f * y2 - 270f, 0), Quaternion.identity);
            mat[x2, y2].tag = "Combine";
            //mat[x2, y2].GetComponent<Animator>().SetTrigger("combine");

            scorenum = scorenum + (int)Mathf.Pow(2, j+2);
        }
    }
}
