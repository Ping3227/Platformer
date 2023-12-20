using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Platformer.Mechanics;

namespace Platformer.UI {
    /// <summary>
    /// This class is responsible for managing the UI.
    /// also implement the event call by UI interaction
    /// </summary>
    public class UIController : MonoBehaviour
    {
        //public Button playButton;
        public Button quitButton;
        public Button RankingListButton;
        //public Button submitButton;

        public Button easyButton;
        public Button mediumButton;
        public Button hardButton;

        public string PlayerName;

        public TextMeshProUGUI userInput;
        
        /// <summary>
        /// boolean to check if the game is paused
        /// </summary>
        
        void Start ()
        {
            //playButton.onClick.AddListener(PlayGame);
            quitButton.onClick.AddListener(QuitGame);
            RankingListButton.onClick.AddListener(LoadRankingList);
            easyButton.onClick.AddListener(PlayEasy);
            mediumButton.onClick.AddListener(PlayMedium);
            hardButton.onClick.AddListener(PlayHard);
        }

        /// <summary>
        /// Quit the game, bind to the Quit button
        /// </summary>
        void QuitGame() 
        {
            Application.Quit();
        }

        //void PlayGame()
        //{
        //    SaveManager.instance.Name = userInput.text;
        //    SceneManager.LoadScene("Tutortial");
        //}

        void PlayEasy()
        {
            SaveManager.instance.Name = userInput.text;
            SaveManager.instance.Level = "Easy";
            SceneManager.LoadScene("Tutortial");
            Time.timeScale = 1f;
            GameController.Instance.ResetCheckPoint();
        }

        void PlayMedium()
        {
            SaveManager.instance.Name = userInput.text;
            SaveManager.instance.Level = "Medium";
            SceneManager.LoadScene("BT_boss");
            Time.timeScale = 1f;
            GameController.Instance.ResetCheckPoint();
        }

        void PlayHard()
        {
            SaveManager.instance.Name = userInput.text;
            SaveManager.instance.Level = "Hard";
            SceneManager.LoadScene("BT_boss2");
            Time.timeScale = 1f;
            GameController.Instance.ResetCheckPoint();
        }

        void LoadRankingList()
        {
            SceneManager.LoadScene("LeaderBoard");
        }

    }


}

