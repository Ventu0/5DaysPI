using UnityEngine;
using System.Collections.Generic;
public enum Turnos
{
    PlayerTurn,
    EnemyTurn
}

public class TurnModeManager : MonoBehaviour
{
    public static TurnModeManager instance;
    [SerializeField] List<EnemyAI> inimigos;
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
    }
    void Update()
    {
        
    }
}
