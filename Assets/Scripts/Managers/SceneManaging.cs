using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManaging : MonoBehaviour
{
    public void StartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayScene()
    {
        SceneManager.LoadScene(1);
    }

    public void EndMenu()
    {
        SceneManager.LoadScene(2);
    }
}
