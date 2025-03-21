using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



[System.Serializable]
public class SettingsData
{
    public float movementVolume;
    public float atackVoiceVolume;
    public float cameraHeight;
}

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad instance;
    public GameObject soundSystem;
    public GameObject cameraSettings;


    public void Start()
    {

        instance = this;
    }
    public void SaveData()
    {

        SettingsData settingsData = new SettingsData
        {
            movementVolume = soundSystem.GetComponent<SoundsSetings>().sliderVolumeMovingValue,
            atackVoiceVolume = soundSystem.GetComponent<SoundsSetings>().sliderVolumeAtackVoiceValue,
            cameraHeight = cameraSettings.GetComponent<CameraSettings>().heightCamera
        };
        string json = JsonUtility.ToJson(settingsData);

        string path = Application.persistentDataPath + "/settingsData.json";
        File.WriteAllText(path, json);

        //Debug.Log("zapisoano");
        
    }

    public void LoadData()
    {

        string path = Application.persistentDataPath + "/settingsData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            // Sprawdzenie, czy plik nie jest pusty
            if (string.IsNullOrEmpty(json))
            {
                Debug.Log("Plik jest pusty, ustawienia nie mog¹ byæ za³adowane.");
                // Ustawienie domyœlnych wartoœci, gdy plik jest pusty
                soundSystem.GetComponent<SoundsSetings>().LoadSettings(0.5f, 0.5f);
                cameraSettings.GetComponent<CameraSettings>().LoadSettings(0.7f);
            }
            else
            {
                SettingsData settingsData = JsonUtility.FromJson<SettingsData>(json);
                soundSystem.GetComponent<SoundsSetings>().LoadSettings(settingsData.movementVolume, settingsData.atackVoiceVolume);
                cameraSettings.GetComponent<CameraSettings>().LoadSettings(settingsData.cameraHeight);
            }
        }
        else
        {
            Debug.Log("Nie ma zapisanych ustawieñ");
            // Ustawienie domyœlnych wartoœci, gdy plik nie istnieje
            soundSystem.GetComponent<SoundsSetings>().LoadSettings(0.5f, 0.5f);
            cameraSettings.GetComponent<CameraSettings>().LoadSettings(0.7f);
        }

    }
}
