using UnityEngine;
using cpluiz.Maskformer.Player;
using cpluiz.GameEventSystem;
using System.ComponentModel;
using System.Collections.Generic;

namespace cpluiz.Maskformer.Environment
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PushableObject : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D rb;
        [SerializeField] private CharacterMaskSettingGameVariable maskSettings;
        // TODO     Change isPushing to a GameVariable to play pushing SFX
        //          or play SFX with this script/object
        [SerializeField, ReadOnly(true)] private bool isPushing;
        [SerializeField, ReadOnly(true)] private bool isTouchingPlayer;
        protected List<Collider2D> contactPoints = new();
        protected int playerContactPoints = 0;
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
            if(isTouchingPlayer)
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
            if (collision.gameObject.CompareTag("Player"))
            {
                CheckIfIsCollidingWithPlayer();
            }
        }

        void LateUpdate()
        {
            if(!isPushing) return;
            isPushing &= isTouchingPlayer;
            if (!isPushing)
            {
                rb.angularVelocity = 0;
                rb.linearVelocity = Vector2.zero;
            }
        }
        void CheckIfIsCollidingWithPlayer()
        {
            rb.GetContacts(contactPoints);
            playerContactPoints = 0;
            foreach(Collider2D collider in contactPoints)
            {
                if(collider.gameObject.CompareTag("Player"))
                    playerContactPoints ++;
            }
            isTouchingPlayer = playerContactPoints > 0;
        }
        void FixedUpdate()
        {
            if (!isPushing)
            {
                rb.angularVelocity = 0;
                rb.linearVelocityX = 0;
            }
        }

        protected void TogglePushState()
        {
            if(!isTouchingPlayer) return;
            rb.bodyType = maskSettings.Value.currentMask.canPushObjects ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        }

    }
}
