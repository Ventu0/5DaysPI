using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SelectTarget : MonoBehaviour
{
    [Header("Essential")]
    public Transform arrowTransform;
    [SerializeField] float duration;

    [Header("Optional")]
    [SerializeField] GameObject attackText;
    [SerializeField] GameObject cancelText;
    [SerializeField] float inputTimerCooldown = 0.2f;
    [SerializeField] float inputTimer;

    [Header("Read-Only")]
    public BasePersonagem selectedTarget;
    public int currentCharacterSelected;
    public List<BasePersonagem> targets = new List<BasePersonagem>();
    public bool isSelecting;

    ///variaveis invisiveis
    public static SelectTarget instance;
    bool canMove;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        inputTimer = inputTimerCooldown;
        attackText.SetActive(false);
        cancelText.SetActive(false);
        arrowTransform.position = Vector3.zero;
        arrowTransform.gameObject.SetActive(false);
        canMove = true;
    }
    void Update()
    {
        if (isSelecting)
        {
            if (inputTimer > 0f) //para não dar erro de apertar espaço duas vezes
            {
                inputTimer -= Time.unscaledDeltaTime;
                return;
            }
            int vertical = (int)Input.GetAxisRaw("Vertical");
            if (vertical < 0 && canMove)
            {
                canMove = false;
                currentCharacterSelected++;
                currentCharacterSelected = Mathf.Clamp(currentCharacterSelected, 0, targets.Count - 1);
                CallMoveArrow(currentCharacterSelected);
            }
            if (vertical > 0 && canMove) //este código está bastante genérico e consegue trabalhar sozinho, apenas remover algumas coisas específicas (como Atacar()) e pronto
            {
                canMove = false;

                currentCharacterSelected--;
                currentCharacterSelected = Mathf.Clamp(currentCharacterSelected, 0, targets.Count - 1);
                CallMoveArrow(currentCharacterSelected);
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space))
                FinishSelect();
            if (Input.GetButtonDown("Cancel"))
                CancelSelect();
        }
    }
    void CancelSelect()
    {
        currentCharacterSelected = 0;
        targets.Clear();
        inputTimer = inputTimerCooldown;
        arrowTransform.gameObject.SetActive(false);
        attackText.SetActive(false);
        cancelText.SetActive(false);
        isSelecting = false;
        selectedTarget = null;
        InteractButtonsController.instance.menu.SetActive(true);
    }
    public void FinishSelect()
    {
        InteractButtonsController interactButtonsController = InteractButtonsController.instance;
        selectedTarget = targets[currentCharacterSelected];
        interactButtonsController.Atacar(interactButtonsController.chosenAttack, selectedTarget);

        targets.Clear();
        inputTimer = inputTimerCooldown;
        attackText.SetActive(false);
        cancelText.SetActive(false);
        selectedTarget = null;
        arrowTransform.gameObject.SetActive(false);
        isSelecting = false;
        canMove = true;
    }
    public void StartSelecting() //caso eu queria mudar para genérico (selecionar aliados também) adicionar parametro generico
    { 
        isSelecting = true;
        arrowTransform.gameObject.SetActive(true);
        currentCharacterSelected = 0;
        arrowTransform.position = new Vector2(targets[0].transform.position.x, targets[0].transform.position.y + 1);
        attackText.SetActive(true);
        cancelText.SetActive(true);
    }
    public void CallMoveArrow(int index)
    {
        Vector2 newPos = new Vector2(targets[index].transform.position.x, targets[index].transform.position.y + 1);
        StartCoroutine(MoveArrow(newPos));
    }
    IEnumerator MoveArrow(Vector2 newPos)
    {
        float iterador = 0;
        while (iterador < duration)
        {
          iterador += Time.deltaTime / duration;
          arrowTransform.position = Vector2.Lerp(arrowTransform.position, newPos, iterador);
          yield return null;
        }
       canMove = true;
    }
}
