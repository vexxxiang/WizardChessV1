using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoPanelHeadLooking : MonoBehaviour
{
    [Header("Materiały")]
    public Material _White, _Black;
    
    [Header("Ustawienia Obrotu")]
    public float maxRotationX = 30f;
    public float maxRotationY = 45f;
    public float XOffset = 0f;
    public float smoothness = 5f;
    public Vector3 rotationOffset;
    
    [Header("Layers")]
    public string blackLayer;
    public string whiteLayer;
    
    

    [Header("Ustawienia Ruchu")]
    public float transitionDuration = 0.3f;
    private Renderer headRenderer;

    void Awake()
    {
        headRenderer = GetComponent<Renderer>();
        
    }


    void Update()
    {
        // Logika obrotu (zostaje bez zmian)
        Vector2 cornerPoint = new Vector2(Screen.width, 0);
        float inputX = (Input.mousePosition.x - cornerPoint.x) / Screen.width;
        float inputY = (Input.mousePosition.y - cornerPoint.y) / Screen.height;
        Vector3 targetRotation = new Vector3(-inputY * maxRotationX, (-inputX * maxRotationY) + XOffset, 0) + rotationOffset;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(targetRotation), Time.deltaTime * smoothness);
    }
    
    public void UpdateColor()
    {
        headRenderer.material = (!ChessGameManager.instance.isWhiteTurn) ? _Black : _White;
        gameObject.layer = (!ChessGameManager.instance.isWhiteTurn) ? LayerMask.NameToLayer(blackLayer) : LayerMask.NameToLayer(whiteLayer);

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
