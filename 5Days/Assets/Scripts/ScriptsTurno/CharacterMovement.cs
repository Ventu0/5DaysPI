using UnityEngine;
using System.Collections;
public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement instance;
    
    void Awake()
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

    public void Move()
    {

    }

    IEnumerator AllyMove()
    {
        yield return null;
    } 
    IEnumerator ShadowMove(GameObject shadow)
    {
        yield return null;
    }
}
