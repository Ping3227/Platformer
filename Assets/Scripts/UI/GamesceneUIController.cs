using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



namespace Platformer.UI
{
    public class GamesceneUIController : MonoBehaviour
    {
        public static GamesceneUIController instance { get; private set; }
        public static bool IsPause = false;
        
        [SerializeField] Canvas HUDCanvas;
   
        [SerializeField] Canvas PauseCanvas;

        [SerializeField] Slider healthbar;
        [SerializeField] Slider Stamina;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

            void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
          
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                   
                    if (IsPause)
                    {
                        Resume();
                    }
                    else
                    {
                        Pause();
                    }
                }
                
            }
        }
       
        public void Resume()
        {
            IsPause = false;
            Time.timeScale = 1f; 
            HUDCanvas.gameObject.SetActive(true);
            PauseCanvas.gameObject.SetActive(false);
        }
        public void Pause()
        {
            IsPause = true;
            Time.timeScale = 0f;
            HUDCanvas.gameObject.SetActive(false);
            PauseCanvas.gameObject.SetActive(true);
        }

        public void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
            
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void SetMaxHealth(int health)
        {
            healthbar.maxValue = health;
            healthbar.value = health;
        }

        public void SetHealth(int health )
        {
            healthbar.value = health;
        }

        public void SetStamina(int stamina) { 
            Stamina.value = stamina;
        }
        public void SetMaxStamina(int stamina)
        {
            Stamina.maxValue = stamina;
            Stamina.value = stamina;
        }   
    }
}
