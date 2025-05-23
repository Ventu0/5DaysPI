using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
public class TextPopup : MonoBehaviour
{
    [Header("Essentials")]
    [SerializeField] Transform prefabText;

    [Header("Configurações")]
    [SerializeField] float baseDuration = 1f;
    [Space]
    [SerializeField] int tamanhoDaPool;

    [Header("Read-Only")]
    [SerializeField] Queue<Transform> textPool = new Queue<Transform>();

    //variaveis invisiveis
    List<TextMeshProUGUI> tmproPool = new List<TextMeshProUGUI>();
    public static TextPopup instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        for (int i = 0; i < tamanhoDaPool; i++)
        {
            Transform text = Instantiate(prefabText, transform.position, transform.rotation);
            text.gameObject.SetActive(false);
            tmproPool.Add(text.GetComponentInChildren<TextMeshProUGUI>());
            textPool.Enqueue(text);
        }
    }
    public void GerarTexto(string texto, float duration, Vector2 targetPos)
    {
        Transform textTransform = textPool.Dequeue();
        TextMeshProUGUI text = tmproPool[0];
        //Vector2 convertedPos = CameraController.instance.mainCamera.ScreenToWorldPoint(targetPos);
        textTransform.gameObject.SetActive(true);
        textTransform.position = targetPos;
        text.text = texto;
        StartCoroutine(moveTextUpwards(textTransform, duration));
    }
    IEnumerator moveTextUpwards(Transform text, float duration)
    {
        float iterador = 0;
        Vector2 newPos = new Vector2(text.position.x, text.position.y + 5f);
        while(iterador < duration)
        {
            iterador += Time.deltaTime / duration;
            text.position = Vector2.Lerp(text.position, newPos, iterador);
            yield return null;
        }
        text.gameObject.SetActive(false);
    }
    void Update()
    {
        
    }
}
