using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class SelectTarget : MonoBehaviour
{
    [Header("Essential")]
    [SerializeField] Transform arrowTransform;
    [SerializeField] float duration;

    [Header("Read-Only")]
    public BasePersonagem selectedTarget;
    [SerializeField] int currentCharacterSelected;
    public List<BasePersonagem> targets = new List<BasePersonagem>();
    public bool isSelecting;
    public bool waitingForInput;

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
        arrowTransform.gameObject.SetActive(false);
        canMove = true;
    }
    void Update()
    {
        if (isSelecting)
        {
            int horizontal = (int)Input.GetAxisRaw("Horizontal");
            if (horizontal > 0 && canMove)
            {
                canMove = false;
                currentCharacterSelected++;
                Vector2 newPos = new Vector2(targets[currentCharacterSelected].transform.position.x, targets[currentCharacterSelected].transform.position.y + 1);
                StartCoroutine(MoveArrow(newPos));
            }
            if (horizontal <= 0 && canMove)
            {
                canMove = false;
                currentCharacterSelected--;
                Vector2 newPos = new Vector2(targets[currentCharacterSelected].transform.position.x, targets[currentCharacterSelected].transform.position.y + 1);
                StartCoroutine(MoveArrow(newPos));
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                selectedTarget = targets[currentCharacterSelected];
                targets.Clear();
                arrowTransform.gameObject.SetActive(false);
                isSelecting = false;
                canMove = true;
            }
        }
    }
    IEnumerator MoveArrow(Vector2 newPos)
    {
        arrowTransform.position = newPos;
        float iterador = 0;
        while (iterador < duration)
        {
          iterador += Time.deltaTime * duration;
          arrowTransform.position = Vector2.Lerp(arrowTransform.position, newPos, iterador);
          yield return null;
        }
       canMove = true;
    }

}
