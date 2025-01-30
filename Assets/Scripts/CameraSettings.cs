using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraSettings : MonoBehaviour
{
    public Transform boardCenter; // Środek planszy
    public float radius = 10f; // Promień obrotu
    public float animationDuration = 2f; // Czas trwania animacji
    public ChessGameManager gameManager; // Odniesienie do ChessGameManager
    public Slider heightSlider; // Slider do zmiany wysokości kamery

    private Vector3 initialPosition; // Początkowa pozycja kamery
    private Quaternion initialRotation; // Początkowa rotacja kamery
    private bool isAnimating = false;
    private float minz, camz;

    void Start()
    {
        // Ustawienie początkowej pozycji kamery
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Dodajemy listener na slider, aby zmieniać wysokość
        heightSlider.onValueChanged.AddListener(UpdateCameraHeight);
    }

    void Update()
    {
        // Jeżeli kamera się animuje, nie robimy zmian na bieżąco
        if (isAnimating) return;

        // Aktualizowanie patrzenia na target (środek planszy)
        transform.LookAt(boardCenter);
    }

    public void RotateAroundBoard()
    {
        if (!isAnimating)
            StartCoroutine(RotateCamera());
    }

    IEnumerator RotateCamera()
    {
        float elapsedTime = 0f;
        float startAngle = Mathf.Atan2(transform.position.z - boardCenter.position.z, transform.position.x - boardCenter.position.x) * Mathf.Rad2Deg;
        float endAngle = gameManager.isWhiteTurn ? 270: 90f;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float currentAngle = Mathf.Lerp(startAngle, endAngle, t);

            // Pozycja kamery w zależności od kąta obrotu
            Vector3 offset = new Vector3(Mathf.Cos(Mathf.Deg2Rad * currentAngle) * radius, transform.position.y, Mathf.Sin(Mathf.Deg2Rad * currentAngle) * radius);
            transform.position = boardCenter.position + offset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ustawienie ostatecznej pozycji
        Vector3 finalOffset = new Vector3(Mathf.Cos(Mathf.Deg2Rad * endAngle) * radius, transform.position.y, Mathf.Sin(Mathf.Deg2Rad * endAngle) * radius);
        transform.position = boardCenter.position + finalOffset;

    }

    void UpdateCameraHeight(float value)
    {


        // -7 13.5
        if (gameManager.isWhiteTurn)
        {
            minz = -10f;
            camz = 3.49f;


        }
        else {
             minz = 16.5f;
            camz = 3.51f;
        }




        // Obliczanie końcowej pozycji kamery
        float height = Mathf.Lerp(0f, 15f, value); // Wartość z slidera, gdzie 0 to wysokość minimalna, a 10 to wysokość maksymalna

        // Ustalamy pozycję kamery w osi Y oraz Z
        Vector3 newPosition = transform.position;
        newPosition.y = height;
        newPosition.z = Mathf.Lerp(minz, camz, value); // Zmiana w osi Z w zależności od wysokości kamery

        transform.position = newPosition;
    }
}
