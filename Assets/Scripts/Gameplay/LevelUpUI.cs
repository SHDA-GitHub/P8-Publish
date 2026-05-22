using System.Collections.Generic;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject levelUpPanel;

    [SerializeField] private List<GameObject> upgradeOptions = new List<GameObject>();

    private void OnEnable()
    {
        PlayerEXP.OnLevelUp += OpenLevelUpUI;
    }

    private void OnDisable()
    {
        PlayerEXP.OnLevelUp -= OpenLevelUpUI;
    }

    private void Start()
    {
        levelUpPanel.SetActive(false);

        FindUpgradeOptions();
    }

    private void OpenLevelUpUI()
    {
        Cursor.lockState = CursorLockMode.None;
        levelUpPanel.SetActive(true);

        Debug.Log("LEVEL UP!");

        foreach (GameObject upgrade in upgradeOptions)
        {
            Debug.Log("Found Upgrade: " + upgrade.name);
        }
        Time.timeScale = 0f;
    }

    public void CloseLevelUpUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void FindUpgradeOptions()
    {
        upgradeOptions.Clear();

        Transform upgrade1 = levelUpPanel.transform.Find("Upgrade1");
        Transform upgrade2 = levelUpPanel.transform.Find("Upgrade2");
        Transform upgrade3 = levelUpPanel.transform.Find("Upgrade3");

        if (upgrade1 != null)
            upgradeOptions.Add(upgrade1.gameObject);

        if (upgrade2 != null)
            upgradeOptions.Add(upgrade2.gameObject);

        if (upgrade3 != null)
            upgradeOptions.Add(upgrade3.gameObject);
    }
}