using UnityEngine;
using cpluiz.GameEventSystem;
using cpluiz.Maskformer;
using UnityEngine.UI;

namespace cpluiz.Maskformer.HUD
{
    public class PlayerHUDController : MonoBehaviour
    {
        [SerializeField] private CharacterMaskSettingGameVariable playerCurrentMask;
        [SerializeField] private Image currentMaskImage;
        [SerializeField] private Image previousMaskImage;
        [SerializeField] private Image nextMaskImage;

        void Awake()
        {
            playerCurrentMask.OnValueChanged.AddListener(UpdateSelectedMask);
            UpdateSelectedMask();
        }
        void OnDestroy()
        {
            playerCurrentMask.OnValueChanged.RemoveListener(UpdateSelectedMask);
        }

        private void UpdateSelectedMask()
        {
            currentMaskImage.sprite = playerCurrentMask.Value.currentMask.maskSprite;
            previousMaskImage.gameObject.SetActive(playerCurrentMask.Value.availableMasks.Length > 2);
            nextMaskImage.gameObject.SetActive(playerCurrentMask.Value.availableMasks.Length >= 2);
            if(playerCurrentMask.Value.availableMasks.Length >= 2)
            {
                nextMaskImage.sprite = playerCurrentMask.Value.availableMasks[(playerCurrentMask.Value.currentSelectedMask + 1) % playerCurrentMask.Value.availableMasks.Length].maskSprite;
                if(playerCurrentMask.Value.availableMasks.Length > 2)
                previousMaskImage.sprite = playerCurrentMask.Value.availableMasks[(playerCurrentMask.Value.currentSelectedMask - 1 + playerCurrentMask.Value.availableMasks.Length) % playerCurrentMask.Value.availableMasks.Length].maskSprite;
            }
        }
    }
}
