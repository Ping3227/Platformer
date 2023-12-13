using System;
using System.IO;
using UnityEngine;

public class PlayerData
{
    public string name;
    public string time;
}

public class LeaderBoard : MonoBehaviour
{
    public string fileName = "test";

    void Start()
    {
        PlayerData[] players = Read();
        if(players.Length > 0)
        {
            foreach(PlayerData player in players)
            {
                Debug.Log("Name: " + player.name);
                Debug.Log("Time: " + player.time);
            }
        }
        else
        {
            Debug.Log("No Data");
        }
        
        //Write("Jack", "8.0");
    }

    public PlayerData[] Read()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);

        PlayerData[] players = new PlayerData[0];

        if (textAsset != null)
        {
            string[] lines = textAsset.text.Split('\n');

            if (lines.Length == 1 && lines[0].Length == 0)
            {
                return players;
            }
            foreach (string line in lines)
            {
                string[] values = line.Split(',');
                
                PlayerData newPlayer = new PlayerData
                {
                    name = values[0],
                    time = values[1]
                };

                Array.Resize(ref players, players.Length + 1);
                players[players.Length - 1] = newPlayer;
            }
        }
        return players;
    }


    public void Write(string name, string time)
    {
        using (StreamWriter writeFile = new StreamWriter(System.IO.Path.Combine("Assets/Resources", "leader.txt"), true))
        {
            string first = writeFile.BaseStream.Length > 0 ? "\n" : "";

            writeFile.Write(first + name + "," + time);
        }
    }
}

