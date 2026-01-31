using UnityEngine;
using cpluiz.Maskformer.Player;
using cpluiz.GameEventSystem;

namespace cpluiz.Maskformer.Environment
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PushableObject : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D rb;
        [SerializeField] private CharacterMaskSettingGameVariable maskSettings;
        private bool isPushing;
        public void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (maskSettings.Value.currentMask.canPushObjects)
                {
                    isPushing = true;
                    rb.bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }
        void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && isPushing)
            {
                DisablePushableState();
            }
        }

        void LateUpdate()
        {
            if(isPushing && !maskSettings.Value.currentMask.canPushObjects)
            {
                DisablePushableState();
            }
        }

        protected void DisablePushableState()
        {
            isPushing = false;
            rb.angularVelocity = 0;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

    }
}
