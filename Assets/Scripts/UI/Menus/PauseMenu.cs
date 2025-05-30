using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        private bool _isGamePaused;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Escape key pressed");
                if (_isGamePaused)
                    Resume();
                else
                    Pause();
            }
        }

        public void Resume()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            _isGamePaused = false;
        }


        private void Pause()
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