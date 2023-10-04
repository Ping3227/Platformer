using UnityEngine;


namespace Platformer.UI {
    /// <summary>
    /// This class is responsible for managing the UI.
    /// also implement the event call by UI interaction
    /// </summary>
    public class UIController : MonoBehaviour
    {
        /// <summary>
        /// must at least contain Health,Stamina 
        /// </summary>
        [SerializeField]Canvas HUDCanvas;
        /// <summary>
        /// must at least contain Resume,Quit
        /// </summary>
        [SerializeField] Canvas PauseCanvas;
        /// <summary>
        /// boolean to check if the game is paused
        /// </summary>
        private bool IsPause;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                IsPause = !IsPause;
                if (IsPause)
                {
                    Time.timeScale = 0;
                    //to do, Menu Mode: sent event to pause the game
                    if (PauseCanvas)
                    {
                        PauseCanvas.gameObject.SetActive(true);
                    }
                }
                else {
                    Time.timeScale = 1;
                    //to do, Game Mode: sent event to resume the game
                    if (PauseCanvas)
                    {
                        PauseCanvas.gameObject.SetActive(false);
                    }
                }
            }
        }
        /// <summary>
        /// Enable the HUD canvas when the game start
        /// </summary>
        void OnEnable()
        {
            if (HUDCanvas) {
                HUDCanvas.gameObject.SetActive(true);
            }
            
        }
        /// <summary>
        /// Disable the HUD canvas when the game end
        /// </summary>
        void OnDisable()
        {
            if (HUDCanvas)
            {
                HUDCanvas.gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// Quit the game, bind to the Quit button
        /// </summary>
        void Quit() {
            Application.Quit();
        }
        
    }


}

