using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI; // Dodaj to do obs³ugi Slidera

public class CameraSettings : MonoBehaviour
{
    public Transform boardCenter; // Œrodek planszy
    public float radius = 10f; // Promieñ okrêgu
    public float animationDuration = 2f; // Czas trwania animacji
    public Slider slider; // Slider kontroluj¹cy wysokoœæ kamery

    private float currentAngle = 0f; // Aktualny k¹t kamery
    private bool isAnimating = false;

    void Start()
    {
        Vector3 centerPoint = new Vector3(3.5f, 0f, 3.5f); // Œrodek planszy
        currentAngle = 270f; // Startowy k¹t (kamera za bia³ymi)
        UpdateCameraPosition(currentAngle, centerPoint);

        // Ustawienie listenera slidera
        slider.onValueChanged.AddListener(SetCameraHeight);
    }

    public void RotateAroundBoard()
    {
        if (!isAnimating)
            StartCoroutine(RotateCamera());
    }

    IEnumerator RotateCamera()
    {
        isAnimating = true;

        float elapsedTime = 0f;
        float startAngle = currentAngle;
        float endAngle = currentAngle + 180f;

        Vector3 centerPoint = new Vector3(3.5f, 0f, 3.5f);

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float currentAnimationAngle = Mathf.Lerp(startAngle, endAngle, t);

            UpdateCameraPosition(currentAnimationAngle, centerPoint);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentAngle = endAngle % 360f;
        UpdateCameraPosition(currentAngle, centerPoint);

        isAnimating = false;
    }

    private void UpdateCameraPosition(float angle, Vector3 centerPoint)
    {
        float radianAngle = Mathf.Deg2Rad * angle;
        float cameraHeight = slider.value; // Pobierz wartoœæ wysokoœci z slidera

        Vector3 offset = new Vector3(
            Mathf.Cos(radianAngle) * radius,
            cameraHeight,
            Mathf.Sin(radianAngle) * radius
        );

        Camera.main.transform.position = centerPoint + offset;
        Camera.main.transform.LookAt(centerPoint);
    }

    public void SetCameraHeight(float height)
    {
        // Aktualizuj wysokoœæ kamery, jeœli slider siê zmieni
        Vector3 centerPoint = new Vector3(3.5f, 0f, 3.5f);
        UpdateCameraPosition(currentAngle, centerPoint);
    }
}