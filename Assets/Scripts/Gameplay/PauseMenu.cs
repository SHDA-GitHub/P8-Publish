using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private bool currentlyPaused = false;
    private InputSystem_Actions controls;
    [SerializeField] private LevelUpUI lvlUpUI;
    [SerializeField] private GameObject pausePanel;

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Awake()
    {
        controls = new InputSystem_Actions();
        controls.UI.Enable();

        if (lvlUpUI == null)
        {
            lvlUpUI = FindFirstObjectByType<LevelUpUI>();
        }

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
            OpenPauseUI();
        }
        else if (currentlyPaused == false && lvlUpUI.levelUpUIOpen == false)
        {
            ClosePauseUI();
        }
    }

    public void OpenPauseUI()
    {
        currentlyPaused = true;

        Debug.Log("Paused");

        Cursor.lockState = CursorLockMode.None;

        pausePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ClosePauseUI()
    {
        currentlyPaused = false;

        Debug.Log("Unpaused");

        Cursor.lockState = CursorLockMode.Locked;

        pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }
}
