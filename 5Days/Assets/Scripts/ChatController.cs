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

    [Header("Configurações para aparecer texto de clicar E")]
    [SerializeField] GameObject pressButtonText;
    [SerializeField] float timer;
    [SerializeField] float timeToTextAppear = 5f;
    [SerializeField] bool canShowText = false;

    BounceEffect bounceEffect;
    public Coroutine falasRoutine;
    public static ChatController instance;
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
        bounceEffect = BounceEffect.instance;
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
        bounceEffect.isOnRoutine = false;
        //SÓ PARA MOSTRAR: posso fazer isso: portraitFundo.SetActive(sprite == null);
        if (sprite == null)
        {
            portraitFundo.SetActive(false);
        }
        else
        {
            portrait.sprite = sprite;
            portraitFundo.SetActive(true);
            //if (portrait.sprite != sprite) //se for um novo sprite
            StartCoroutine(bounceEffect?.Bounce(portrait.transform, 0.25f, 5)); //efeito de pulinho
        }

        dialogueText.text = "";
        if(falasRoutine != null)
        {
            StopAllCoroutines();
            dialogueText.text = "";
            dialogueText.text = fala;
            falasRoutine = null;
            return;
        }
        else 
            falasRoutine = StartCoroutine(EscreverFalas(fala));
    }
    public void CloseDialogue()
    {
        StopAllCoroutines();
        canShowText = false;
        pressButtonText.SetActive(false);
        chatMenu.SetActive(false);
        bounceEffect.isOnRoutine = false;
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
