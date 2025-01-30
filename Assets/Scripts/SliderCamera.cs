/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderCamera : MonoBehaviour
{
    public Camera mainCamera; // Kamera do kontrolowania
    public Slider slider; // Slider, który bêdzie zmieniaæ pozycjê kamery
    public float[] zTab = { -7f, 4f};
    public float[] yTab = { 2f, 20f};

    void Start()
    {
        // Ustawienie wartoœci min i max slidera na -90 do 90
        slider.minValue = 0f;
        slider.maxValue = 1f;

        // Ustawienie wartoœci pocz¹tkowej slidera (œrednia wysokoœæ kamery)
        slider.value = 1f;

        // Dodajemy listener, który bêdzie reagowa³ na zmianê wartoœci slidera
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // Funkcja wywo³ywana przy zmianie wartoœci slidera
    void OnSliderValueChanged(float value)
    {
        
        // Ustawiamy now¹ pozycjê kamery na osi Y, pozostawiaj¹c inne osie bez zmian
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, (value*(yTab[1]-yTab[0]))+yTab[0], (value * (zTab[1] - zTab[0])) + zTab[0]);
    }
}
*/