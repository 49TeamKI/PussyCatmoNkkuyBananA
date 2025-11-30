using System;
using UnityEngine;

public class Sheep 
{
    public int SheepNumber;
    public static int _totalSheepCount;
    private static readonly int _initialPopulation = 5;
    static Sheep()
    {
        _totalSheepCount = _initialPopulation;
    }
    public int ASheepNumber
    {
        get { return SheepNumber; }
    }
    public static int TotalSheepCount
    {
        get { return _totalSheepCount; }
    }

    public Sheep(int sheepNumber)
    {
        SheepNumber = sheepNumber;
        _totalSheepCount++;
    }

    public int AskNumber()
    {
        return SheepNumber;
    }
    public static int GetAllSheep()
    {
        return _totalSheepCount;
    }
    public void SetNumber(int number)
    {
        SheepNumber = number;
    }
    public static void RemoveSheep(int count)
    {
        _totalSheepCount -= count;
    }
    public void Jump()
    {
        Debug.Log("Sheep is jump Sheep got gravity " + FarmUtils.Gravity);
    }
}
