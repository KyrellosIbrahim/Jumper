using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("gameplay");
        if(Time.timeScale == 0f) {
            Time.timeScale = 1f;
        }
    }
}
