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

        void Awake()
        {
            Fade(intermediateLever, 0, 0);
            Fade(unlockedLever, 0, 0);
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (maskSettings.Value.currentMask.canInteractWithObjects)
                {
                    leverCollider.enabled = false;
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
