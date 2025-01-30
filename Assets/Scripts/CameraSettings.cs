using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    public Transform boardCenter; // �rodek planszy (ustaw na obiekt reprezentuj�cy �rodek planszy)
    public float radius = 10f; // Promie� p�okr�gu
    public float animationDuration = 2f; // Czas trwania animacji
    public ChessGameManager gameManager; // Odniesienie do ChessGameManager

    private Vector3 initialPosition; // Pocz�tkowa pozycja kamery
    private Quaternion initialRotation; // Pocz�tkowa rotacja kamery
    private bool isAnimating = false;

    void Start()
    {
        // Ustawienie pocz�tkowej pozycji kamery
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

        Vector3 centerPoint = new Vector3(3.5f, 0f, 3.5f); // �rodek planszy
        float radius = 10f; // Promie� obrotu kamery

        // Pozycja pocz�tkowa - Kamera zaczyna od ty�u
        Vector3 startPosition = Camera.main.transform.position;
        float startAngle = Mathf.Atan2(startPosition.z - centerPoint.z, startPosition.x - centerPoint.x) * Mathf.Rad2Deg;

        // Ko�cowy k�t w zale�no�ci od tury
        float endAngle = gameManager.isWhiteTurn ? -90f : 90f; // Kamera zaczyna od ty�u, zatem:
                                                               // - Dla bia�ych: od ty�u, ko�czy za bia�ymi.
                                                               // - Dla czarnych: od ty�u, ko�czy za czarnymi.

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // Progres animacji
            float currentAngle = Mathf.Lerp(startAngle, endAngle, t); // Aktualny k�t

            // Obliczenie pozycji kamery na okr�gu
            float radianAngle = Mathf.Deg2Rad * currentAngle;
            Vector3 offset = new Vector3(Mathf.Cos(radianAngle) * radius, 11f, Mathf.Sin(radianAngle) * radius); // Zmiana wysoko�ci kamery o 1.5f

            Camera.main.transform.position = centerPoint + offset; // Pozycja kamery
            Camera.main.transform.LookAt(centerPoint); // Ustawienie kamery, by patrzy�a na plansz�

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ustawienie ko�cowej pozycji i rotacji
        float finalRadianAngle = Mathf.Deg2Rad * endAngle;
        Vector3 finalOffset = new Vector3(Mathf.Cos(finalRadianAngle) * radius, 11f, Mathf.Sin(finalRadianAngle) * radius); // Zmiana wysoko�ci kamery
        Camera.main.transform.position = centerPoint + finalOffset;
        Camera.main.transform.LookAt(centerPoint);
    }

}
