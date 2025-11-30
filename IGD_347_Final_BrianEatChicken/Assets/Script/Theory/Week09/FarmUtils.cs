using System;
using UnityEngine;

public class FarmUtils 
{
    public const int WoodCapacity = 2;
    public const float Gravity = 9.81f;

    public static readonly float DayTime = (DateTime.Now.Month >= 4) ? 10 : 8;
    public static readonly float NightTime = (DateTime.Now.Month >= 4) ? 4 : 6;
    public static int CalculateWoodCapacity(int amount)
    {
        return WoodCapacity * amount;
    }
}
