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


    public void Start()
    {
        instance = this;
}
    // Update is called once per frame
    void Update()
    {
        sliderVolumeMoving.onValueChanged.AddListener(delegate { UpdateVolume(sliderVolumeMoving.value, "sliderVolumeMoving"); }); ;
        sliderVolumeAtackVoice.onValueChanged.AddListener(delegate { UpdateVolume(sliderVolumeAtackVoice.value, "sliderVolumeAtackVoice"); }); ;
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
    }
}
