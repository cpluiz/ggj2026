using UnityEngine;
using DG.Tweening;
using cpluiz.GameEventSystem;

namespace cpluiz.Maskformer.Environment
{
    public class DoorButton : MonoBehaviour
    {
        [SerializeField] protected Collider2D buttonCollider;
        [SerializeField] protected SpriteRenderer lockedButton;
        [SerializeField] protected SpriteRenderer unlockedButton;
        [SerializeField] protected GameEvent unlockDoorEvent;
        [SerializeField] private CharacterMaskSettingGameVariable maskSettings;
        private bool isTouchingPlayer;
        private bool alreadyInteracted;

        void Awake()
        {
            Fade(unlockedButton, 0, 0);
        }
        void OnEnable()
        {
            maskSettings.OnValueChanged.AddListener(MaskChanged);
        }
        void OnDisable()
        {
            maskSettings.OnValueChanged.RemoveListener(MaskChanged);
        }

        private void MaskChanged()
        {
            if(isTouchingPlayer && !alreadyInteracted)
            {
                UnlockDoor();
            }
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isTouchingPlayer = true;    
                UnlockDoor();
            }
        }
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isTouchingPlayer = false;
            }
        }
        protected void Fade(SpriteRenderer sprite, float targetAlpha, float duration = 0.5f)
        {
            sprite.DOFade(targetAlpha, duration);
        }
        public void UnlockDoor()
        {
            if(!maskSettings.Value.currentMask.canInteractWithObjects) return;
            alreadyInteracted = true;
            buttonCollider.enabled = false;

            lockedButton.DOFade(0, 0.5f);
            unlockDoorEvent.Raise(false);
        }
    }
}
