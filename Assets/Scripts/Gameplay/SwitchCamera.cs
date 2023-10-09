using UnityEngine;
using Platformer.Mechanics;
using static Platformer.Core.Simulation;
using Cinemachine; 
namespace Platformer.Gameplay
{
    /// <summary>
    /// change camera 
    /// </summary>
    /// <typeparam name="SwitchCamera"></typeparam>
    public class SwitchCamera : Event<SwitchCamera>
    {
        public CinemachineVirtualCamera source;
        public CinemachineVirtualCamera target;
        public override void Execute()
        {
            SwitchCamera(source, target);
            CinemachineVirtualCamera SwitchCamera(CinemachineVirtualCamera source, CinemachineVirtualCamera target)
            {
                source.Priority = 0;
                target.Priority = 10;
                return target;
            }
            
        }
    }
}