using TMPro;
using UnityEngine;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour
    {
        [SerializeField] GameObject InteractionHint;
        private GameObject TheHint;
        [SerializeField] KeyCode InteractionKey;
        [SerializeField] IAction[] InteractionObject; // need interface here 

        private void Awake()
        {
            TheHint = Instantiate(InteractionHint, transform.position, Quaternion.identity);
            TheHint.SetActive(false);
            TheHint.GetComponentInChildren<TMP_Text>().text = InteractionKey.ToString();
        }
        /// <summary>
        /// Create visual hint for player to interact with object
        /// </summary>
        /// <param name="collision"></param>
        void OnTriggerEnter2D(Collider2D collider)
        {
            TheHint.SetActive(true);
        }
        /// <summary>
        /// Destroy visual hint for player to interact with object
        /// </summary>
        /// <param name="collider"></param>
        void OnTriggerExit2D(Collider2D collider)
        {
            TheHint.SetActive(false);
        }
        void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {
                if (Input.GetKeyDown(InteractionKey))
                {
                    foreach (IAction actioner in InteractionObject)
                    {
                        actioner.Action();
                    }
                }
            }
        }
    }
}