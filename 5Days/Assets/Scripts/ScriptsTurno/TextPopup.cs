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
    [SerializeField] List<Transform> textPool;

    //variaveis invisiveis
    List<TextMeshProUGUI> tmproPool;
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
            text.position = Vector3.zero;
            tmproPool.Add(text.GetComponent<TextMeshProUGUI>());
            textPool.Add(text);
        }
    }
    public void GerarTexto(string texto, float duration)
    {
        Transform textTransform = textPool[0];
        TextMeshProUGUI text = tmproPool[0];
        text.text = texto;
        StartCoroutine(moveTextUpwards(textTransform, duration));
    }
    IEnumerator moveTextUpwards(Transform text, float duration)
    {
        float iterador = 0;
        Vector2 newPos = new Vector2(text.position.x, text.position.y + 2f);
        while(iterador < duration)
        {
            iterador += Time.deltaTime;
            text.position = Vector2.Lerp(text.position, newPos, iterador / duration);
            yield return null;
        }
        text.gameObject.SetActive(false);
    }
    void Update()
    {
        
    }
}
