using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour
    {
        [SerializeField] GameObject InteractionHint;
        [SerializeField] Vector3 Offset;
        private GameObject TheHint;
        [SerializeField] KeyCode InteractionKey;
        [SerializeField] InteractActor[] InteractionObjects; // need interface here 
        private bool IsInteractable;
        private void Awake()
        {
            TheHint = Instantiate(InteractionHint, transform.position+Offset, Quaternion.identity);
            TheHint.GetComponentInChildren<TMP_Text>().text = InteractionKey.ToString();
            TheHint.SetActive(false);
            
        }
        /// <summary>
        /// Create visual hint for player to interact with object
        /// </summary>
        /// <param name="collision"></param>
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {
                TheHint.SetActive(true);
                IsInteractable = true;
            }
        }
        /// <summary>
        /// Destroy visual hint for player to interact with object
        /// </summary>
        /// <param name="collider"></param>
        void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.tag == "Player") {
                TheHint.SetActive(false);
                IsInteractable = false;
            }
                
        }
        void Update()
        {
            if (IsInteractable &&Input.GetKeyDown(InteractionKey))
            {
                Debug.Log("Interact");
                foreach (InteractActor actioner in InteractionObjects)
                {    
                    actioner.Action();
                        
                }
            }
            
        }
    }


    
}