using UnityEngine;
using cpluiz.GameEventSystem;

namespace cpluiz.Maskformer.Player
{
    public class GroundChecker : MonoBehaviour
    {
        public BoolVariable isTouchingGround;
        public LayerMask layersToCheck;
        private float checkRadius = 0.05f;

        void FixedUpdate()
        {
            isTouchingGround.Value = Physics2D.CircleCast(transform.position, checkRadius, Vector2.down, 0, layersToCheck);
        }
    }
}
