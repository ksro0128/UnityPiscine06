using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveData
{
    public string nickName;
    public string timeString;
    public float time;

    public SaveData(string nickName, string timeString, float time)
    {
        this.nickName = nickName;
        this.timeString = timeString;
        this.time = time;
    }
}
