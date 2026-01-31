using DG.Tweening;
using UnityEngine;

namespace cpluiz.Maskformer.HUD
{
    public class TutorialBubble : MonoBehaviour
    {
        protected SpriteRenderer[] tutorialSprites;
        void Awake()
        {
            tutorialSprites = GetComponentsInChildren<SpriteRenderer>();
            FadeAll(0, 0);
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            FadeAll(1, 0.5f);
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            FadeAll(0, 0.5f);
        }

        protected void FadeAll(float targetAlpha, float duration)
        {
            foreach(SpriteRenderer sprite in tutorialSprites)
            {
                sprite.DOFade(targetAlpha, duration);
            }
        }
    }
}
