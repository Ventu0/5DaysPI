using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;


public class ChatController : MonoBehaviour
{
    [SerializeField] GameObject dontDestroyObject;
    [SerializeField] GameObject chatMenu;
    [SerializeField] GameObject portraitFundo;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI dialogueText;
    public Coroutine falasRoutine;
    public static ChatController instance;

    [Header("Configurações para aparecer texto de clicar E")]
    [SerializeField] GameObject pressButtonText;
    [SerializeField] float timer;
    [SerializeField] float timeToTextAppear = 5f;
    [SerializeField] bool canShowText = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(dontDestroyObject);
        }
        else
        {
            Destroy(dontDestroyObject);
        }
    }
    private void Start()
    {
        pressButtonText.SetActive(false);
        chatMenu.SetActive(false);
    }
    private void Update()
    {
        if(canShowText) timer += Time.deltaTime;
        else timer = 0;
        if(timer >= timeToTextAppear)
        {
            pressButtonText.SetActive(true);
            timer = 0;
            canShowText = false;
        }
    }
    public void StartDialogue(Sprite sprite, string fala)
    {
        chatMenu.SetActive(true);
        timer = 0;
        canShowText = true;
        pressButtonText.SetActive(false);
        Player.instance.canMove = false;
        portraitFundo.SetActive(true);
        if (sprite == null)
        {
            portraitFundo.SetActive(false);
        }else
            portrait.sprite = sprite;

        print("indo para segunda parte");
        dialogueText.text = "";
        if(falasRoutine != null)
        {
            StopAllCoroutines();
            dialogueText.text = "";
            dialogueText.text = fala;
            falasRoutine = null;
            return;
        }
        if(falasRoutine == null)
            falasRoutine = StartCoroutine(EscreverFalas(fala));
    }
    public void CloseDialogue()
    {
        StopAllCoroutines();
        Player.instance.canMove = true;
        canShowText = false;
        pressButtonText.SetActive(false);
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
