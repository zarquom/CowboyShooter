using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarObject : MonoBehaviour
{
    [SerializeField] private Image healthObj;
    [SerializeField] private Color healthColor;
    [SerializeField] private Color healthFadedColor;
    [SerializeField] private float currentLifeThreshold = 40f;
    [SerializeField] private float pulseDuration = 0.2f;

    private Tweener healthTween;
    public void SetLife(float currentLife, float maxLife)
    {
        if(currentLife < currentLifeThreshold && healthTween == null)
        {
            healthObj.color = healthColor;
            healthTween = healthObj.DOColor(healthFadedColor, pulseDuration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
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
        healthObj.transform.localScale = new Vector3(currentLife / maxLife, 1f, 1f);
    }
}
