using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Camera.main.transform.SetParent(Player.instance.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
