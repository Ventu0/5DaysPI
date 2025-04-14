using UnityEngine;
using System.Collections.Generic;

public class BasePersonagem : MonoBehaviour, IDamageable
{
    public int força;
    public int vida;
    
    public void TakeDamage(int damage)
    {
        vida -= damage;
    }
}
