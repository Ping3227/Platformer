using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour
    {
        [SerializeField] Item ItemNeed;
        private bool NeedItem=false;
        //[SerializeField] 
        [SerializeField] GameObject InteractionHint;
        [SerializeField] GameObject InteractionDialog;
        [SerializeField] Vector3 Offset;
        private GameObject TheHint;
        private GameObject TheDialog;
        [SerializeField] KeyCode InteractionKey;
        [SerializeField] InteractActor[] InteractionObjects; // need interface here 
        private bool IsInteractable;
        private void Awake()
        {
            TheHint = Instantiate(InteractionHint, transform.position+Offset, Quaternion.identity);
            
            TheHint.GetComponentInChildren<TMP_Text>().text = InteractionKey.ToString();
            
            TheHint.SetActive(false);
            if (ItemNeed) { 
                NeedItem = true;
                TheDialog = Instantiate(InteractionDialog, transform.position + Offset, Quaternion.identity);
                TheDialog.GetComponentInChildren<TMP_Text>().text = "Need " + ItemNeed.Name;
                TheDialog.SetActive(false);
            }
            
        }
        /// <summary>
        /// Create visual hint for player to interact with object
        /// </summary>
        /// <param name="collision"></param>
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {
                if (NeedItem) {
                    if (!InventoryManager.Instance.Contain(ItemNeed)) {
                        TheDialog.SetActive(true);
                        return;
                    }
                        
                }
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
                if (NeedItem) {
                    if (!InventoryManager.Instance.Contain(ItemNeed)) {
                        TheDialog.SetActive(false);
                        return;
                    }
                        
                }
                TheHint.SetActive(false);
                IsInteractable = false;
            }
                
        }
        void Update(){
            if (IsInteractable &&Input.GetKeyDown(InteractionKey)){
                
                foreach (InteractActor actioner in InteractionObjects){    
                    actioner.Action();
                    Debug.Log(actioner.name);    
                }
            }
            
        }
    }


    
}