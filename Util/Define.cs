using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum UIEvent
    {
        Click,
        Drag,
    }
    public enum MouseEvent
    {
        Press,
        Click,
        Drag,
    }
    public enum Scene
    {
        Unknown,
        Title,
        Main,
        Lobby,
        Game,
    }
    public enum Photon
    {
        None,
        Connected,
        JoinRamdomRoom,
        LeaveRoom,
        ApplyEmoticon,



    }
    public enum Emoticon
    {
        E1,
        E2,
        E3,
        E4,
        E5,
        E6,
        E7,
        E8,
        E9,
        E10,
        E11,
        E12,
        E13,
        E14,
        E15,
        E16,
    }
}
