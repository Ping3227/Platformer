using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




namespace Platformer.UI {
    /// <summary>
    /// This class is responsible for managing the UI.
    /// also implement the event call by UI interaction
    /// </summary>
    public class UIController : MonoBehaviour
    {
        public Button playButton;
        public Button quitButton;
        
       
        /// <summary>
        /// boolean to check if the game is paused
        /// </summary>
        
        void Start ()
        {

            playButton.onClick.AddListener(PlayGame);
            quitButton.onClick.AddListener(QuitGame);
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
            SceneManager.LoadScene("preview");
        }

    }


}

