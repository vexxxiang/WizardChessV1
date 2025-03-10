using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public GameObject soundsSystem;
    public GameObject sliderCamera;
    public GameObject fpsCounter;

    public GameObject _volumeSlider;
    public GameObject _colorTour;
    public GameObject _sliderCamera;
    public GameObject _fpsCounter;

    public GameObject panelSettings;

    public void Menu()
    {
        if (panelSettings.activeSelf == true) { panelSettings.SetActive(false); }
        else if (panelSettings.activeSelf == false) { panelSettings.SetActive(true); }
    }

    public void Update()
    {

        if (_volumeSlider.GetComponent<Toggle>().isOn)
        {
            if (soundsSystem.activeSelf == false)
            {
                soundsSystem.SetActive(true);
            }
        }
        else
        {
            if (soundsSystem.activeSelf == true)
            {
                soundsSystem.SetActive(false);
            }
        }



        if (_sliderCamera.GetComponent<Toggle>().isOn)
        {
            if (sliderCamera.activeSelf == false)
            {
                sliderCamera.SetActive(true);
            }
        }
        else
        {
            if (sliderCamera.activeSelf == true)
            {
                sliderCamera.SetActive(false);
            }
        }



        if (_fpsCounter.GetComponent<Toggle>().isOn)
        {
            if (fpsCounter.activeSelf == false)
            {
                fpsCounter.SetActive(true);
            }
        }
        else
        {
            if (fpsCounter.activeSelf == true)
            {
                fpsCounter.SetActive(false);
            }
        }



        if (_colorTour.GetComponent<Toggle>().isOn)
        {
            if (ChessGameManager.instance.showTour == false)
            {
                ChessGameManager.instance.showTour = true;
            }
        }
        else
        {
            if (ChessGameManager.instance.showTour == true)
            {
                ChessGameManager.instance.showTour = false;
            }
        }
    }



}

