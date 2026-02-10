using UnityEngine;
using cpluiz.GameEventSystem;

namespace cpluiz.Maskformer.Player
{
    public class GroundChecker : MonoBehaviour
    {
        public BoolVariable isTouchingGround;
        public FloatVariable groundFriction;
        public LayerMask layersToCheck;
        private float checkRadius = 0.05f;
        private RaycastHit2D ground;

        void FixedUpdate()
        {
            ground = Physics2D.CircleCast(transform.position, checkRadius, Vector2.down, 0, layersToCheck);
            if (ground)
            {
                isTouchingGround.Value = true;
                if(groundFriction)
                    groundFriction.Value = ground.rigidbody.sharedMaterial ? ground.rigidbody.sharedMaterial.friction : 1;
                return;
            }
            isTouchingGround.Value = false;
        }
    }
}
