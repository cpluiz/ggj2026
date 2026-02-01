using UnityEngine;

namespace cpluiz.Maskformer
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayAudioFromEvent : MonoBehaviour
    {
        [SerializeField]
        protected AudioSource audioSource;
        void Awake()
        {
            if(audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }
        public void PlayOneShotAudio(UnityEngine.Object clipParameter)
        {
            if(clipParameter.GetType() != typeof(AudioClip))
            {
                Debug.LogWarning($"Expected AudioClip, received {clipParameter.GetType()}");
            }
            audioSource.PlayOneShot((AudioClip)clipParameter);
        }
    }
}
