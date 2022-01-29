using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateInfo : MonoBehaviour
{
    public int x;
    public int y;

    public int X
    {
        get { return x; }
        set { x = value; }
    }

    public int Y
    {
        get { return y; }
        set { y = value; }
    }
}
