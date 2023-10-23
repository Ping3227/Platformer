using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Platformer.UI
{
    public class GamesceneUIController : MonoBehaviour
    {
        public static GamesceneUIController instance { get; private set; }
        
        [SerializeField] GameObject Canvas;
        
        [Header("HUD")]
        [SerializeField] Canvas HUDCanvas;
        [SerializeField] Slider healthbar;
        [SerializeField] Slider Stamina;

        [Header("Pause")]
        [SerializeField] Canvas PauseCanvas;
        public static bool IsPause = false;

        [Header("Death")]
        [SerializeField] Canvas DeathCanvas;
        [SerializeField] TMP_Text DeathText;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                DontDestroyOnLoad(Canvas.gameObject);
            }
            else{
                Destroy(gameObject);
            }
        }
        
        void Update(){
            if (Input.GetButtonDown("Cancel")){
                if (Input.GetKeyDown(KeyCode.Escape)){
                    if (IsPause){
                        Resume();
                    }
                    else{
                        Pause();
                    }
                }
            }
            Debug.Log($"UI: {instance == null}, {instance == this}");
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

        public void LoadMenu(){
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        #region Update UI
        public void SetMaxHealth(float health)
        {
            healthbar.maxValue = health;
            healthbar.value = health;
            
        }

        public void SetHealth(float health )
        {
            healthbar.value = health;
        }

        public void SetStamina(float stamina) { 
            Stamina.value = stamina;
        }
        public void SetMaxStamina(float stamina)
        {
            Stamina.maxValue = stamina;
            Stamina.value = stamina;
        }
        #endregion
        public void Death() {
            
            LeanTween.alphaCanvas(DeathCanvas.GetComponent<CanvasGroup>(), 1, 1f).setEase(LeanTweenType.easeOutCubic).setDelay(0.5f);
        }
        public void Restart()
        {
            LeanTween.alphaCanvas(DeathCanvas.GetComponent<CanvasGroup>(), 0, 1f).setEase(LeanTweenType.easeOutCubic);
        }
    }
}
