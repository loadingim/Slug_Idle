using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;
    [Header("Out->In 전환 대기 시간")]
    [SerializeField] private float fadeWaitTime;

    private Coroutine fadeInRoutine;
    private Coroutine fadeOutRoutine;

    private Color outColor = new Color(0, 0, 0, 1);
    private Color inColor = new Color(0, 0, 0, 0);



    //Fade In,Out 상태
    private bool isFade;
    public bool IsFade { get { return isFade; } }

    public void FadeOut()
    {


        fadeOutRoutine = StartCoroutine(FadeOutCo());

        if (!isFade && fadeOutRoutine != null)
        {
            StopCoroutine(FadeOutCo());
            fadeOutRoutine = null;
        }


    }

    public void FadeIn()
    {

        fadeInRoutine = StartCoroutine(FadeInCo());
         
        if (isFade && fadeInRoutine != null)
        {
            StopCoroutine(FadeInCo());
            fadeInRoutine = null;
        }

    }

    private IEnumerator FadeInCo()
    {

        
        float elapsedTime = 0;

        while (elapsedTime < fadeInTime)
        {
            gameObject.GetComponent<Image>().color = Color.Lerp(outColor, inColor, elapsedTime / fadeOutTime);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        isFade = false;

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

        yield return new WaitForSeconds(fadeWaitTime);

        isFade = true;


    }



}
