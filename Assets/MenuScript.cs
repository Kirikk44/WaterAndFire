using UnityEngine;
using UnityEngine.SceneManagement;

public class SplitScreenRestartManager : MonoBehaviour
{
    private bool _player1Pressed = false;
    private bool _player2Pressed = false;

    public void OnPlayer1Restart()
    {
        _player1Pressed = true;
        TryRestart();
    }

    public void OnPlayer2Restart()
    {
        _player2Pressed = true;
        TryRestart();
    }


    private void TryRestart()
    {
        if (_player1Pressed && _player2Pressed)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void OnPlayer1Exit()
    {
        QuitGame();
    }


    public void OnPlayer2Exit()
    {
        QuitGame();
    }


    private void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}