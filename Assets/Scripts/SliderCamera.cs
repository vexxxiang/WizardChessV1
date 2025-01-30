using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderCamera : MonoBehaviour
{
    public Camera mainCamera; // Kamera do kontrolowania
    public Slider slider; // Slider, który bêdzie zmieniaæ pozycjê kamery
    public float minHeight = -90f; // Minimalna wysokoœæ kamery (przesuwanie w dó³)
    public float maxHeight = 90f;  // Maksymalna wysokoœæ kamery (przesuwanie w górê)

    void Start()
    {
        // Ustawienie wartoœci min i max slidera na -90 do 90
        slider.minValue = -90f;
        slider.maxValue = 90f;

        // Ustawienie wartoœci pocz¹tkowej slidera (œrednia wysokoœæ kamery)
        slider.value = mainCamera.transform.position.y;

        // Dodajemy listener, który bêdzie reagowa³ na zmianê wartoœci slidera
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // Funkcja wywo³ywana przy zmianie wartoœci slidera
    void OnSliderValueChanged(float value)
    {
        // Ustawiamy now¹ pozycjê kamery na osi Y, pozostawiaj¹c inne osie bez zmian
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, Mathf.Clamp(value, minHeight, maxHeight), mainCamera.transform.position.z);
    }
}
