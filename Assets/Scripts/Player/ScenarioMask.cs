using UnityEngine;

namespace cpluiz.Maskformer.Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ScenarioMask : MonoBehaviour
    {
        [SerializeField] private CharacterMaskSettings maskSettings;
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
                collision.GetComponent<PlayerController>().AddMaskToCollection(maskSettings);
                Destroy(transform.gameObject);
            }
        }
    }
}
