using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public enum Turnos
{
    PlayerTurn,
    EnemyTurn
}

public class TurnModeManager : MonoBehaviour
{
    public delegate void PlayerTurn();
    public PlayerTurn onPlayerTurn;
    public static TurnModeManager instance;
    [SerializeField] List<EnemyAI> inimigos;
    [SerializeField] bool hasOnlyOneEnemy;
    [SerializeField] List<Aliados> aliados;
    [SerializeField] int qualInimigoVaiAtacar;
    int turnoDeQualJogador;
    public Turnos turno;
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
        if (inimigos.Count > 1)
        {
            hasOnlyOneEnemy = false;
        }else if(inimigos.Count == 1)
        {
            hasOnlyOneEnemy = true;
        }
            InteractButtonsController.instance.SetupMenu(aliados[turnoDeQualJogador].transform.position);
        InteractButtonsController.instance.ataques = aliados[turnoDeQualJogador].ataques;

        onPlayerTurn?.Invoke();
    }
    void Update()
    {
        //float vertical = Input.GetAxisRaw("Vertical");
        //if (!hasOnlyOneEnemy)
        //{
        //    StartCoroutine(MoverSeta(new Vector2();
        //}
    }
    public BasePersonagem EncontrarAlvo()
    {
        if (hasOnlyOneEnemy)
        {
            return inimigos[0].GetComponent<BasePersonagem>();
        }
        else
        {
            return null;
        }
       
    }
    IEnumerator MoverSeta(Vector2 newPos)
    {
        yield return null;
    }
    public void QualPlayerVaiAtacar()
    {
        //aliados[qualJogadorVaiComeçar].
    }
}
