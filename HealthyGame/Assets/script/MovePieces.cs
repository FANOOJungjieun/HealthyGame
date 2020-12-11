using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieces : MonoBehaviour
{
    public static MovePieces instance;
    game3 game;

    NodePiece moving;
    game3point newIndex;
    Vector2 mouseStart;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        game = GetComponent<game3>();
    }

    // Update is called once per frame
    void Update()
    {
        if(moving != null)
        {
            Vector2 dir = ((Vector2)Input.mousePosition - mouseStart);
            Vector2 nDir = dir.normalized;
            Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

            newIndex = game3point.clone(moving.index);
            game3point add = game3point.zero;
            if(dir.magnitude > 32) // 마우스가 시작지점부터 32픽셀 이상 전진하면
            {
                //add를 1.0 / -1.0 . 0.1/ 0.-1중 하나로 분류
                if (aDir.x > aDir.y)
                    add = (new game3point((nDir.x > 0) ? 1 : -1, 0));
                else if (aDir.y > aDir.x)
                    add = (new game3point(0, (nDir.y > 0) ? 1 : -1));
            }
            newIndex.add(add);

            Vector2 pos = game.getPositionFromPoint(moving.index);
            if (!newIndex.Equals(moving.index))
                pos += game3point.mult(add, 16).ToVector();
            moving.MovePositionTo(pos);

        }
    }

    public void MovePiece(NodePiece piece)
    {
        if (moving != null) return;
        moving = piece;
        mouseStart = Input.mousePosition;

    }

    public void DropPiece()
    {
        if (moving == null) return;
        //Debug.Log("Dropped");
        //newindex가 movinginddex와 일치하지 않으면 조각교환. 그렇지 않으면 조각을 원래위치에.
        game.ResetPiece(moving);
        moving = null;
    }
}
