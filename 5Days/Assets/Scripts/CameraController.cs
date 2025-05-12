using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cinemachineCamera;
    void Start()
    {
        cinemachineCamera.LookAt = Player.instance.transform;
        cinemachineCamera.Follow = Player.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
