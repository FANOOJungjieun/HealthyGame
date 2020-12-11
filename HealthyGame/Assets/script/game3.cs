using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game3 : MonoBehaviour
{
    public ArrayLayout boardLayout;
    public Sprite[] pieces;
    public RectTransform gameBoard;
    public RectTransform killedBoard;
    public Text score, bscore;

    [Header("Prefabs")]
    public GameObject nodePiece;
    public GameObject killedPiece;

    int width = 9;
    int height = 14;
    int[] fills; //채워진 x좌표
    int scoredata = 0;
    Node[,] board;

    List<NodePiece> update;
    List<FlippedPieces> flipped;
    List<NodePiece> dead;
    List<KilledPieces> killed;


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
            FlippedPieces flip = getFlipped(piece);
            NodePiece flippedPiece = null;

            int x = (int)piece.index.x;
            fills[x] = Mathf.Clamp(fills[x] - 1, 0, width);

            List<game3point> connected = isConnected(piece.index, true);
            bool wasFlipped = (flip != null);

            if (wasFlipped)
            {
                flippedPiece = flip.getOtherPiece(piece);
                AddPoints(ref connected, isConnected(flippedPiece.index, true));
            }
            if(connected.Count == 0) //매치 달성되지 못함
            {
                if (wasFlipped) //flip
                {
                    FlipPieces(piece.index, flippedPiece.index, false); //flip back

                }
            } else //매치 달성됨
            {
                foreach(game3point pnt in connected) //이어졌을때 노드 피스 삭제
                {
                    KillPiece(pnt);
                    Node node = getNodeAtPoint(pnt);
                    NodePiece nodePiece = node.getPiece();
                    if (nodePiece != null)
                    {
                        nodePiece.gameObject.SetActive(false);
                        dead.Add(nodePiece);

                        scoredata++;
                    }
                    node.SetPiece(null);
                }

                ApplyGravityToBoard();
            }

            score.text = scoredata.ToString();
            int tmp = PlayerPrefs.GetInt("BestMatch3", 0);
            if(scoredata > tmp)
            {
                PlayerPrefs.SetInt("BestMatch3", scoredata);
            }
            bscore.text = PlayerPrefs.GetInt("BestMatch3", 0).ToString();

            flipped.Remove(flip); //플립 삭제
            update.Remove(piece);
        }
    }

    void ApplyGravityToBoard()
    {
        for(int x=0; x<width; x++)
        {
            for(int y = (height-1); y>=0; y--)
            {
                game3point p = new game3point(x, y);
                Node node = getNodeAtPoint(p);
                int val = getValueAtPoint(p);
                if (val != 0) continue; //구멍이 아닐경우 아무것도 안함
                for (int ny = (y-1); ny>=-1; ny--)
                {
                    game3point next = new game3point(x, ny);
                    int nextVal = getValueAtPoint(next);
                    if(nextVal == 0)
                        continue;
                    if(nextVal != -1) //끝에 도달하지 않았으나 0이 아닐때
                    {
                        Node gotten = getNodeAtPoint(next);
                        NodePiece piece = gotten.getPiece();
                        //구멍 세팅
                        node.SetPiece(piece);
                        update.Add(piece);

                        //구멍 교체
                        gotten.SetPiece(null);
                    }
                    else //hit the end
                    {
                        //전부 채워짐
                        int newVal = fillPiece();
                        NodePiece piece;
                        game3point fallpnt = new game3point(x, (-1 - fills[x]));
                        if(dead.Count>0)
                        {
                            NodePiece revived = dead[0];
                            revived.gameObject.SetActive(true);
                            revived.rect.anchoredPosition = getPositionFromPoint(fallpnt);
                            piece = revived;

                            dead.RemoveAt(0);
                        }
                        else
                        {
                            GameObject obj = Instantiate(nodePiece, gameBoard);
                            NodePiece n = obj.GetComponent<NodePiece>();
                            RectTransform rect = obj.GetComponent<RectTransform>();
                            rect.anchoredPosition = getPositionFromPoint(fallpnt);
                            piece = n;
                        }
                        piece.Initialize(newVal, p, pieces[newVal - 1]);

                        Node hole = getNodeAtPoint(p);
                        hole.SetPiece(piece);
                        ResetPiece(piece);
                        fills[x]++;
                    }
                    break;
                }
            }
        }
    }


    FlippedPieces getFlipped(NodePiece p)
    {
        FlippedPieces flip = null;
        for (int i = 0; i < flipped.Count; i++)
        {
            if (flipped[i].getOtherPiece(p) != null)
            {
                flip = flipped[i];
                break;
            }
        }
        return flip;
    }

    void StartGame()
    {
        fills = new int[width];
        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());
        update = new List<NodePiece>();
        flipped = new List<FlippedPieces>();
        dead = new List<NodePiece>();
        killed = new List<KilledPieces>();
        bscore.text = PlayerPrefs.GetInt("BestMatch3", 0).ToString();

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
                Node node = getNodeAtPoint(new game3point(x, y));

                int val = board[x, y].value;
                if(val<=0) continue;
                GameObject p = Instantiate(nodePiece, gameBoard);
                NodePiece piece = p.GetComponent<NodePiece>();
                RectTransform rect = p.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(32 + (64 * x), -32 - (64 * y));
                piece.Initialize(val, new game3point(x, y), pieces[val - 1]);
                node.SetPiece(piece);

            }
        }
    }

    public void ResetPiece(NodePiece piece)
    {
        piece.ResetPosition();
        //piece.flipped = null;
        update.Add(piece);
    }

    public void FlipPieces(game3point one, game3point two, bool main)
    {
        if (getValueAtPoint(one) < 0) return;

        Node nodeOne = getNodeAtPoint(one);
        NodePiece pieceOne = nodeOne.getPiece();
        if(getValueAtPoint(two) >0)
        {
            Node nodeTwo = getNodeAtPoint(two);
            NodePiece pieceTwo = nodeTwo.getPiece();
            nodeOne.SetPiece(pieceTwo);
            nodeTwo.SetPiece(pieceOne);

            if(main)
                flipped.Add(new FlippedPieces(pieceOne, pieceTwo));

            update.Add(pieceOne);
            update.Add(pieceTwo);
        }else
        {
            ResetPiece(pieceOne);
        }
    }

    void KillPiece(game3point p)
    {
        List<KilledPieces> available = new List<KilledPieces>();
        for(int i=0; i<killed.Count; i++)
        {
            if (!killed[i].falling) available.Add(killed[i]);
        }
        KilledPieces set = null;
        if(available.Count >0)
        {
            set = available[0];
        }
        else
        {
            GameObject kill = GameObject.Instantiate(killedPiece, killedBoard);
            KilledPieces kPiece = kill.GetComponent<KilledPieces>();
            set = kPiece;
            killed.Add(kPiece);
        }

        int val = getValueAtPoint(p) - 1;
        if (set != null && val >= 0 && val < pieces.Length)
            set.Initialize(pieces[val], getPositionFromPoint(p));
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

    Node getNodeAtPoint(game3point p)
    {
        return board[p.x, p.y];
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
    NodePiece piece;

    public Node(int v, game3point i)
    {
        value = v;
        index = i;
    }
    public void SetPiece(NodePiece p)
    {
        piece = p;
        value = (piece == null) ? 0 : piece.value;
        if (piece == null) return;
        piece.SetIndex(index);
    }

    public NodePiece getPiece()
    {
        return piece;
    }

}

[System.Serializable]
public class FlippedPieces
{
    public NodePiece one;
    public NodePiece two;

    public FlippedPieces(NodePiece o, NodePiece t)
    {
        one = o;
        two = t;

    }
    public NodePiece getOtherPiece(NodePiece p)
    {
        if (p == one)
            return two;
        else if (p == two)
            return one;
        else
            return null;
    }
}