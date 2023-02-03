using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//마스터만 사용함 
public class RuleManager : MonoBehaviour
{
    /* 0  1  ... 13 14
     * 15 16 ... 28 29
     * 30 31 ... 43 44
     * ...
     */
    // x(가로), y(세로)
    //빈 공간 : 0
    // 흑돌 : 1
    // 백돌 : 2

    /*         *<-돌 위치   (33)
     * 0 1 2 3 4 5 6 7 8
     * _ _ 0 0 1 1 1 0 0  c0
     * _ 0 0 1 1 1 0 0 _  c0
     * 0 0 1 1 1 0 0 _ _  c0
     * _ _ _ 0 1 0 1 1 0  c1
     * _ 0 1 0 1 1 0 _ _  c1
     * 0 1 0 1 1 0 _ _ _  c1
     * _ _ _ 0 1 1 0 1 0  c1
     * _ _ 0 1 1 0 1 0 _  c1
     * 0 1 1 0 1 0 _ _ _  c1
     * _ 0 1 0 1 0 1 0 _  c2
     * 
     *   5 = anything Continue
     */
       
    /*         *    (44) 범위 -4 ~ 4
     * 0 1 2 3 4 5 6 7 8       
     * _ _ _ _ 1 1 1 1 _
     * _ _ _ 1 1 1 1 _ _
     * _ _ 1 1 1 1 _ _ _  
     * _ 1 1 1 1 _ _ _ _  
     * _ _ _ _ 1 0 1 1 1
     * _ _ 1 0 1 1 1 _ _
     * _ 1 0 1 1 1 _ _ _
     * 1 0 1 1 1 _ _ _ _
     * _ _ _ _ 1 1 0 1 1
     * _ _ _ 1 1 0 1 1 _
     * _ 1 1 0 1 1 _ _ _
     * 1 1 0 1 1 _ _ _ _
     * _ _ _ _ 1 1 1 0 1
     * _ _ _ 1 1 1 0 1 _
     * _ _ 1 1 1 0 1 _ _
     * 1 1 1 0 1 _ _ _ _
     */

    int[,] map = new int[15, 15];

    Dictionary<int, int> Dic_Bans = new Dictionary<int, int>(); //<Index, Ban Type>

    [SerializeField] private GameManager GameManager;
    [SerializeField] private BoardManager Board;


    private void Start()
    {
        if (GameManager == null)
            GameManager = gameObject.GetComponent<GameManager>();
    }

    //int[9] case = Crack Count
    //0 = crack, 1 = blackStone, 5 = anything Continue
    readonly int[,] case33 =
        {{ 5,5,0,0,1,1,1,0,0,0 },
         { 5,0,0,1,1,1,0,0,5,0 },
         { 0,0,1,1,1,0,0,5,5,0 },
         { 5,5,5,0,1,0,1,1,0,1 },
         { 5,0,1,0,1,1,0,5,5,1 },
         { 0,1,0,1,1,0,5,5,5,1 },
         { 5,5,5,0,1,1,0,1,0,1 },
         { 5,5,0,1,1,0,1,0,5,1 },
         { 0,1,1,0,1,0,5,5,5,1 },
         { 5,0,1,0,1,0,1,0,5,2 } };


    readonly int[,] case44 = { { 5,5,5,5,1,1,1,1,5 },
                                {5,5,5,1,1,1,1,5,5},
                                {5,5,1,1,1,1,5,5,5},
                                {5,1,1,1,1,5,5,5,5},
                                {5,5,5,5,1,0,1,1,1},
                                {5,5,1,0,1,1,1,5,5},
                                {5,1,0,1,1,1,5,5,5},
                                {1,0,1,1,1,5,5,5,5},
                                {5,5,5,5,1,1,0,1,1},
                                {5,5,5,1,1,0,1,1,5},
                                {5,1,1,0,1,1,5,5,5},
                                {1,1,0,1,1,5,5,5,5},
                                {5,5,5,5,1,1,1,0,1},
                                {5,5,5,1,1,1,0,1,5},
                                {5,5,1,1,1,0,1,5,5},
                                {1,1,1,0,1,5,5,5,5 } };

    readonly int[,] exception44 = { { 5,5,1,1,1,1,1},
                                    { 5,1,1,1,1,1,5},
                                    { 1,1,1,1,1,5,5}};


    readonly int[] dx = { 1, 0, 1, 1 };
    readonly int[] dy = { 0, 1, 1, -1 };


    public void SetStone(int _index, bool _isBlack)
    {
        var (x, y) = IndexToCoor(_index);
        //Debug.Log("x :" + x + " y : " + y);

        if (_isBlack)
            map[x, y] = 1;
        else
            map[x, y] = 2;

        if (isWin(x, y, _isBlack))
        {
            if (_isBlack)
                GameManager.M_O_GameResult(1, 2); //마스터가 이김(흑돌)
            else
                GameManager.M_O_GameResult(2, 1); //슬레이브가 이김(백돌)

        }

        CheckBans(); //딕셔너리에 저장되어 있는 데이터 체크

        if (_isBlack)
            SearchBans(x, y); //현재 놓은 돌 기준으로 33 44 장목 체크
    }

    int CoorToIndex(int x, int y)
    {
        int index = x + y * 15;
        return index;
    }

    (int x, int y) IndexToCoor(int _index)
    {
        if (_index == 0)
            return (0, 0);
        return (_index % 15, _index / 15);
    }

    void SearchBans(int x, int y)
    {
        for (int goX = -4; goX <= 4; goX++)
        {
            for (int goY = -4; goY <= 4; goY++)
            {
                if (!RangeCheck(x + goX, y + goY)) continue;
                if (Ban44(x + goX, y + goY))
                {
                    if (Dic_Bans.ContainsKey(CoorToIndex(x + goX, y + goY)))
                        continue;
                    //Debug.Log("Index@@@" + CoorToIndex(x + goX, y + goY));
                    Board.BanStone(CoorToIndex(x + goX, y + goY), 44, true);
                    Dic_Bans.Add(CoorToIndex(x + goX, y + goY), 44);
                }

            }
        }

        for (int goX = -3; goX <= 3; goX++)
        {
            for (int goY = -3; goY <= 3; goY++)
            {
                if (!RangeCheck(x + goX, y + goY)) continue;
                if (Ban33(x + goX, y + goY)) {
                    if (Dic_Bans.ContainsKey(CoorToIndex(x + goX, y + goY)))
                        continue;
                    //Debug.Log("Index@@@" + CoorToIndex(x + goX, y + goY));
                    Board.BanStone(CoorToIndex(x + goX, y + goY), 33, true);
                    Dic_Bans.Add(CoorToIndex(x + goX, y + goY), 33);
                }
            }
        }


        var (jangX, jangY) = Ban6(x, y);
        if (jangX != -1) {
            if (Dic_Bans.ContainsKey(CoorToIndex(jangX, jangY)))
                return;
            //Debug.Log("Index@@@" + CoorToIndex(jangX , jangY));
            Board.BanStone(CoorToIndex(jangX, jangY), 6, true);
            Dic_Bans.Add(CoorToIndex(jangX, jangY), 6);
        }

    }

    void CheckBans()
    {
        if (Dic_Bans.Count <=0 )
            return;

        int removeKey = -1;
        foreach(KeyValuePair<int,int> items in Dic_Bans)
        {
            var (x, y) = IndexToCoor(items.Key);
            if(items.Value == 33)
            {
                if (!Ban33(x, y))
                {
                    Board.BanStone(items.Key, 33, false);
                    removeKey=items.Key;
                }
            }
            else if(items.Value == 44)
            {
                if (!Ban44(x, y))
                {
                    Board.BanStone(items.Key, 44, false);
                    removeKey = items.Key;
                }
            }
            else if(items.Value == 6)
            {
                var (jangX, jangY) = Ban6(x, y);
                if(jangX == -1)
                {
                    Board.BanStone(items.Key, 6, false);
                    removeKey = items.Key;

                }
            }
        }
        // foreach문에서 Remove시 아래와 같은 에러 발생
        //InvalidOperationException: Collection was modified;
        //enumeration operation may not execute.
        if (removeKey != -1)
            Dic_Bans.Remove(removeKey); 
    }

    bool Ban33(int x, int y)
    {
        if (map[x, y] != 0) //빈 공간 확인
            return false;

        //흑돌이 놓여있다는걸 가정하기
        map[x, y] = 1;

        int[] crack = { 0,0,0 };
       

        for (int d = 0; d < 4; d++) //dx dy 
        {
            for (int h = 0; h < case33.GetLength(0); h++) //Case 세로
            {
                bool isSatisfied = true;
                int RangeOver = 0; 

                for (int w = -4; w <= 4; w++) //Case 가로
                {
                    int goX = dx[d] * w;
                    int goY = dy[d] * w;
                    if (case33[h, w + 4] == 5) continue; //필요 없는 부분 continue
                    if (!RangeCheck(x + goX, y + goY))
                    {
                        RangeOver++;
                        if (RangeOver > 1)  //2개 이상 다르다? break한다~
                        {
                            isSatisfied = false;
                            break;
                        }
                        continue; //범위 벗어나면 continue
                    }
                    if (map[x + goX, y + goY] != case33[h, w + 4])  //일치 하지 않으면 false
                    {
                        isSatisfied = false;
                        break;
                    }
                }
                if (isSatisfied)
                {
                    crack[case33[h, 9]]++;
                    break;
                }
            }
        }
        //원래대로
        map[x, y] = 0;

        if (crack[0] + crack[1] > 1)
            return true; //33이다.
        if (crack[2] > 0 && crack[0] > 0) return true; //33이다

        return false;

    }
    
    bool Ban44(int x, int y)
    {
        if (map[x, y] != 0) //빈 공간 확인
            return false;

        //흑돌이 놓여있다는걸 가정하기
        map[x, y] = 1;

        int satisfiedCnt = 0;
        for (int d = 0; d < 4; d++) //dx dy
        {
            for(int h = 0; h< exception44.GetLength(0); h++) //exception 열
            {
                bool exception = true;
                for(int w =-3; w <= 3; w++) //exception 행 
                {
                    int goX = dx[d] * w;
                    int goY = dy[d] * w;
                    if (exception44[h, w + 3] == 5) continue;
                    if (!RangeCheck(x + goX, y + goY))  //범위에 벗어났으면 확인할 필요가 없음.
                    {
                        exception = false;
                        break;
                    } 
                    if (map[x + goX, y + goY] != exception44[h, w + 3]) //예외배열과 다르다면
                    {
                        exception = false;
                        break;
                    }
                }
                if (exception)
                {
                    if (d == 3) //마지막방향에서 예외처리 됨 -> 해당 좌표에 44는 없다.
                    {
                        map[x, y] = 0;
                        return false;
                    }
                    d++; //dx dy 바꾸기
                    
                }
            }

            for (int h = 0; h < case44.GetLength(0); h++) //Case 열
            {
                bool isSatisfied = true;

                for (int w = -4; w <= 4; w++) //Case 행
                {
                    int goX = dx[d] * w;
                    int goY = dy[d] * w;

                    if (case44[h, w + 4] == 5)  continue; //필요 없는 부분 continue
                    if (!RangeCheck(x + goX, y + goY)) //범위 벗어나면 break(실패)
                    {
                        isSatisfied = false;
                        break;
                    }

                    if (map[x + goX, y + goY] != case44[h, w + 4]) //다르면 break(실패)
                    {
                        isSatisfied = false;
                        break;
                    }
                  
                }
                if (isSatisfied)
                {
                    satisfiedCnt++;
                }

                if (satisfiedCnt > 1) //4금수에 만족하는 줄이 2개 이상이면 44이다.
                {
                    map[x, y] = 0;
                    return true;
                }
            }
        }
        map[x, y] = 0;
        return false;
    }

    //장목은 좌표를 줄거임
    (int virX, int virY) Ban6(int x, int y)
    {

        for (int d = 0; d < 4; d++)
        {
            int blank = 0;
            int blackCnt = 0;
            int blankX = -1, blankY = -1;
            for (int i = 0; i < 6; i++)
            {
                int goX = dx[d] * i;
                int goY = dy[d] * i;
                if (!RangeCheck(x + goX, y + goY)) break;
                if (map[x + goX, y + goY] == 0)
                {
                    blank++;
                    blankX = x+goX; blankY = y+ goY;
                    if (blank > 1) break;
                }
                else if (map[x + goX, y + goY] == 1) blackCnt++;
                else if (map[x + goX, y + goY] == 2) break;

                if (blackCnt >= 5)//장목이다.
                {
                    return (blankX, blankY);
                }
            }

            blank = 0;
            blackCnt = 0;
            for (int i = 0; i < 6; i++)
            {
                int goX = dx[d] * i;
                int goY = dy[d] * i;
                if (!RangeCheck(x - goX, y - goY)) break;
                if (map[x - goX, y - goY] == 0)
                {
                    blank++;
                    blankX = x + goX; blankY = y + goY;
                    if (blank > 1) break;
                }
                else if (map[x - goX, y - goY] == 1) blackCnt++;
                else if (map[x - goX, y - goY] == 2) break;

                if (blackCnt >= 5)//장목이다.
                {
                    return (blankX, blankY);

                }
            }
        }

        return (-1,-1); //실패
    }


    bool isWin(int x, int y, bool _isblack)
    {

        for (int d = 0; d < 4; d++)
        {
            int stoneCnt = 0;
            for (int i = -4; i < 5; i++)
            {
                int goX = dx[d] * i;
                int goY = dy[d] * i;
                if (RangeCheck(x + goX, y + goY))
                {
                    if (map[x + goX, y + goY] == (_isblack ? 1 : 2))
                    {
                        stoneCnt++;
                        if (stoneCnt > 4)
                            return true;
                    }
                    else
                    {
                        stoneCnt = 0;
                    }
                }
            }

        }
        return false;
    }

        bool RangeCheck(int x, int y)
    {
        if (x > 14 || x < 0 || y > 14 || y < 0)
            return false;
        return true;
    }
}
