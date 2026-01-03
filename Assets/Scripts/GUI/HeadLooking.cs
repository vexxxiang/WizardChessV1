using System.Collections;
using UnityEngine;

public class HeadLooking : MonoBehaviour
{
    [Header("Materiały")]
    public Material _White, _Black;
    
    [Header("Ustawienia Obrotu")]
    public float maxRotationX = 30f;
    public float maxRotationY = 45f;
    public float smoothness = 5f;
    public Vector3 rotationOffset;

    [Header("Ustawienia Skalowania")]
    public float transitionDuration = 0.3f;
    public Vector3 NormalScale = new Vector3(0.7f, 0.7f, 0.7f);
    public Vector3 ZeroScale = Vector3.zero;

    private Renderer headRenderer;
    private bool isSwitching = false;

    void Awake()
    {
        headRenderer = GetComponent<Renderer>();
        // Jeśli zapomniałeś ustawić skali w inspektorze, użyj domyślnej
        if (NormalScale == Vector3.zero) NormalScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    void Start()
    {
        StopAllCoroutines();
        transform.localScale = NormalScale; // Wymuś widoczność na starcie
        Debug.Log("Głowa zainicjalizowana. Skala: " + transform.localScale);
    }

    void Update()
    {
        // Logika obrotu (zostaje bez zmian)
        Vector2 cornerPoint = new Vector2(Screen.width, 0);
        float inputX = (Input.mousePosition.x - cornerPoint.x) / Screen.width;
        float inputY = (Input.mousePosition.y - cornerPoint.y) / Screen.height;
        Vector3 targetRotation = new Vector3(-inputY * maxRotationX, (-inputX * maxRotationY) - 7, 0) + rotationOffset;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(targetRotation), Time.deltaTime * smoothness);
    }

    public void SwitchColor()
    {
        if (ChessGameManager.instance.showTour == true)
        {
            
            if (!ChessGameManager.instance.isWhiteTurn) 
            {
                StartCoroutine(SwitchCharacterRoutine(_Black, "KnightB"));
            }
            else
            {
                StartCoroutine(SwitchCharacterRoutine(_White, "KnightW"));
            }
        }
        else
        {
            headRenderer.material = (!ChessGameManager.instance.isWhiteTurn) ? _Black : _White;
            gameObject.layer = (!ChessGameManager.instance.isWhiteTurn) ? LayerMask.NameToLayer("KnightB") : LayerMask.NameToLayer("KnightW");

        }

    }

    

    IEnumerator SwitchCharacterRoutine(Material newMat, string layerName)
    {
        isSwitching = true;
        
        // 1. Znikaj
        yield return StartCoroutine(LerpScale(ZeroScale));

        // 2. Zmień
        if (headRenderer != null) headRenderer.material = newMat;
        int layer = LayerMask.NameToLayer(layerName);
        if (layer != -1) gameObject.layer = layer;

        // Mała pauza, żeby system "odetchnął"
        yield return new WaitForSeconds(0.05f);

        // 3. Pojaw się
        yield return StartCoroutine(LerpScale(NormalScale));
        
        isSwitching = false;
    }
    
    IEnumerator LerpScale(Vector3 target)
    {
        Vector3 startScale = transform.localScale;
        float time = 0;
        // Zabezpieczenie przed zerowym czasem trwania
        float duration = (transitionDuration <= 0) ? 0.3f : transitionDuration;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, target, time / duration);
            yield return null;
        }
        transform.localScale = target;
    }
}