using UnityEngine;
using UnityEngine.SceneManagement;

namespace cpluiz.Maskformer
{
    public class MainMenuController : MonoBehaviour
    {
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
