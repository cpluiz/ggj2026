using UnityEngine;
using UnityEngine.SceneManagement;

namespace cpluiz.Maskformer
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] protected GameObject ExitButton;
        void Awake()
        {
            #if UNITY_WEBGL
            ExitButton.SetActive(false);
            #endif
        }
        public void GoToLevel(int lvlId)
        {
            SceneManager.LoadScene(lvlId);
        }
        public void Exit()
        {
            Application.Quit();
        }
    }
}
