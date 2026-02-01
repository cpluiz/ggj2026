using UnityEngine;
using cpluiz.Maskformer.Player;
using cpluiz.GameEventSystem;
using System.ComponentModel;

namespace cpluiz.Maskformer.Environment
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PushableObject : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D rb;
        [SerializeField] private CharacterMaskSettingGameVariable maskSettings;
        [SerializeField, ReadOnly(true)] private bool isPushing;
        [SerializeField, ReadOnly(true)] private bool isTouchingPlayer;
        public void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
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
            TogglePushState();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isTouchingPlayer = true;
                TogglePushState();
            }
        }
        void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && isPushing)
            {
                isTouchingPlayer = false;
                TogglePushState();
            }
        }

        protected void TogglePushState()
        {
            if(!isTouchingPlayer && !isPushing) return;
            
            isPushing = isTouchingPlayer && maskSettings.Value.currentMask.canPushObjects;
            rb.bodyType = isPushing ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
            if (!isPushing)
            {
                rb.angularVelocity = 0;
                rb.linearVelocity = Vector2.zero;
            }
        }

    }
}
