using UnityEngine;

public class CutsceneChat : MonoBehaviour
{
    [Header("Configurações de Fala")]
    [SerializeField] int falaAtual = 0;
    [Space]
    [Header("Todos os Textos (se quiser adicionar mais falas: aumenta as array")]
    [SerializeField] int dialogoAtual = 0;
    [SerializeField] string[] dialogo1;
    [SerializeField] string[] dialogo2;
    [SerializeField] string[] dialogo3;
    [SerializeField] string[] dialogo4;
    string[][] dialogos;
    [Space]
    [Header("Todos os Rostos")]
    [SerializeField] Sprite[] charactersFace1;
    [SerializeField] Sprite[] charactersFace2;
    [SerializeField] Sprite[] charactersFace3;
    [SerializeField] Sprite[] charactersFace4;
    Sprite[][] faces;
    ChatController chatController;
    [SerializeField] CutsceneController waitPlayerInput;
    void Start()
    {
        chatController = ChatController.instance;
        faces = new Sprite[][] { charactersFace1, charactersFace2, charactersFace3, charactersFace4 };
        dialogos = new string[][] { dialogo1, dialogo2, dialogo3, dialogo4};

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
        if (chatController.falasRoutine == null && chatController != null)
            falaAtual++;
        if (falaAtual < falasMaximas)
        {
            chatController.StartDialogue(charactersFace[falaAtual], falas[falaAtual]);
        }
        else if (falaAtual >= falasMaximas)
        {
            Fechar();
        }
    }
    public void Fechar()
    {
        print("fechando");
        chatController.CloseDialogue();
        dialogoAtual += 1;
        dialogoAtual = Mathf.Clamp(dialogoAtual, 0, 3);
        falaAtual = -1;
        waitPlayerInput.director.Play();
        waitPlayerInput.waitingInput = false;
    }
}
