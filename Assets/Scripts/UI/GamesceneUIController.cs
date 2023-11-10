using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Platformer.Core;
using Platformer.Gameplay;

namespace Platformer.UI
{
    public class GamesceneUIController : MonoBehaviour
    {
        public static GamesceneUIController instance { get; private set; }
        
        [SerializeField] GameObject Canvas;
        
        [Header("HUD")]
        [SerializeField] Canvas HUDCanvas;
        [SerializeField] Slider healthLoss;
        [SerializeField] Slider healthbar;
        [SerializeField] Slider Stamina;
        [SerializeField] TMP_Text ShowTime;
        private float TimeCounter;

        [Header("Pause")]
        [SerializeField] Canvas PauseCanvas;
        public static bool IsPause = false;

        [Header("Death")]
        [SerializeField] Canvas DeathCanvas;
        [SerializeField] TMP_Text DeathText;

        [Header("Victory")]
        [SerializeField] Canvas VictoryCanvas;
        [SerializeField] TMP_Text VictoryTime;
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
                Debug.Log("Destroy");
                Destroy(Canvas.gameObject);
            }
            TimeCounter = 0;
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
            if (ShowTime) {
                TimeCounter += Time.deltaTime;
                ShowTime.text = TimeCounter.ToString("F1");
                
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

        public void LoadMenu(){
            Time.timeScale = 1f;
            PauseCanvas.gameObject.SetActive(false);
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
            LeanTween.value(healthLoss.gameObject, healthLoss.value, health, 1.0f).setEase(LeanTweenType.easeOutCubic).setOnUpdate((float val) =>
            {
               
                healthLoss.value = val;
            });
            
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
        public void Respawn()
        {
            InventoryManager.Instance.Clear();
            LeanTween.alphaCanvas(DeathCanvas.GetComponent<CanvasGroup>(), 0, 1f).setEase(LeanTweenType.easeOutCubic).setDelay(1.5f);
        }
        public void Victory()
        {
            VictoryCanvas.gameObject.SetActive(true);
            VictoryCanvas.GetComponent<CanvasGroup>().alpha = 0;
            LeanTween.alphaCanvas(VictoryCanvas.GetComponent<CanvasGroup>(), 1, 1f).setEase(LeanTweenType.easeOutCubic);
            VictoryTime.text = "Pass Time:\n"+TimeCounter.ToString("F1")+" s";
        }
        public void Restart()
        {
            VictoryCanvas.gameObject.SetActive(false);
            var reload = Simulation.Schedule<LoadScene>();
            reload.SceneName = SceneManager.GetActiveScene().name;
            TimeCounter = 0;
            InventoryManager.Instance.Clear();
        }
        public void UpdateModeNumber() {
            PauseCanvas.GetComponentsInChildren<TMP_Text>()[1].text = "Mode: " + ModeManager.instance.index;
            
        }
    }
}
