using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Platformer.UI {
    /// <summary>
    /// This class is responsible for managing the UI.
    /// also implement the event call by UI interaction
    /// </summary>
    public class UIController : MonoBehaviour
    {
        public Button playButton;
        public Button quitButton;
        public Button RankingListButton;
        public Button submitButton;

        public string PlayerName;

        public TextMeshProUGUI userInput;
        
        /// <summary>
        /// boolean to check if the game is paused
        /// </summary>
        
        void Start ()
        {
            playButton.onClick.AddListener(PlayGame);
            quitButton.onClick.AddListener(QuitGame);
            RankingListButton.onClick.AddListener(LoadRankingList);

        }

        /// <summary>
        /// Quit the game, bind to the Quit button
        /// </summary>
        void QuitGame() 
        {
            Application.Quit();
        }

        void PlayGame()
        {
            SaveManager.instance.Name = userInput.text;
            Debug.Log(PlayerName);
            SceneManager.LoadScene("Tutortial");
        }

        void LoadRankingList()
        {
            SceneManager.LoadScene("LeaderBoard");
        }

    }


}

