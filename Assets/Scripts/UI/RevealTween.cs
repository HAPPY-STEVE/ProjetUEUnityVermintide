using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace UI.Tween
{

public class RevealTween : TweenContainer
{
    [Header("Variables animation")]
    public float tempsAnim = 1f;
    public float tempsDelay = 0f;
    public Ease EaseAnim = Ease.InOutSine;
    [Header("Est-ce qu'on anime l'apparition de l'UI ?")]
    public bool animationStart=true;
    [Header("Est-ce qu'on anime la fin de l'UI ?")]
    public bool animationEnd=true;

    // Start is called before the first frame update
    void Start()
    {
        if (animationStart == true)
        {
                OnActive(); 
        }

    }

    override public void OnActive()
        {
            GetComponent<CanvasGroup>().alpha = 0;
            Vector3 p = (new Vector3(0, 1, 1));
            gameObject.transform.localScale = p;
            DOTween.Sequence()
                .Append(GetComponent<CanvasGroup>().DOFade(1, tempsAnim * 2).SetDelay(tempsDelay).SetEase(EaseAnim).SetAutoKill(true))
                .Append(gameObject.transform.DOScale(new Vector3(1, 1, 1), tempsAnim).SetDelay(tempsDelay).SetEase(EaseAnim));

            DOTween.PlayAll();
        }
        override public void OnClose()
    {
            DOTween.Sequence()
            .Append(GetComponent<CanvasGroup>().DOFade(0, tempsAnim).SetDelay(tempsDelay).SetEase(EaseAnim).SetAutoKill(true));

            DOTween.PlayAll();
    }
}

}