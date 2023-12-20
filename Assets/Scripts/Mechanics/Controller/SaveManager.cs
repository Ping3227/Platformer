using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private Dictionary<String, SaveData> SaveDatas ;

    public string Name;
    public string Level;

    [Serializable]
    public class SaveData{
        public string PlayerName;
        public int minute;
        public int second;
        public int milisecond;
        public string level;
        public String CheckPoint;
    }
    public static SaveManager instance;
    
    public TimeSpan recentSpan;
  
    public void Save(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        recentSpan = new TimeSpan(0, 0, data.minute, data.second, data.milisecond);
        data.PlayerName = Name;
        data.level = Level;
        string myKey = Name + "_" + Level;
        if (SaveDatas.ContainsKey(myKey) && TimeCompare(myKey, data))
        {
            SaveDatas[myKey] = data;
            Debug.Log("Data Is saved");
        }
        else if (!SaveDatas.ContainsKey(myKey))
        {
            SaveDatas.Add(myKey, data);
            Debug.Log("Data Is created");
        }
        string path = Application.persistentDataPath + "/save.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, SaveDatas);
        stream.Close();
    }

    private bool TimeCompare(String Name, SaveData data)
    {
        SaveData origin = SaveDatas[Name];
        TimeSpan originSpan = new TimeSpan(0, 0, origin.minute, origin.second, origin.milisecond);
        TimeSpan dataSpan = new TimeSpan(0, 0, data.minute, data.second, data.milisecond);
        return dataSpan.CompareTo(originSpan) < 0;
    }

    public SaveData Load(String Name)
    {
        return SaveDatas[Name];
    }

    public Dictionary<String, SaveData> AllData()
    {
        return SaveDatas
        .Where(kvp => kvp.Value.level == Level)
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        //return SaveDatas;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
       
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/save.data", FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        if(fileStream.Length == 0)
        {
            SaveDatas = new Dictionary<string, SaveData>();
            return;
        }
        else { 
            SaveDatas = (Dictionary<String, SaveData>)formatter.Deserialize(fileStream);
        }
        fileStream.Close();
    }
}
