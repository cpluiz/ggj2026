using UnityEngine;
using cpluiz.Maskformer;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;

namespace cpluiz.GameEventSystem
{
    [CreateAssetMenu(menuName = "GameVariable/CharacterMaskSettings", fileName = "CharacterMaskSettings")]
    public class CharacterMaskSettingGameVariable : ScriptableVariable<CharacterAvailableMasks>
    {
        public void SetMask(int maskId)
        {
            value.currentSelectedMask = maskId;
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
            ForceUpdate();
        }
        public void NextMask()
        {
            SetMask((value.currentSelectedMask + 1) % value.availableMasks.Length);
        }
        public void PreviousMask()
        {
            SetMask((value.currentSelectedMask - 1 + value.availableMasks.Length) % value.availableMasks.Length);
        }
        public void AddMask(CharacterMaskSettings maskSettings)
        {
            List<CharacterMaskSettings> masks = new List<CharacterMaskSettings>();
            masks.AddRange(value.availableMasks);
            masks.Add(maskSettings);
            value.availableMasks = masks.ToArray();
            SetMask(value.availableMasks.Length -1);
        }
    }
    [System.Serializable]
    public struct CharacterAvailableMasks
    {
        public CharacterMaskSettings defaultMask;
        public CharacterMaskSettings[] availableMasks;
        public int currentSelectedMask;
        public CharacterMaskSettings currentMask
        {
            get{
                if(availableMasks.Length > 0)
                {
                    return availableMasks[currentSelectedMask];
                }
                return defaultMask;
            }
        }
        
    }
}
