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
    Queue<TextMeshProUGUI> tmproPool = new Queue<TextMeshProUGUI>();
    float normalTextSize;
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
            tmproPool.Enqueue(text.GetComponentInChildren<TextMeshProUGUI>());
            textPool.Enqueue(text);
        }
    }
    public void GerarTexto(string texto, Vector2 targetPos, Color textColor, float textSize = 36f)
    {
        Transform textTransform = textPool.Dequeue();
        TextMeshProUGUI tmProText = tmproPool.Dequeue();

        float x = Random.Range(-0.5f, 0.5f);
        float y = Random.Range(-0.5f, 0.5f);
        targetPos = new Vector2(targetPos.x + x, targetPos.y + y);
        
        textTransform.gameObject.SetActive(true);
        textTransform.position = targetPos;
        tmProText.color = textColor;
        tmProText.text = texto;

        if(textSize != 0) tmProText.fontSize = textSize;
        StartCoroutine(moveTextUpwards(textTransform));
    }
    IEnumerator moveTextUpwards(Transform text)
    {
        float iterador = 0;
        Vector2 newPos = new Vector2(text.position.x, text.position.y + 0.4f);
        while(iterador < baseDuration)
        {
            iterador += Time.deltaTime / baseDuration;
            text.position = Vector2.Lerp(text.position, newPos, iterador);
            yield return null;
        }
        yield return null;
        iterador = 0;
        newPos = new Vector2(text.position.x, text.position.y - 0.4f);
        while (iterador < baseDuration)
        {
            iterador += Time.deltaTime / baseDuration;
            text.position = Vector2.Lerp(text.position, newPos, iterador);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        textPool.Enqueue(text);
        tmproPool.Enqueue(text.GetComponentInChildren<TextMeshProUGUI>());
        text.gameObject.SetActive(false);
    }
}
