using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;

    private Coroutine fadeInRoutine;
    private Coroutine fadeOutRoutine;

    private Color outColor = new Color(0, 0, 0, 1);
    private Color inColor = new Color(0, 0, 0, 0);
    public void FadeOut()
    {

        fadeOutRoutine = StartCoroutine(FadeOutCo());
    }

    public void FadeIn()
    {

        fadeInRoutine = StartCoroutine(FadeInCo());
    }

    private IEnumerator FadeInCo()
    {
        float elapsedTime = 0;

        while (elapsedTime <= fadeInTime)
        {
            gameObject.GetComponent<Image>().color = Color.Lerp(outColor, inColor, elapsedTime / fadeOutTime);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield break;

    }



    private IEnumerator FadeOutCo()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutTime)
        {
            gameObject.GetComponent<Image>().color = Color.Lerp(inColor, outColor, elapsedTime / fadeOutTime);
             
            elapsedTime += Time.deltaTime;

            yield return null;
        }
         
        yield break;
    }



}
