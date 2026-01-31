using DG.Tweening;
using UnityEngine;

namespace cpluiz.Maskformer.Environment
{
    public class LockedDoor : MonoBehaviour
    {
        [SerializeField] protected Collider2D doorCollider;
        [SerializeField] protected Transform closedDoorObject;
        protected SpriteRenderer[] closedDoorSprites;

        void Awake()
        {
            closedDoorSprites = closedDoorObject.GetComponentsInChildren<SpriteRenderer>();
        }
        protected void Fade(float targetAlpha, float fadeDuration = 0.5f)
        {
            foreach(SpriteRenderer sprite in closedDoorSprites)
            {
                sprite.DOFade(targetAlpha, fadeDuration);
            }
        }
        public void ToggleLockedStatus(bool lockedStatus)
        {
            doorCollider.enabled = lockedStatus;
            Fade(lockedStatus ? 1 : 0, 0.5f);
        }
    }
}
