using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManaging : MonoBehaviour
{
    public void StartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void PlayScene()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void EndMenu()
    {
        SceneManager.LoadScene("EndMenu");
    }
}
