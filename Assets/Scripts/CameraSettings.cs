using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class CameraSettings : MonoBehaviour
{
    public Transform boardCenter; // Środek planszy
    
    public ChessGameManager gameManager; // Odniesienie do ChessGameManager
    public Slider heightSlider; // Slider do zmiany wysokości kamery
    public Transform target; // Obiekt, wokół którego ma się obracać kamera (środek)
    public float animationDuration = 1.2f; // Czas trwania animacji (możesz dostosować)
    private Vector3 offset; // Przesunięcie kamery w stosunku do targetu

    void Update()
    {
        RotateCamera();
        heightSlider.onValueChanged.AddListener(delegate { UpdateCameraHeight(heightSlider.value); }); ; 
    }
    void RotateCamera()
    {
        offset = new Vector3(this.transform.position.x , this.transform.position.y, this.transform.position.z);
        // Oblicz nową pozycję kamery w zależności od rotacji obiektu
        transform.position = offset;
    }

    public void RotateAroundBoard([CallerMemberName] string callerName = "")
    {
        //Debug.Log($"SomeMethod was called by: {callerName}");
        float startAngle = target.transform.rotation.eulerAngles.y;  // Pobieramy obecny kąt Y
        float endAngle = !gameManager.isWhiteTurn ? 180f : 0f;  // Ustawiamy kąt docelowy

        StartCoroutine(RotateCamera(startAngle, endAngle));  // Uruchamiamy animację obracania
    }

    IEnumerator RotateCamera(float startAngle, float endAngle)
    {
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;  // Progres animacji od 0 do 1

            // Interpolacja Ease In Out (SmoothStep)
            float smoothT = Mathf.SmoothStep(0f, 1f, t);  // Używamy SmoothStep dla efektu Ease In Out

            float currentAngle = Mathf.LerpAngle(startAngle, endAngle, smoothT);  // Interpolacja kąta rotacji z efektem Ease In Out

            // Ustawiamy nowy kąt rotacji obiektu
            target.transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);

            elapsedTime += Time.deltaTime;  // Aktualizacja czasu
            yield return null;  // Czekamy do następnej klatki
        }

        // Na końcu ustawiamy dokładnie końcową rotację, by uniknąć ewentualnych niedokładności
        target.transform.rotation = Quaternion.Euler(0f, endAngle, 0f);
    }

    public void UpdateCameraHeight(float value)
    {

        if (gameManager.isWhiteTurn)
        {
            Vector3 startPoint = new Vector3(transform.position.x, 1.5f, Mathf.Lerp(-6f, 3f, 0f)); // Minimalna wysokość
            Vector3 controlPoint1 = new Vector3(transform.position.x, 5f, Mathf.Lerp(-6f, 3f, -0.8f)); // Punkt kontrolny 1
            Vector3 controlPoint2 = new Vector3(transform.position.x, 8f, Mathf.Lerp(-6f, 3f, -0.9f)); // Punkt kontrolny 2
            Vector3 endPoint = new Vector3(transform.position.x, 13f, Mathf.Lerp(-6f, 3f, 1f)); // Maksymalna wysokość

            // Interpolacja Béziera dla Y i Z w zależności od slidera (value od 0 do 1)
            float t = value;
            Vector3 bezierPosition = Mathf.Pow(1 - t, 3) * startPoint +
                                     3 * Mathf.Pow(1 - t, 2) * t * controlPoint1 +
                                     3 * (1 - t) * Mathf.Pow(t, 2) * controlPoint2 +
                                     Mathf.Pow(t, 3) * endPoint;
            // Aktualizacja pozycji kamery
            transform.position = new Vector3(transform.position.x, bezierPosition.y, bezierPosition.z);
        }
        else {
            Vector3 startPoint = new Vector3(transform.position.x, 1.5f, Mathf.Lerp(13f, 4f, 0f)); // Minimalna wysokość
            Vector3 controlPoint1 = new Vector3(transform.position.x, 5f, Mathf.Lerp(13f, 4f, -0.8f)); // Punkt kontrolny 1
            Vector3 controlPoint2 = new Vector3(transform.position.x, 8f, Mathf.Lerp(13f, 4f, -0.9f)); // Punkt kontrolny 2
            Vector3 endPoint = new Vector3(transform.position.x, 13f, Mathf.Lerp(13f, 4f, 1f)); // Maksymalna wysokość
            // Interpolacja Béziera dla Y i Z w zależności od slidera (value od 0 do 1)
            float t = value;
            Vector3 bezierPosition = Mathf.Pow(1 - t, 3) * startPoint +
                                     3 * Mathf.Pow(1 - t, 2) * t * controlPoint1 +
                                     3 * (1 - t) * Mathf.Pow(t, 2) * controlPoint2 +
                                     Mathf.Pow(t, 3) * endPoint;
            // Aktualizacja pozycji kamery
            transform.position = new Vector3(transform.position.x, bezierPosition.y, bezierPosition.z);
        }
        transform.LookAt(boardCenter);
    }
}
