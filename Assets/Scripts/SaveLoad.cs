using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



[System.Serializable]
public class SettingsData
{
    public float movementVolume;
    public float atackVoiceVolume;
    public float destructionVolume;
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
            movementVolume = soundSystem.GetComponent<SoundsSettings>().sliderVolumeMovingValue,
            atackVoiceVolume = soundSystem.GetComponent<SoundsSettings>().sliderVolumeAtackVoiceValue,
            destructionVolume = soundSystem.GetComponent<SoundsSettings>().sliderVolumeDestructionValue,
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
                Debug.Log("Plik jest pusty, ustawienia nie mog� by� za�adowane.");
                // Ustawienie domy�lnych warto�ci, gdy plik jest pusty
                soundSystem.GetComponent<SoundsSettings>().LoadSettings(0.5f, 0.5f,0.5f);
                cameraSettings.GetComponent<CameraSettings>().LoadSettings(0.7f);
            }
            else
            {
                SettingsData settingsData = JsonUtility.FromJson<SettingsData>(json);
                soundSystem.GetComponent<SoundsSettings>().LoadSettings(settingsData.movementVolume, settingsData.atackVoiceVolume, settingsData.destructionVolume);
                cameraSettings.GetComponent<CameraSettings>().LoadSettings(settingsData.cameraHeight);
            }
        }
        else
        {
            Debug.Log("Nie ma zapisanych ustawie�");
            // Ustawienie domy�lnych warto�ci, gdy plik nie istnieje
            soundSystem.GetComponent<SoundsSettings>().LoadSettings(0.5f, 0.5f,0.5f);
            cameraSettings.GetComponent<CameraSettings>().LoadSettings(0.7f);
        }

        SaveData();

    }
}
