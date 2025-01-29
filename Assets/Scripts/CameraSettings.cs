using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    public Transform boardCenter; // Œrodek planszy (ustaw na obiekt reprezentuj¹cy œrodek planszy)
    public float radius = 10f; // Promieñ pó³okrêgu
    public float animationDuration = 2f; // Czas trwania animacji
    public ChessGameManager gameManager; // Odniesienie do ChessGameManager

    private Vector3 initialPosition; // Pocz¹tkowa pozycja kamery
    private Quaternion initialRotation; // Pocz¹tkowa rotacja kamery
    private bool isAnimating = false;

    void Start()
    {
        // Ustawienie pocz¹tkowej pozycji kamery
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void RotateAroundBoard()
    {
        if (!isAnimating)
            StartCoroutine(RotateCamera());
    }


    IEnumerator RotateCamera()
    {
        float duration = 1f; // Czas trwania animacji
        float elapsedTime = 0f;

        Vector3 centerPoint = new Vector3(3.5f, 0f, 3.5f); // Œrodek planszy
        float radius = 10f; // Promieñ obrotu kamery

        // Pozycja pocz¹tkowa - Kamera zaczyna od ty³u
        Vector3 startPosition = Camera.main.transform.position;
        float startAngle = Mathf.Atan2(startPosition.z - centerPoint.z, startPosition.x - centerPoint.x) * Mathf.Rad2Deg;

        // Koñcowy k¹t w zale¿noœci od tury
        float endAngle = gameManager.isWhiteTurn ? -90f : 90f; // Kamera zaczyna od ty³u, zatem:
                                                   // - Dla bia³ych: od ty³u, koñczy za bia³ymi.
                                                   // - Dla czarnych: od ty³u, koñczy za czarnymi.

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // Progres animacji
            float currentAngle = Mathf.Lerp(startAngle, endAngle, t); // Aktualny k¹t

            // Obliczenie pozycji kamery na okrêgu
            float radianAngle = Mathf.Deg2Rad * currentAngle;
            Vector3 offset = new Vector3(Mathf.Cos(radianAngle) * radius, 11f, Mathf.Sin(radianAngle) * radius); // Zmiana wysokoœci kamery o 1.5f

            Camera.main.transform.position = centerPoint + offset; // Pozycja kamery
            Camera.main.transform.LookAt(centerPoint); // Ustawienie kamery, by patrzy³a na planszê

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ustawienie koñcowej pozycji i rotacji
        float finalRadianAngle = Mathf.Deg2Rad * endAngle;
        Vector3 finalOffset = new Vector3(Mathf.Cos(finalRadianAngle) * radius, 11f, Mathf.Sin(finalRadianAngle) * radius); // Zmiana wysokoœci kamery
        Camera.main.transform.position = centerPoint + finalOffset;
        Camera.main.transform.LookAt(centerPoint);
    }

}

