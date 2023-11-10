using Platformer.Mechanics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public struct Mode {
    // player properties 
    public float Movespeed;
    public float Jumpforce;
    public float DoubleJumpforce;
    public float health;
    public float RechargeingRate;
    public float RechargeingDelay;
}
public class ModeManager : MonoBehaviour
{
    public static ModeManager instance { get; private set; }
    public int index { get; private set; }
    private int length;
    public Mode[] modes;
    private void Awake()
    {
        length= modes.Length;
        instance = this;
    }
    public int getNext() {
        index++;
        if (index >= length) {
            index -= length;
        }
        return index;
    }
}
