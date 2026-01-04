using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    
    public static SettingsPanel instance;
    public GameObject sliderCamera;
    public GameObject fpsCounter;
    
    public GameObject _colorTour;
    public GameObject _sliderCamera;
    public GameObject _fpsCounter;


    public void Start()
    {
        instance = this;
        
        this.gameObject.SetActive(false);
    }
    
    public void Update()
    {
        
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
                ChessGameManager.instance.HeadColor.gameObject.SetActive(true);
                ChessGameManager.instance.showTour = true;
            }
        }
        else
        {
            if (ChessGameManager.instance.showTour == true)
            {
                ChessGameManager.instance.HeadColor.gameObject.SetActive(false);
                ChessGameManager.instance.showTour = false;
            }
        }
    }



}

