using UnityEngine;
using System.Collections.Generic;

public class BasePersonagem : MonoBehaviour, IDamageable
{   
    public int vida;
    [SerializeField] List<BasicAttack> ataques;
    [SerializeField] Turnos turnoTipo;

    void Start()
    {
        
    }
    void Update()
    {
        
    }
    
    public void TakeDamage(int damage)
    {
        vida -= damage;
    }
}
