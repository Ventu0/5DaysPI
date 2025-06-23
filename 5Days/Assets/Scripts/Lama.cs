using UnityEngine;

public class Lama : MonoBehaviour
{
    float originalSpeed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            originalSpeed = player.Speed;
            player.Speed /= 2;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.Speed = originalSpeed;
        }
    }

}
