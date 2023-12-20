using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class ShowLeaderBoard : MonoBehaviour
{
    public TextMeshProUGUI RankData; 
    public TextMeshProUGUI NameData; 
    public TextMeshProUGUI TimeData;

    public TextMeshProUGUI BestData;

    public TextMeshProUGUI LevelData;

    public class PlayerData
    {
        public string name;
        public string level;
        public TimeSpan timeSpan;
        public int minute;
        public int second;
        public int milisecond;
    }

    void Start()
    {
        var allData = SaveManager.instance.AllData();

        string level = SaveManager.instance.Level;

        int id = 0;

        string PlayerName = "";
        string PlayerRank = "";
        string PlayerTime = "";

        PlayerData[] players = allData.Select(kvp => new PlayerData
        {
            name = kvp.Key.Split('_')[0],
            timeSpan = new TimeSpan(0, 0, kvp.Value.minute, kvp.Value.second, kvp.Value.milisecond),
            minute = kvp.Value.minute,
            second = kvp.Value.second,
            milisecond = kvp.Value.milisecond,
        }).ToArray();

        players = players.OrderBy(player => player.timeSpan.TotalMilliseconds).ToArray();

        foreach (PlayerData player in players)
        {
            id++;
            PlayerName += player.name;
            PlayerName += "\n";
            PlayerRank += id.ToString();
            PlayerRank += "\n";
            PlayerTime += player.minute.ToString().PadLeft(2, '0'); ;
            PlayerTime += ":";
            PlayerTime += player.second.ToString().PadLeft(2, '0'); ;
            PlayerTime += ":";
            PlayerTime += player.milisecond.ToString().PadLeft(3, '0');
            PlayerTime += "\n";
        }
        RankData.text = PlayerRank;
        NameData.text = PlayerName;
        TimeData.text = PlayerTime;
        BestData.text = GetBestData();
        LevelData.text = level;
    }

    string GetBestData()
    {
        string best = "";

        TimeSpan nowSpan = SaveManager.instance.recentSpan;
        best += nowSpan.Minutes.ToString().PadLeft(2, '0'); ;
        best += ":";
        best += nowSpan.Seconds.ToString().PadLeft(2, '0'); ;
        best += ":";
        best += nowSpan.Milliseconds.ToString().PadLeft(3, '0');

        return best;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //void Read

    // Update is called once per frame
    void Update()
    {
        
    }
}
