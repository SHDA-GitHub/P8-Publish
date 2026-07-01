using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private bool currentlyPaused = false;
    private InputSystem_Actions controls;
    [SerializeField] private LevelUpUI lvlUpUI;
    [SerializeField] private GameObject pausePanel;

    private void Awake()
    {
        if (lvlUpUI == null)
        {
            lvlUpUI = FindFirstObjectByType<LevelUpUI>();
        }

        controls = new InputSystem_Actions();

        controls.UI.Submit.performed += Pause;
    }

    private void Pause(InputAction.CallbackContext context)
    {
        currentlyPaused = !currentlyPaused;
    }

    void Update()
    {
        if (currentlyPaused == true && lvlUpUI.levelUpUIOpen == false)
        {
            Debug.Log("Paused");
            OpenPauseUI();
        }
        else if (currentlyPaused == false && lvlUpUI.levelUpUIOpen == false)
        {
            Debug.Log("Unpaused");
            ClosePauseUI();
        }
    }

    public void OpenPauseUI()
    {
        Cursor.lockState = CursorLockMode.None;

        pausePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ClosePauseUI()
    {
        Cursor.lockState = CursorLockMode.Locked;

        pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }
}
