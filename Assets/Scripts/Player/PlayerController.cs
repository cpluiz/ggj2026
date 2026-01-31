using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using cpluiz.GameEventSystem;
using System.ComponentModel;
using UnityEngine.SceneManagement;

namespace cpluiz.Maskformer.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private BoolVariable isTouchingGround;
        [SerializeField] private BoolVariable isTouchingWall;
        [SerializeField] private float moveSpeed = 0.01f;
        [SerializeField] private float jumpForce = 1;
        [SerializeField] private CharacterMaskSettingGameVariable maskSettings;
        [SerializeField, ReadOnly(true)] private float horizontalWalkValue;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;
        private int currentFrame;
        private int animationFrames = 20;
        private int currentAnimationFrame;
        private bool canJump;
        [SerializeField] private CurrentPlayerAnimation currentPlayerAnimation;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            maskSettings.Value.currentMask.SetCurrentAnimationSprites(currentPlayerAnimation);
            SceneManager.activeSceneChanged += ClearAllMasks;
        }

        public void HorizontalWalk(InputAction.CallbackContext contextParameter)
        {
            if (contextParameter.phase == InputActionPhase.Performed)
            {
                horizontalWalkValue = maskSettings.Value.currentMask.walkSpeed * contextParameter.ReadValue<Vector2>().x;
            }
            if (contextParameter.phase == InputActionPhase.Canceled)
            {
                horizontalWalkValue = 0;
            }
        }

        public void JumpActionPerformed(InputAction.CallbackContext contextParameter)
        {
            //TODO Jump only if is on ground
            //TODO maybe implement double jump for a specific mask

            canJump = isTouchingGround.Value || (maskSettings.Value.currentMask.canJumpInWalls && isTouchingWall.Value);
            if (contextParameter.phase == InputActionPhase.Started && canJump)
            {
                rb.AddForceY(maskSettings.Value.currentMask.jumpForce * jumpForce, ForceMode2D.Impulse);
            }
        }

        void FixedUpdate()
        {
            transform.position = transform.position + new Vector3(horizontalWalkValue * moveSpeed, 0, 0);
        }
        void LateUpdate()
        {
            CheckIfAnimationHasChanged();
            currentFrame = (currentFrame + 1) % animationFrames;
            if(currentFrame == 0)
            {
                currentAnimationFrame = (currentAnimationFrame + 1) % maskSettings.Value.currentMask.currentAnimationSprites.Length;
                spriteRenderer.sprite = maskSettings.Value.currentMask.currentAnimationSprites[currentAnimationFrame];
            }
        }
        private void CheckIfAnimationHasChanged()
        {
            //TODO make a better job
            if (!isTouchingGround.Value)
            {
                currentPlayerAnimation = CurrentPlayerAnimation.jumping;
                rb.gravityScale = 1 * maskSettings.Value.currentMask.gravityModifier;
            }
            else if(horizontalWalkValue != 0 && currentPlayerAnimation != CurrentPlayerAnimation.walking)
            {
                currentPlayerAnimation = CurrentPlayerAnimation.walking;
            }
            else if (horizontalWalkValue == 0 && currentPlayerAnimation != CurrentPlayerAnimation.idle)
            {
                currentPlayerAnimation = CurrentPlayerAnimation.idle;
            }
            if(horizontalWalkValue != 0)
            {
                spriteRenderer.flipX = horizontalWalkValue < 0;
            }
            if (isTouchingGround.Value)
            {
                rb.gravityScale = 1;
            }
            maskSettings.Value.currentMask.SetCurrentAnimationSprites(currentPlayerAnimation);
        }
        protected void ClearAllMasks(Scene current, Scene next)
        {
            maskSettings.Value = maskSettings.DefaultValue;
        }
        public void ChangeMask(int maskID)
        {
            this.maskSettings.SetMask(maskID);
        }
        public void AddMaskToCollection(CharacterMaskSettings maskSettings)
        {
            this.maskSettings.AddMask(maskSettings);
        }

        public void NextMaskActionPerformed(InputAction.CallbackContext contextParameter)
        {
            if(contextParameter.phase == InputActionPhase.Started)
            {
                NextMask();
            }
        }
        public void PreviousMaskActionPerformed(InputAction.CallbackContext contextParameter)
        {
            if(contextParameter.phase == InputActionPhase.Started)
            {
                PreviousMask();
            }
        }
        public void NextMask()
        {
            maskSettings.NextMask();
        }
        public void PreviousMask()
        {
            maskSettings.PreviousMask();
        }
    }
    
}
