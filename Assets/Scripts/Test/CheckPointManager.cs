using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager instance;
    [SerializeField] CheckPoint[] CheckPoints;
    void Awake()
    {
        instance = this;
        if(CheckPoints.Length == 0)
        {
           CheckPoints =GetComponentsInChildren<CheckPoint>();
        }
    }
    public Transform ReturnCheckPoint(string CheckPointName)
    {
        foreach(CheckPoint checkPoint in CheckPoints)
        {
            if (checkPoint.name == CheckPointName)
            {
                return checkPoint.transform;
            }
        }
        return null;
    }
    
}
