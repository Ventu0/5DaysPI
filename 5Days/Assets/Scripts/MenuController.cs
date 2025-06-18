using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class MenuController : MonoBehaviour
{
    [SerializeField] Transform characterTransform;
    [SerializeField] float bobbingDuration = 0.5f;
    void Start()
    {

    }
    public void StartButton()
    {
        SceneManager.LoadScene("VilaIndio");
    }
    public void OptionsButton()
    {

    }
    public void ExitButton()
    {
        Application.Quit();
    }
    public void OnCharacterClick()
    {
        StartCoroutine(BounceEffect(characterTransform));
    }
    IEnumerator BounceEffect(Transform tranform)
    {
        float iterador = 0;
        Vector2 newPos = new Vector2(tranform.position.x, tranform.position.y + 10f);
        while (iterador < bobbingDuration)
        {
            iterador += Time.deltaTime / bobbingDuration;
            tranform.position = Vector2.Lerp(tranform.position, newPos, iterador);
            yield return null;
        }
        yield return null;
        iterador = 0;
        newPos = new Vector2(tranform.position.x, tranform.position.y - 10f);
        while (iterador < bobbingDuration)
        {
            iterador += Time.deltaTime / bobbingDuration;
            tranform.position = Vector2.Lerp(tranform.position, newPos, iterador);
            yield return null;
        }
    }
}
