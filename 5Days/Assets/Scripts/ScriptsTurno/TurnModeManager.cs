using UnityEngine;
using System.Collections.Generic;
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
    [SerializeField] List<BasePersonagem> aliados;
    int qualJogadorVaiComeçar;
    public Turnos turno;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
