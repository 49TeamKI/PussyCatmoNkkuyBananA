using System;
using UnityEngine;

public class SheepGenerator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sheep jeb = new Sheep(0);
        Sheep mika = new Sheep(1);
        Sheep bear = new Sheep(3);
        Debug.Log("jeb is number " + jeb.SheepNumber);
        Debug.Log("TotalSheep is " + Sheep._totalSheepCount);

        Debug.Log("mika is number " + mika.AskNumber());
        Debug.Log("All Sheep in farm " + Sheep.GetAllSheep());

        bear.SetNumber(2);
        Debug.Log("bear is number " + bear.ASheepNumber);
        Sheep.RemoveSheep(1);
        Debug.Log("All Sheep in farm " + Sheep._totalSheepCount);

        int WoodCapcity = FarmUtils.CalculateWoodCapacity(Sheep.GetAllSheep());
        Debug.Log("WoodCapcity is " + WoodCapcity);
        Debug.Log("Daytime this month is " + FarmUtils.DayTime + " hours");
    }
}
