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

        void Awake()
        {
            Fade(unlockedButton, 0, 0);
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (maskSettings.Value.currentMask.canInteractWithObjects)
                {
                    buttonCollider.enabled = false;
                    UnlockDoor();
                }
            }
        }
        protected void Fade(SpriteRenderer sprite, float targetAlpha, float duration = 0.5f)
        {
            sprite.DOFade(targetAlpha, duration);
        }
        public void UnlockDoor()
        {
            lockedButton.DOFade(0, 0.5f);
            unlockDoorEvent.Raise(false);
        }
    }
}
