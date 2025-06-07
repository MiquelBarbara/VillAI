using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    /// <summary>
    /// PauseMenu is a MonoBehaviour that manages the game's pause menu,
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        public bool _isGamePaused = false;
        private void Start()
        {
            gameObject.SetActive(false);
        }
        
        public void Resume()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            _isGamePaused = false;
        }
        
        public void Toggle()
        {
            if (_isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
        public void Pause()
        {
            gameObject.SetActive(true);
            Time.timeScale = 0f;
            _isGamePaused = true;
        }

        public void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenuScene");
            SceneManager.UnloadSceneAsync("Essentials");
            SceneManager.UnloadSceneAsync("Village");
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}