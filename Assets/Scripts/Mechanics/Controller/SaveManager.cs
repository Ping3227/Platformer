using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private Dictionary<String, SaveData> SaveDatas ;
    [Serializable]
    public class SaveData{
        public string PlayerName;
        public struct Time
        {
            int minute;
            int second;
            int milisecond;
        }
        public Time time;
        public String CurrentLevel;
        public String CheckPoint;
    }
    public static SaveManager instance;
    public void Save(SaveData data) { 
        BinaryFormatter formatter = new BinaryFormatter();
        SaveDatas.Add(data.PlayerName, data);
        string path = Application.persistentDataPath + "/save.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, SaveDatas);
        stream.Close();
    }
    public SaveData Load(String Name)
    {
        return SaveDatas[Name];
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    private void Start()
    {
       
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/save.data", FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        SaveDatas = (Dictionary<String, SaveData>)formatter.Deserialize(fileStream);

    }
}
