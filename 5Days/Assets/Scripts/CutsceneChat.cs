using UnityEngine;

public class CutsceneChat : MonoBehaviour
{
    [Header("Configurações de Fala")]
    [SerializeField] int falaAtual = -1;
    [Space]
    [Header("Todos os Textos")]
    [SerializeField] int dialogoAtual = 0;
    [SerializeField] string[] dialogo1;
    [SerializeField] string[] dialogo2;
    [SerializeField] string[] dialogo3;
    string[][] dialogos;
    [Space]
    [Header("Todos os Rostos")]
    [SerializeField] Sprite[] charactersFace1;
    [SerializeField] Sprite[] charactersFace2;
    [SerializeField] Sprite[] charactersFace3;
    Sprite[][] faces;
    ChatController chatController;
    [SerializeField] WaitPlayerinput waitPlayerInput;
    void Start()
    {
        chatController = ChatController.instance;
        faces = new Sprite[][] { charactersFace1, charactersFace2, charactersFace3 };
        dialogos = new string[][] { dialogo1, dialogo2, dialogo3};
    }
    void Update()
    {
        
    }
    public void IniciarFala()
    {
        Falar(dialogos[dialogoAtual], faces[dialogoAtual]);
    }
    public void Falar(string[] falas, Sprite[] charactersFace)
    {
        int falasMaximas = falas.Length;
        if (chatController.falasRoutine == null)
            falaAtual++;
        if (falaAtual < falasMaximas)
        {
            chatController.StartDialogue(charactersFace[falaAtual], falas[falaAtual]);
        }
        if (falaAtual > falasMaximas)
        {
            chatController.CloseDialogue();
            dialogoAtual += 1;
            dialogoAtual = Mathf.Clamp(dialogoAtual, 0, 2);
            falaAtual = -1;
            waitPlayerInput.director.Play();
            waitPlayerInput.waitingInput = false;
        }
    }
}
