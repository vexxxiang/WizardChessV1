using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transparenting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         var renderer = GetComponent<Renderer>();
        renderer.material = new Material(renderer.material);
        StartCoroutine(FadeOut());
        
    }
    IEnumerator FadeOut()
    {
        

        
        float fadeDuration = 1.5f; // Czas zanikania w sekundach
        float elapsedTime = 0f;

        // Pobranie komponentu Renderer
        Renderer rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("Brak komponentu Renderer!");
            yield break;
        }

        Material mat = rend.material;

        // Ustawienie shadera na Standard (Transparent)
        mat.shader = Shader.Find("Standard");
        mat.SetFloat("_Mode", 3); // Tryb Transparent
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000; // Kolejka Transparent

        // Pobranie koloru materia�u
        Color startColor = mat.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Docelowy kolor (pe�na przezroczysto��)
        yield return new WaitForSeconds(1f); // Op�nienie 1 sekundy przed startem
        // Animacja zanikania z efektem Ease In -> Ease Out
        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            float alpha = Mathf.Pow(t, 1.5f); // T^2 -> Ease In (wolny start), potem szybkie zej�cie
            mat.color = new Color(startColor.r, startColor.g, startColor.b, 1f - alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ustawienie pe�nej przezroczysto�ci na ko�cu
        mat.color = targetColor;
        gameObject.SetActive(false);
    }

}
