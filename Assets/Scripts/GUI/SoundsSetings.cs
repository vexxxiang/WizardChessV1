using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundsSetings : MonoBehaviour
{
    public static SoundsSetings instance;
    public Slider sliderVolumeMoving;
    public Slider sliderVolumeAtackVoice;
    public AudioSource movingAudioSource, speakingAudioSource;
    public GameObject saveLoad;


   public float sliderVolumeMovingValue, sliderVolumeAtackVoiceValue;


    public void Start()
    {

        saveLoad.GetComponent<SaveLoad>().LoadData();
        instance = this;
        sliderVolumeMoving.onValueChanged.AddListener(delegate { UpdateVolume(sliderVolumeMovingValue, "sliderVolumeMoving"); }); ;
        sliderVolumeAtackVoice.onValueChanged.AddListener(delegate { UpdateVolume(sliderVolumeAtackVoiceValue, "sliderVolumeAtackVoice"); }); ;

        DontDestroyOnLoad(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        sliderVolumeMovingValue = sliderVolumeMoving.value;
        sliderVolumeAtackVoiceValue = sliderVolumeAtackVoice.value;



    }
    public void UpdateVolume(float value, string what)
    {
        if (what == "sliderVolumeMoving")
        {
            movingAudioSource.volume = value;
        }
        else if (what == "sliderVolumeAtackVoice")
        {
            speakingAudioSource.volume = value;
        }
        saveLoad.GetComponent<SaveLoad>().SaveData();


    }

    public void LoadSettings(float moving, float atackVoice)
    {
        Debug.Log(moving + " " + atackVoice);
        sliderVolumeMoving.value = moving;
        sliderVolumeAtackVoice.value = atackVoice;
        UpdateVolume(moving, "sliderVolumeMoving");
        UpdateVolume(atackVoice, "sliderVolumeAtackVoice");
        

    }
}
