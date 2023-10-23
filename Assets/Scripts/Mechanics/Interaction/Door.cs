using UnityEngine;
namespace Platformer.Mechanics
{
    public class Door : InteractActor
    {
        
        
        private Vector3 StartPosition;
        private Vector3 Endpoisiton;
        [SerializeField] Vector3 MoveToward;
        [SerializeField] float Speed;
        
        private bool IsMoving =false;
        [SerializeField] bool IsClosing = false;
        private void Start()
        {
            StartPosition = transform.position;
            Endpoisiton = StartPosition + MoveToward;
        }
        private void Update()
        {
            if (IsMoving == true) { 
                if (IsClosing)
                {
                    transform.position = Vector2.MoveTowards(transform.position, Endpoisiton, Speed * Time.deltaTime);
                    if (transform.position == Endpoisiton)
                    {
                        IsMoving = false;
                    }
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, StartPosition, Speed * Time.deltaTime);
                    if (transform.position == StartPosition)
                    {
                        IsMoving = false;
                    }
                }
               
            }
        }
        override public void Action() {
            IsMoving = true;
            IsClosing = !IsClosing;
        }
        
    }
}