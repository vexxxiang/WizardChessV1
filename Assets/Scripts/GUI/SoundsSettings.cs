using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundsSettings : MonoBehaviour
{
    public static SoundsSettings instance;
    public Slider sliderVolumeMoving;
    public Slider sliderVolumeAtackVoice;
    public Slider sliderVolumeDestruction;
    public AudioSource movingAudioSource, speakingAudioSource, destructionAudioSource;
    public GameObject saveLoad;


   public float sliderVolumeMovingValue, sliderVolumeAtackVoiceValue,sliderVolumeDestructionValue;


    public void Start()
    {

        saveLoad.GetComponent<SaveLoad>().LoadData();
        instance = this;
        sliderVolumeMoving.onValueChanged.AddListener(delegate { UpdateVolume(sliderVolumeMovingValue, "sliderVolumeMoving"); }); ;
        sliderVolumeAtackVoice.onValueChanged.AddListener(delegate { UpdateVolume(sliderVolumeAtackVoiceValue, "sliderVolumeAtackVoice"); }); ;
        sliderVolumeDestruction.onValueChanged.AddListener(delegate { UpdateVolume(sliderVolumeDestructionValue, "sliderVolumeDestruction"); }); ;

        DontDestroyOnLoad(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        sliderVolumeMovingValue = sliderVolumeMoving.value;
        sliderVolumeAtackVoiceValue = sliderVolumeAtackVoice.value;
        sliderVolumeDestructionValue = sliderVolumeDestruction.value;



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
        else if (what == "sliderVolumeDestruction")
        {
            destructionAudioSource.volume = value;
        }
        

    }

    public void openSettingsPanel()
    {
        
        
        this.gameObject.SetActive(true);
        ChessBoard.instance.selecting = false;

    }
    public void closeSettingsPanel()
    {
        
        saveLoad.GetComponent<SaveLoad>().SaveData();
        this.gameObject.SetActive(false);
        ChessBoard.instance.selecting = true;

    }

    public void LoadSettings(float moving, float atackVoice, float destruction)
    {
        Debug.Log(moving + " " + atackVoice);
        sliderVolumeMoving.value = moving;
        sliderVolumeAtackVoice.value = atackVoice;
        sliderVolumeDestruction.value = destruction;
        UpdateVolume(moving, "sliderVolumeMoving");
        UpdateVolume(atackVoice, "sliderVolumeAtackVoice");
        UpdateVolume(destruction, "sliderVolumeDestruction");
        

    }
}
