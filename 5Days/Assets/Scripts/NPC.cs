using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class NPC : MonoBehaviour
{
    [Header("Configurações de Fala")]
    [SerializeField] string[] falas;
    [SerializeField] int falaAtual;
    [SerializeField] Sprite[] expressõesPersonagens;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI dialogueText;
    void Start()
    {
        StartCoroutine(EscreverFalas("Eu Adoro Pintar O Cabelo"));
    }
    void Update()
    {

    }
    public void Falar()
    {
        print("falando");
        if (falas.Length > 0 && falaAtual <= falas.Length)
        {
            StartCoroutine(EscreverFalas(falas[falaAtual]));
        }
    }
    IEnumerator EscreverFalas(string fala)
    {
        char[] caracteres = fala.ToCharArray();
        for(int i = 0; i < caracteres.Length; i++)
        {
            dialogueText.text += caracteres[i];
            yield return new WaitForSeconds(0.05f);
        }
    }
}
