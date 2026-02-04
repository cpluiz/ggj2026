using UnityEngine;
using cpluiz.GameEventSystem;
using Unity.Cinemachine;

namespace cpluiz.Maskformer.Helpers
{
    [RequireComponent(typeof(GameEventListenerTransform), typeof(CinemachineCamera))]
    public class CinemachineCameraTarget : MonoBehaviour
    {
        private CinemachineCamera cinemachineCamera;
        void Awake()
        {
            cinemachineCamera = GetComponent<CinemachineCamera>();
            if(cinemachineCamera == null)
                gameObject.SetActive(false);
        }
        public void SetCameraTarget(Transform targetTransform)
        {
            cinemachineCamera.Target.TrackingTarget = targetTransform;
        }
    }
}