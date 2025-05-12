using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ChatController : MonoBehaviour
{
    public static ChatController instance;
    [SerializeField] GameObject chatMenu;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI dialogueText;
    public Coroutine falasRoutine;
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
    private void Start()
    {
        chatMenu.SetActive(false);
    }
    public void StartDialogue(Sprite sprite, string fala)
    {
        chatMenu.SetActive(true);
        portrait.sprite = sprite;
        dialogueText.text = "";
        if(falasRoutine != null)
        {
            StopCoroutine(falasRoutine);
            dialogueText.text = fala;
        }
        falasRoutine = StartCoroutine(EscreverFalas(fala));
    }
    public void CloseDialogue()
    {
        StopAllCoroutines();
        chatMenu.SetActive(false);
        dialogueText.text = "";
        portrait.sprite = null;
    }
    IEnumerator EscreverFalas(string fala)
    {
        char[] caracteres = fala.ToCharArray();
        for (int i = 0; i < caracteres.Length; i++)
        {
            dialogueText.text += caracteres[i];
            yield return new WaitForSeconds(0.05f);
        }
        falasRoutine = null;
    }
}
