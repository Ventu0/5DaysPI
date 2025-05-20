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
    public GameObject attackText;
    [SerializeField] float inputTimerCooldown = 0.2f;
    [SerializeField] float inputTimer;

    [Header("Read-Only")]
    public BasePersonagem selectedTarget;
    [SerializeField] int currentCharacterSelected;
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
        attackText.gameObject.SetActive(false);
        arrowTransform.position = Vector3.zero;
        arrowTransform.gameObject.SetActive(false);
        canMove = true;
    }
    void Update()
    {
        if (isSelecting)
            {
            if (inputTimer > 0f)
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
                CallMoveArrow();
            }
            if (vertical > 0 && canMove) //este código está bastante genérico e consegue trabalhar sozinho, apenas remover algumas coisas específicas (como Atacar()) e pronto
            {
                canMove = false;

                currentCharacterSelected--;
                currentCharacterSelected = Mathf.Clamp(currentCharacterSelected, 0, targets.Count - 1);
                CallMoveArrow();
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                InteractButtonsController interactButtonsController = InteractButtonsController.instance;
                selectedTarget = targets[currentCharacterSelected];
                interactButtonsController.Atacar(interactButtonsController.chosenAttack, selectedTarget);

                targets.Clear();
                inputTimer = inputTimerCooldown;
                attackText.SetActive(false);
                selectedTarget = null;
                arrowTransform.gameObject.SetActive(false);
                isSelecting = false;
                canMove = true;
            }
        }
    }
    public void StartSelecting() //caso eu queria mudar para genérico (selecionar aliados também) adicionar parametro generico
    { 
        isSelecting = true;
        arrowTransform.gameObject.SetActive(true);
        currentCharacterSelected = 0;
        arrowTransform.position = new Vector2(targets[0].transform.position.x, targets[0].transform.position.y + 1);
        attackText.SetActive(true);
    }
    void CallMoveArrow()
    {
        Vector2 newPos = new Vector2(targets[currentCharacterSelected].transform.position.x, targets[currentCharacterSelected].transform.position.y + 1);
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
