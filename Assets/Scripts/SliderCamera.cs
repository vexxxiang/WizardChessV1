using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderCamera : MonoBehaviour
{
    public Camera mainCamera; // Kamera do kontrolowania
    public Slider slider; // Slider, kt�ry b�dzie zmienia� pozycj� kamery
    public float minHeight = -90f; // Minimalna wysoko�� kamery (przesuwanie w d�)
    public float maxHeight = 90f;  // Maksymalna wysoko�� kamery (przesuwanie w g�r�)

    void Start()
    {
        // Ustawienie warto�ci min i max slidera na -90 do 90
        slider.minValue = -90f;
        slider.maxValue = 90f;

        // Ustawienie warto�ci pocz�tkowej slidera (�rednia wysoko�� kamery)
        slider.value = mainCamera.transform.position.y;

        // Dodajemy listener, kt�ry b�dzie reagowa� na zmian� warto�ci slidera
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // Funkcja wywo�ywana przy zmianie warto�ci slidera
    void OnSliderValueChanged(float value)
    {
        // Ustawiamy now� pozycj� kamery na osi Y, pozostawiaj�c inne osie bez zmian
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, Mathf.Clamp(value, minHeight, maxHeight), mainCamera.transform.position.z);
    }
}
