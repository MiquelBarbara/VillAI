using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private string nameMenuScene;
        [SerializeField] private string nameEssentialScene;
        [SerializeField] private string nameNewGameStartScene;

        public void StartNewGame()
        {
            Debug.Log("Button Clicked!");
            SceneManager.UnloadSceneAsync(nameMenuScene);
            SceneManager.LoadScene(nameNewGameStartScene, LoadSceneMode.Single);
            SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Additive);
        }


        public void ExitGame()
        {
            Application.Quit();
        }
    }
}