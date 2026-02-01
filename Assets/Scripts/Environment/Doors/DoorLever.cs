using UnityEngine;
using DG.Tweening;
using cpluiz.GameEventSystem;

namespace cpluiz.Maskformer.Environment
{
    public class DoorLever : MonoBehaviour
    {
        [SerializeField] protected Collider2D leverCollider;
        [SerializeField] protected SpriteRenderer lockedLever;
        [SerializeField] protected SpriteRenderer intermediateLever;
        [SerializeField] protected SpriteRenderer unlockedLever;
        [SerializeField] protected GameEvent exitDoorEvent;
        [SerializeField] private CharacterMaskSettingGameVariable maskSettings;
        [SerializeField] private GameEvent sfxEvent;
        [SerializeField] private AudioClip doorUnlockedAudioClip;
        private bool isTouchingPlayer;
        private bool alreadyInteracted;

        void Awake()
        {
            Fade(intermediateLever, 0, 0);
            Fade(unlockedLever, 0, 0);
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
            leverCollider.enabled = false;

            sfxEvent.Raise(doorUnlockedAudioClip);

            Sequence unlockSequence = DOTween.Sequence();
            unlockSequence.Append(lockedLever.DOFade(0, 0.3f))
            .Join(intermediateLever.DOFade(1, 0.3f))
            .PrependInterval(0.3f)
            .Append(intermediateLever.DOFade(0, 0.3f))
            .Join(unlockedLever.DOFade(1, 0.3f))
            .PrependInterval(0.3f)
            .OnComplete(()=>exitDoorEvent.Raise(false));
        }
    }
}
