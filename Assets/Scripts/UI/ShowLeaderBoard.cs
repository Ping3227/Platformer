using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ShowLeaderBoard : MonoBehaviour
{
    public TextMeshProUGUI RankData; 
    public TextMeshProUGUI NameData; 
    public TextMeshProUGUI TimeData;

    void Start()
    {
        string rank = "";
        string name = "";
        string time = "";
        int id = 0;
        PlayerData[] players = LeaderBoard.Read();

        players = players.OrderBy(player => float.Parse(player.time)).ToArray();

        players = players.OrderBy(player => player.name).ToArray();
        foreach (PlayerData player in players)
        {
            id++;
            name += player.name;
            name += "\n";
            rank += id.ToString();
            rank += "\n";
            time += player.time;
            time += "\n";
            
        }
        RankData.text = rank;
        NameData.text = name;
        TimeData.text = time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
