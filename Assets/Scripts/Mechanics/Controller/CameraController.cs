using Platformer.Core;
using UnityEngine;
using Cinemachine;
using UnityEditor.Experimental.GraphView;

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
        private CinemachineBasicMultiChannelPerlin shake;
        void OnEnable()
        {
            Current_camera.Priority = 10;
            if (Instance != null && Instance != this)
            {
                Instance.Current_camera = Current_camera;
                Instance.shake = Current_camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                Destroy(this);
            }
            else { 
                Instance = this;
                shake = Current_camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
            
            
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
        public void ShakeCamera(float amp, float frequency, float duration) {
            
            shake.m_AmplitudeGain =amp;
            shake.m_FrequencyGain = frequency;
            shakeCounter = duration;
        }
        public void Update()
        {
            if (shakeCounter > 0) { 
                shakeCounter -= Time.deltaTime;

            }
            else
            {
                shakeCounter = 0f;
                if(shake)
                shake.m_AmplitudeGain = 0;
            }
            
        }
    }
}

