using UnityEngine;
public enum TransformType 
{ 
    Transform,
    RectTransform
}
public class SineUpDown : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] TransformType transformType = TransformType.Transform;
    [Space(20)]
    [SerializeField] float amplitude = 1f;
    [SerializeField] float frequency = 2f;
    [Tooltip("Speed")]

    [Header("Read-Only")]
    [SerializeField] float offsetY;
    [SerializeField] float timeElapsed;
    RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (transformType.Equals(TransformType.RectTransform))
            offsetY = rectTransform.anchoredPosition.y;
        else if (transformType.Equals(TransformType.Transform))
            offsetY = transform.position.y;
    }
    void Update()
    {
        timeElapsed = Time.time;
        if (transformType.Equals(TransformType.RectTransform))
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, amplitude * Mathf.Sin(timeElapsed * frequency) + offsetY);

        else if (transformType.Equals(TransformType.Transform))
            transform.position = new Vector2(transform.position.x, amplitude * Mathf.Sin(timeElapsed * frequency) + offsetY);
            
    }
}
