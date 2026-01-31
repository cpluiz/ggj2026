using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace cpluiz.Maskformer.Environment
{
    public class LockedDoor : MonoBehaviour
    {
        public int nextLevelID;
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
            doorCollider.isTrigger = !lockedStatus;
            Fade(lockedStatus ? 1 : 0, 0.5f);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                SceneManager.LoadScene(nextLevelID);
            }
        }
    }
}
