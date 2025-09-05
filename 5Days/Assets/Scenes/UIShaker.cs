using UnityEngine;

public class UIShaker : MonoBehaviour
{
    public float shakeAmount = 5f;  // Pixels de movimento (pode ser tipo 2~10)
    public float shakeSpeed = 50f;

    private RectTransform rectTransform;
    private Vector3 originalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float offsetX = Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) * 2 - 1;
        float offsetY = Mathf.PerlinNoise(0f, Time.time * shakeSpeed) * 2 - 1;

        Vector2 shakeOffset = new Vector2(offsetX, offsetY) * shakeAmount;
        rectTransform.anchoredPosition = (Vector2)originalPosition + shakeOffset;
    
    }
}
