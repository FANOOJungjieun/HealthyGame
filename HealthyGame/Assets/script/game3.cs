using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game3 : MonoBehaviour
{
    public ArrayLayout boardLayout;
    public Sprite[] pieces;
    public RectTransform gameBoard;

    [Header("Prefabs")]
    public GameObject nodePiece;

    int width = 9;
    int height = 14;
    Node[,] board;

    List<NodePiece> update;

    System.Random random;

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        List<NodePiece> finishedUpdating = new List<NodePiece>();
        for(int i=0; i<update.Count; i++)
        {
            NodePiece piece = update[i];
            if (!piece.UpdatePiece()) finishedUpdating.Add(piece);

        }
        for (int i = 0; i < finishedUpdating.Count; i++)
        {
            NodePiece piece = finishedUpdating[i];
            update.Remove(piece);
        }
    }

    void StartGame()
    {
        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());
        update = new List<NodePiece>();

        InitializeBoard();
        verifyBoard();
        InstantiateBoard();
    }

    void InitializeBoard()
    {
        board = new Node[width, height];
        for(int y=0; y<height; y++)
        {
            for(int x=0; x<width; x++)
            {
                board[x, y] = new Node((boardLayout.rows[y].row[x]) ? -1 : fillPiece(), new game3point(x, y));
            }
        }
    }

    void verifyBoard()
    {
        List<int> remove;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
               game3point p = new game3point(x, y);
                int val = getValueAtPoint(p);
                if (val <= 0) continue;

                remove = new List<int>();
                while(isConnected(p,true).Count > 0)
                {
                    val = getValueAtPoint(p);
                    if (!remove.Contains(val))
                        remove.Add(val);
                    setValueAtPoint(p, newValue(ref remove));
                }
            }
        }
    }

    void InstantiateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int val = board[x, y].value;
                if(val<=0) continue;
                GameObject p = Instantiate(nodePiece, gameBoard);
                NodePiece node = p.GetComponent<NodePiece>();
                RectTransform rect = p.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(32 + (64 * x), -32 - (64 * y));
                node.Initialize(val, new game3point(x, y), pieces[val - 1]);
            }
        }
    }

    public void ResetPiece(NodePiece piece)
    {
        piece.ResetPosition();
        update.Add(piece);
    }

    List<game3point> isConnected(game3point p, bool main)
    {
        List<game3point> connected = new List<game3point>();
        int val = getValueAtPoint(p);
        game3point[] directions =
        {
            game3point.up,
            game3point.down,
            game3point.right,
            game3point.left
        };

        foreach(game3point dir in directions) { //checking if there is 2 or more same shape in the direction. 이어져있는지 고려해 찾음
            List<game3point> line = new List<game3point>();

            int same = 0;
            for(int i=1;i< 3; i++)
            {
                game3point check = game3point.add(p, game3point.mult(dir, i));
                if(getValueAtPoint(check) == val)
                {
                    line.Add(check);
                    same++;
                }
            }

            if (same > 1) // 이 줄에 1개이상의 같은모양이 존재(매치)
                AddPoints(ref connected, line); // 이 점을 연결된 리스트에 추가함 
        }

        for(int i=0; i<2; i++) // 같은 모양 2개 사이에 존재하는지 확인
        {
            List<game3point> line = new List<game3point>();

            int same = 0;
            game3point[] check = { game3point.add(p, directions[i]), game3point.add(p, directions[i + 2]) }; //direction의 4개 전부탐색
            foreach(game3point next in check) //조각의 양 옆이 같은 값인지 확인한 뒤, 그렇다면 리스트에 추가
            {
                if (getValueAtPoint(next) == val)
                {
                    line.Add(next);
                    same++;
                }
            }
            if (same > 1)
                AddPoints(ref connected, line);
        }

        for(int i=0; i<4; i++) // 2x2체크
        {
            List<game3point> square = new List<game3point>();

            int same = 0;
            int next = i + 1;
            if(next >=4)
            {
                next -= 4;
            }

            game3point[] check = { game3point.add(p, directions[i]), game3point.add(p, directions[next]), game3point.add(p, game3point.add(directions[i], directions[next]))};
            foreach (game3point pnt in check) //조각의 양 옆이 같은 값인지 확인한 뒤, 그렇다면 리스트에 추가
            {
                if (getValueAtPoint(pnt) == val)
                {
                    square.Add(pnt);
                    same++;
                }
            }
            if(same > 2)
            {
                AddPoints(ref connected, square);
            }
        }

        if(main) //현재 다른 매치된 것들을 찾아냄
        {
            for(int i=0; i<connected.Count; i++)
            {
                AddPoints(ref connected, isConnected(connected[i], false));
            }
        }

        if (connected.Count > 0)
            connected.Add(p);

        return connected;
    }

    void AddPoints(ref List<game3point> points, List<game3point> add)
    {
        foreach(game3point p in add)
        {
            bool doadd = true;
            for(int i=0; i< points.Count; i++)
            {
                if(points[i].Equals(p))
                {
                    doadd = false;
                    break;
                }
            }

            if (doadd) points.Add(p);
        }
    }

    int fillPiece()
    {
        int val = 1;
        val = (random.Next(0, 100) / (100 / pieces.Length)) + 1;
        return val;
    }

    int getValueAtPoint(game3point p)
    {
        if (p.x < 0 || p.x >= width || p.y < 0 || p.y >= height) return -1;
        return board[p.x, p.y].value;
    }

    void setValueAtPoint(game3point p, int v)
    {
        board[p.x, p.y].value = v;
    }

    int newValue(ref List<int> remove)
    {
        List<int> available = new List<int>();
        for(int i=0; i<pieces.Length; i++)
        {
            available.Add(i + 1);
        }
        foreach (int i in remove)
            available.Remove(i);

        if (available.Count <= 0) return 0;
        return available[random.Next(0, available.Count)];
    }

    string getRandomSeed()
    {
        string seed = "";
        string acceptableChara = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
        for(int i=0; i<20; i++)
        {
            seed += acceptableChara[Random.Range(0, acceptableChara.Length)];
        }
        return seed; 
    }

    public Vector2 getPositionFromPoint(game3point p)
    {
        return new Vector2(32 + (64 * p.x), -32 - (64 * p.y));
    }
}

[System.Serializable]

public class Node
{
    public int value; //0=black, 1=cube, 2=sphere, 3=cylinder, 4=pryamid, 5=octahedron, -1=hole
    public game3point index;

    public Node(int v, game3point i)
    {
        value = v;
        index = i;
    }
}