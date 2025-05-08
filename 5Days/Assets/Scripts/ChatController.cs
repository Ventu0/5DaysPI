using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ChatController : MonoBehaviour
{
    public static ChatController instance;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI dialogueText;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    IEnumerator EscreverFalas(string fala)
    {
        char[] caracteres = fala.ToCharArray();
        for (int i = 0; i < caracteres.Length; i++)
        {
            dialogueText.text += caracteres[i];
            yield return new WaitForSeconds(0.05f);
        }
    }
}
