using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string Name;
    public DateTime CreatedTime, LastUpdatedTime;
    public Dictionary<string, bool> createdConcoctions;
    public Dictionary<string, bool> discoveredIngredients;
    public SVector3 PlayerPosition;
    public float CameraRotation; 
    public SQuaternion PlayerBodyRotation;

    // General
    public SVector3 SunLocalPosition;
    public SQuaternion SunLocalRotation;
    public SVector3 MoonLocalPosition;
    public SQuaternion MoonLocalRotation;

    public int TimeOfDay, DaysPassed;

    // Journal
    public int PageNoLeft;

    // Ingredients
    public List<IngredientBottleData> IngredientBottles;

    // Merchant
    public MerchantSaveData MerchantSaveData;

    //Starting Values
    private SVector3 startingPlayerPosition = new Vector3(8.8f, 38.2f, 2.5f);
    private float startingCameraRotation = 0f;
    private SQuaternion startingPlayerBodyRotation = Quaternion.identity;

    //private SVector3 startingSunPosition = new SVector3(0, 0, -100);
    //private SQuaternion startingSunRotation = new SQuaternion(0.223752543f, -0.101848498f, 0.0235173181f, 0.969024599f);
    //private SVector3 startingMoonPosition = new SVector3(0, 0, 100);
    //private SQuaternion startingMoonRotation = new SQuaternion(1, 0, 0, 0);

    public SaveData(int count)
    {
        Name = "Game " + count;
        CreatedTime = DateTime.Now;
        LastUpdatedTime = DateTime.Now;
        createdConcoctions = new Dictionary<string, bool>();
        discoveredIngredients = new Dictionary<string, bool>();
        PlayerPosition = startingPlayerPosition;
        CameraRotation = startingCameraRotation;
        PlayerBodyRotation = startingPlayerBodyRotation;
        TimeOfDay = 0;
        DaysPassed = 0;
        //SunLocalPosition = startingSunPosition;
        //SunLocalRotation = startingSunRotation;
        //MoonLocalPosition = startingMoonPosition;
        //MoonLocalRotation = startingMoonRotation;
    }

    // In Game Time of Day
    // In Game Day from start
    // Player location and rotation
    // Player held item if any
    // Book - selected page and general progress
    // All ingredient locations/rotations. Including in containers like mortar
    // All potion locations/rotations.

}
