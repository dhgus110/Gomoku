using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateInfo : MonoBehaviour
{
    [Header("AboutIndex")]
    [SerializeField] private int index;
    public int Index
    {
        get { return index; }
        set { index = value; }
    }
}
