using UnityEngine;
using UnityEngine.InputSystem;
using cpluiz.GameEventSystem;
using UnityEngine.SceneManagement;
using Unity.Collections;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;

namespace cpluiz.Maskformer.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Configuration")]
        [SerializeField] private float moveSpeed = 0.01f;
        [SerializeField] private float jumpForce = 1;
        [Header("SFX Settings")]
        //TODO - Change sound list by type of floor
        [SerializeField] private AudioClip[] stepClip;
        [SerializeField] private AudioClip maskChangedSFX;
        [Header("Debug Information")]
        [SerializeField, ReadOnly] private CurrentPlayerAnimation currentPlayerAnimation;
        [SerializeField, ReadOnly] private float horizontalWalkValue;
        [Header("GameVariables")]
        [SerializeField] private CharacterMaskSettingGameVariable maskSettings;
        [SerializeField] private BoolVariable isTouchingGround;
        [SerializeField] private BoolVariable isTouchingWall;
        [Header("GameEvents")]
        [SerializeField] private GameEvent sfxEvent;
        [SerializeField] private GameEvent setAsCameraTarget;

        #region Private and Protected Fields
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;
        private int currentFrame;
        private int animationFrames = 20;
        private int currentAnimationFrame;
        private bool canJump;
        private int preivousMaskID;
        #endregion

        #region Unity Functions
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            maskSettings.OnValueChanged.AddListener(MaskUpdated);
            maskSettings.Value.currentMask.SetCurrentAnimationSprites(currentPlayerAnimation);
            SceneManager.activeSceneChanged += ClearAllMasks;
        }
        void OnDestroy()
        {
            maskSettings.OnValueChanged.RemoveListener(MaskUpdated);
        }
        void Start()
        {
            setAsCameraTarget?.Raise(transform);
        }
        void Update()
        {
            // rb.linearVelocityX = 0;
        }
        void FixedUpdate()
        {
            transform.position = transform.position + Vector3.right * maskSettings.Value.currentMask.walkSpeed * horizontalWalkValue * moveSpeed;
        }
        void LateUpdate()
        {
            CheckIfAnimationHasChanged();
            currentFrame = (currentFrame + 1) % animationFrames;
            if(currentFrame == 0)
            {
                currentAnimationFrame = (currentAnimationFrame + 1) % maskSettings.Value.currentMask.currentAnimationSprites.Length;
                spriteRenderer.sprite = maskSettings.Value.currentMask.currentAnimationSprites[currentAnimationFrame];
                //TODO Implement better controll for walking sound effect
                if (isTouchingGround.Value && stepClip.Length > 0 && horizontalWalkValue != 0 && currentAnimationFrame == 0)
                {
                    sfxEvent.Raise(stepClip[Random.Range(0, stepClip.Length)]);
                }
            }
            // rb.linearVelocityX = 0;
        }
        #endregion Unity Functions

        #region Event Listeners
        public void WalkActionPerformed(InputAction.CallbackContext contextParameter)
        {
            if (contextParameter.phase == InputActionPhase.Performed)
            {
                horizontalWalkValue = contextParameter.ReadValue<Vector2>().x;
            }
            if (contextParameter.phase == InputActionPhase.Canceled)
            {
                horizontalWalkValue = 0;
            }
        }

        public void JumpActionPerformed(InputAction.CallbackContext contextParameter)
        {
            //TODO maybe implement double jump for a specific mask
            canJump = isTouchingGround.Value || (maskSettings.Value.currentMask.canJumpInWalls && isTouchingWall.Value);
            if (contextParameter.phase == InputActionPhase.Started && canJump)
            {
                rb.AddForceY(maskSettings.Value.currentMask.jumpForce * jumpForce, ForceMode2D.Impulse);
                sfxEvent.Raise(maskSettings.Value.currentMask.jumpSFX);
            }
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
        #endregion Event Listeners
        
        private void MaskUpdated()
        {
            if(preivousMaskID == maskSettings.Value.currentSelectedMask) return;
            
            preivousMaskID = maskSettings.Value.currentSelectedMask;
            sfxEvent.Raise(maskChangedSFX);
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
        public void NextMask()
        {
            maskSettings.NextMask();
        }
        public void PreviousMask()
        {
            maskSettings.PreviousMask();
        }
        #region Async/Coroutine
        
        #endregion Async/Coroutine
    }
    
}
