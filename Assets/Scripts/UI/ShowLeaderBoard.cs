using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class ShowLeaderBoard : MonoBehaviour
{
    public TextMeshProUGUI RankData; 
    public TextMeshProUGUI NameData; 
    public TextMeshProUGUI TimeData;

    void Start()
    {
        var allData = SaveManager.instance.AllData();

        int id = 0;

        string PlayerName = "";
        string PlayerRank = "";
        string PlayerTime = "";

        PlayerData[] players = allData.Select(kvp => new PlayerData
        {
            name = kvp.Key,
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
