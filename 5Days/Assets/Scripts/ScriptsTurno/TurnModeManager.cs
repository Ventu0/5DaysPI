using UnityEngine;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] List<Aliados> aliados;
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
        InteractButtonsController.instance.SetupMenu(aliados[turnoDeQualJogador].transform.position, aliados[turnoDeQualJogador].ataques.ToArray());

        onPlayerTurn?.Invoke();
    }
    void Update()
    {
        
    }
    public void QualPlayerVaiAtacar()
    {
        //aliados[qualJogadorVaiComeçar].
    }
}
