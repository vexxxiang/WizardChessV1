using System.Collections;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    public Transform boardCenter; // Środek planszy (ustaw na obiekt reprezentujący środek planszy)
    public float radius = 10f; // Promień obrotu kamery
    public float animationDuration = 2f; // Czas trwania animacji
    public ChessGameManager gameManager; // Odniesienie do ChessGameManager

    private bool isAnimating = false;

    public void RotateAroundBoard()
    {
        if (!isAnimating)
            StartCoroutine(RotateCamera());
    }

    IEnumerator RotateCamera()
    {
        isAnimating = true;
        float elapsedTime = 0f;

        // **🔹 Odczytaj aktualną pozycję kamery i przelicz kąt**
        Vector3 startPosition = Camera.main.transform.position;
        float startAngle = Mathf.Atan2(startPosition.z - boardCenter.position.z, startPosition.x - boardCenter.position.x) * Mathf.Rad2Deg;

        // **🔹 Oblicz nowy kąt po obrocie o 180°**
        float endAngle = startAngle + 180f;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration; // Progres animacji (0 - 1)
            float newAngle = Mathf.Lerp(startAngle, endAngle, t);

            // **🔹 Oblicz nową pozycję kamery na okręgu**
            float radianAngle = Mathf.Deg2Rad * newAngle;
            Vector3 offset = new Vector3(Mathf.Cos(radianAngle) * radius, startPosition.y, Mathf.Sin(radianAngle) * radius);

            Camera.main.transform.position = boardCenter.position + offset;
            Camera.main.transform.LookAt(boardCenter.position);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // **🔹 Precyzyjnie ustaw końcową pozycję (usuwa błędy zaokrągleń)**
        float finalRadianAngle = Mathf.Deg2Rad * endAngle;
        Vector3 finalOffset = new Vector3(Mathf.Cos(finalRadianAngle) * radius, startPosition.y, Mathf.Sin(finalRadianAngle) * radius);

        Camera.main.transform.position = boardCenter.position + finalOffset;
        Camera.main.transform.LookAt(boardCenter.position);

        isAnimating = false;
    }
}
