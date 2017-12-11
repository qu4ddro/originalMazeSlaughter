using UnityEngine;

namespace Assets.Scripts
{
    public class MoveModel
    {
        public bool CanMove { get; set; }
        public bool IsMoving { get; set; }
        
        public RaycastHit2D Hit { get; set; }
    }
}