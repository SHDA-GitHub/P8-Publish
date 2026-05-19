using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManaging : MonoBehaviour
{
    public void StartMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void PlayScene()
    {
        SceneManager.LoadScene("Play Scene");
    }

    public void EndMenu()
    {
        SceneManager.LoadScene("End Menu");
    }
}
