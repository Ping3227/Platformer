using UnityEngine;
namespace Platformer.Mechanics
{
    public class Door : InteractActor
    {
        [SerializeField] GameObject DoorObject;
        private float Endpoisiton;
        private float StartPosition;
        private float IsMoving;
        override public void Action() {
            
        }
        
    }
}