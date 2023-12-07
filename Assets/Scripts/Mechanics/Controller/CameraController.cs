using Platformer.Core;
using UnityEngine;
using Cinemachine;


namespace Platformer.Mechanics
{
    /// <summary>
    /// Tick the simulation.
    /// </summary>
    public class CamearaController : MonoBehaviour
    {
        public CinemachineVirtualCamera Current_camera;
        public static CamearaController Instance { get; private set; }
        private float shakeCounter = 0f;
        private int ShakePriority = -1;
        private CinemachineBasicMultiChannelPerlin shake;
        void OnEnable()
        {
            
            shake = Current_camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            Instance = this;
            
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }
        public void SwitchCamera(CinemachineVirtualCamera newCamera)
        {
            Current_camera.Priority = 9;
            Current_camera = newCamera;
            Current_camera.Priority = 10;
            shake= Current_camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        public void ShakeCamera(float amp, float frequency, float duration,int priority=0) {
            Debug.Log("ShakeCamera"+ShakePriority+" "+ priority);
            if (priority>=ShakePriority) {
                
                shake.m_AmplitudeGain = amp;
                shake.m_FrequencyGain = frequency;
                ShakePriority= priority;
                shakeCounter = duration;
                Debug.Log(shake.m_AmplitudeGain + " " + shake.m_FrequencyGain);
            }
            
        }
        public void Update()
        {
            if (shakeCounter > 0) { 
                shakeCounter -= Time.deltaTime;
                Debug.Log(shake.m_AmplitudeGain + " " + shake.m_FrequencyGain);
                Debug.Log(Current_camera.name);
            }
            else
            {
                ShakePriority = -1;
                shakeCounter = 0f;
                if(shake)shake.m_AmplitudeGain = 0;
            }
            
        }
    }
}

