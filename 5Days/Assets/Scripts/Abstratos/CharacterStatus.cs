using UnityEngine;

public abstract class CharacterStatus : MonoBehaviour
{
    [SerializeField] float speed;
    [Range(0, 10)]
    public float Speed { get => speed; set => speed = value; }
}
