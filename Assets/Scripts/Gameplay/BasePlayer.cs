using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BasePlayer : MonoBehaviour
{
    protected virtual void Death()
    {
        SceneManager.LoadScene("PlayerMovementTest");
    }
}
