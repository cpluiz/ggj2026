using UnityEngine;
using cpluiz.GameEventSystem;

namespace cpluiz.Maskformer.Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ScenarioMask : MonoBehaviour
    {
        [SerializeField] private CharacterMaskSettings maskSettings;
        [SerializeField] private GameEvent sfxEvent;
        [SerializeField] private AudioClip maskSFXAudioClip;
        private SpriteRenderer maskSpriteRenderer;

        void Awake()
        {
            maskSpriteRenderer = GetComponent<SpriteRenderer>();
            maskSpriteRenderer.sprite = maskSettings.maskSprite;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                sfxEvent.Raise(maskSFXAudioClip);
                collision.GetComponent<PlayerController>().AddMaskToCollection(maskSettings);
                Destroy(transform.gameObject);
            }
        }
    }
}
