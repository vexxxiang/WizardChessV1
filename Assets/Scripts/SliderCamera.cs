/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderCamera : MonoBehaviour
{
    public Camera mainCamera; // Kamera do kontrolowania
    public Slider slider; // Slider, kt�ry b�dzie zmienia� pozycj� kamery
    public float[] zTab = { -7f, 4f};
    public float[] yTab = { 2f, 20f};

    void Start()
    {
        // Ustawienie warto�ci min i max slidera na -90 do 90
        slider.minValue = 0f;
        slider.maxValue = 1f;

        // Ustawienie warto�ci pocz�tkowej slidera (�rednia wysoko�� kamery)
        slider.value = 1f;

        // Dodajemy listener, kt�ry b�dzie reagowa� na zmian� warto�ci slidera
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // Funkcja wywo�ywana przy zmianie warto�ci slidera
    void OnSliderValueChanged(float value)
    {
        
        // Ustawiamy now� pozycj� kamery na osi Y, pozostawiaj�c inne osie bez zmian
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, (value*(yTab[1]-yTab[0]))+yTab[0], (value * (zTab[1] - zTab[0])) + zTab[0]);
    }
}
*/