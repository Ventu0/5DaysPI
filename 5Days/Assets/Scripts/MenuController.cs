using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;


public class MenuController : MonoBehaviour
{
    [SerializeField] Sprite clickSprite;
    [SerializeField] Transform characterTransform;
    [SerializeField] float jumpQuantity = 10f;
    [SerializeField] float bobbingDuration = 0.5f;
    Image objectImage;
    Sprite originalSprite;
    Coroutine routine;
    void Start()
    {
        objectImage = characterTransform.GetComponent<Image>();
        originalSprite = objectImage.sprite;
    }
    public void StartButton()
    {
        SceneManager.LoadScene("CasaDianas");
    }
    public void OptionsButton()
    {

    }
    public void ExitButton()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
    public void OnCharacterClick()
    {
        if (routine != null)
            return; 
        else if (routine == null)
            routine = StartCoroutine(BounceEffect(characterTransform));

    }
    IEnumerator BounceEffect(Transform tranform)
    {
        objectImage.sprite = clickSprite;
        float iterador = 0;
        Vector2 newPos = new Vector2(tranform.position.x, tranform.position.y + jumpQuantity);
        while (iterador < bobbingDuration)
        {
            iterador += Time.deltaTime / bobbingDuration;
            tranform.position = Vector2.Lerp(tranform.position, newPos, iterador);
            yield return null;
        }

        yield return null;

        iterador = 0;
        newPos = new Vector2(tranform.position.x, tranform.position.y - jumpQuantity);
        while (iterador < bobbingDuration)
        {
            iterador += Time.deltaTime / bobbingDuration;
            tranform.position = Vector2.Lerp(tranform.position, newPos, iterador);
            yield return null;
        }
        yield return new WaitForSeconds(0.4f);
        routine = null;
        objectImage.sprite = originalSprite;
    }
}
