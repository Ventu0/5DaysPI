using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class ReturnScene : MonoBehaviour
{
    [Header("Configurações animation")]
    [SerializeField] float duration = 1.5f;
    [SerializeField] float characterSpeed = 5;
    [SerializeField] string sceneName;
    public static ReturnScene instance;
    private void Awake()
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
    public void ReturnSceneBTN()
    {
        QuestController.instance.menu.SetActive(true);
        SceneManager.UnloadSceneAsync(sceneName);
        SceneTimeController.instance.onPauseGame?.Invoke();
        Time.timeScale = 1f;
    }
    public IEnumerator RunAnimation(BasePersonagem[] characters)
    {
        float iterador = 0;
        while (iterador < duration)
        {
            for(int i = 0; i < characters.Length; i++)
            {
                Transform characterTransform = characters[i].transform;
                iterador += Time.deltaTime;
                characterTransform.Translate(Vector3.left * characterSpeed * Time.deltaTime / duration);
                yield return null;
            }
        }
        ReturnSceneBTN();
    }
}