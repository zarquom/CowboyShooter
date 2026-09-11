using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarObject : MonoBehaviour
{
    [SerializeField] private Image healthObj;
    [SerializeField] private Color healthColor;
    [SerializeField] private Color healthFadedColor;

    private float currentLifeThreshold = 40f;

    private Tweener healthTween;
    public void SetLife(float currentLife)
    {
        if(currentLife < currentLifeThreshold && healthTween == null)
        {
            healthObj.color = healthColor;
            healthTween = healthObj.DOColor(healthFadedColor, 0.2f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        } 
        else if(currentLife >= currentLifeThreshold)
        {
            if (healthTween != null)
            {
                healthTween.Kill();
                healthTween = null;
            }
            healthObj.color = healthColor;
        }
        healthObj.transform.localScale = new Vector3(currentLife / 100f, 1f, 1f);
    }
}
