using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManaging : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip Click;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartMenu()
    {
        audioSource.PlayOneShot(Click);
        SceneManager.LoadScene(0);
    }

    public void PlayScene()
    {
        audioSource.PlayOneShot(Click);
        SceneManager.LoadScene(1);
    }

    public void CreditsMenu()
    {
        audioSource.PlayOneShot(Click);
        SceneManager.LoadScene(2);
    }

    public void EndMenu()
    {
        audioSource.PlayOneShot(Click);
        SceneManager.LoadScene(3);
    }
}
