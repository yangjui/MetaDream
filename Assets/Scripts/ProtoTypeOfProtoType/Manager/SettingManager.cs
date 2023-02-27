using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : Singleton<SettingManager>
{
    [SerializeField]
    private GameObject settingMenu;

    [System.NonSerialized]
    public bool isSettingMenuOn = false;

    private void Awake()
    {
        Resume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isSettingMenuOn)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()
    {
        settingMenu.SetActive(false);
        Time.timeScale = 1f;
        isSettingMenuOn = false;
    }

    private void Pause()
    {
        settingMenu.SetActive(true);
        Time.timeScale = 0f;
        isSettingMenuOn = true;
    }
}
