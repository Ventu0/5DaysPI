using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneEnder : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    void Start()
    {
        DeleteSave.instance.DeletarTudoDoDontDestroy();
    }
    public void EndCutscene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
