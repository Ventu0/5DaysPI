using UnityEngine;
using System.Collections.Generic;

public class BasePersonagem : MonoBehaviour, IDamageable
{
    public int força;
    public int vida;
    
    public void TakeDamage(int damage)
    {
        if(vida > 0)
        {
            vida -= damage;
        }else if(vida <= 0)
        {
            Destroy(gameObject);
        }

    }
}
