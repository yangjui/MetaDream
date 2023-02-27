using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightManager : Singleton<FlashLightManager>
{
    [SerializeField]
    private GameObject flashLight = null;
    [SerializeField]
    private GameObject uvLight = null;

    [System.NonSerialized]
    public bool uvLightOn = false;

    private bool lightOn = true;

    private void Update()
    {
        if (SettingManager.Instance.isSettingMenuOn == false)
        {
            Light();

            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                lightOn = !lightOn;
                LightOnOff();
            }
        }
    }

    private void Light()
    {
        if (lightOn)
        {
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");
            if (wheelInput > 0)
            {
                flashLight.SetActive(true);
                uvLight.SetActive(false);

                uvLightOn = false;
            }
            else if (wheelInput < 0)
            {
                flashLight.SetActive(false);
                uvLight.SetActive(true);

                uvLightOn = true;
            }
        }
    }

    private void LightOnOff()
    {
        if (lightOn)
        {
            if (uvLightOn)
            {
                uvLight.SetActive(true);
                flashLight.SetActive(false);
            }
            else
            {
                flashLight.SetActive(true);
                uvLight.SetActive(false);
            }
        }
        else
        {
            flashLight.SetActive(false);
            uvLight.SetActive(false);
        }
    }
}