using UnityEngine;

namespace cpluiz.Maskformer
{
    [CreateAssetMenu(menuName = "CharacterMaskSettings", fileName = "CharacterMaskSettings")]
    public class CharacterMaskSettings : ScriptableObject
    {
        public Sprite maskSprite;
        public Sprite[] idleSprites;
        public Sprite[] walkingSprites;
        public Sprite[] jumpingSprites;
        public Sprite[] currentAnimationSprites;
        public float gravityModifier = 1;
        public float jumpForce;
        public float walkSpeed;
        public bool canPushObjects;
        public bool canJumpInWalls;
        public void SetCurrentAnimationSprites(CurrentPlayerAnimation currentPlayerAnimation)
        {
            switch (currentPlayerAnimation)
            {
                case CurrentPlayerAnimation.walking:
                    currentAnimationSprites = walkingSprites;
                    break;
                case CurrentPlayerAnimation.jumping:
                    currentAnimationSprites = jumpingSprites;
                    break;
                case CurrentPlayerAnimation.idle:
                default:
                    currentAnimationSprites = idleSprites;
                    break;
            }
        }
    }
    public enum CurrentPlayerAnimation
    {
        idle = 0,
        walking = 1,
        jumping = 2,
    }
}
