using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BasePlayer : MonoBehaviour
{
    protected virtual void Death()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("EndMenu");
    }
}
