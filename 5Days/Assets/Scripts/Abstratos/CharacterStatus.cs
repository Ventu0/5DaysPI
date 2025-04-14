using UnityEngine;

public abstract class CharacterStatus : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] float speed;
    [SerializeField] int vida;
    public float Speed { get => speed; set => speed = value; }
    public int Vida { get => vida; set => vida = value; }
}
