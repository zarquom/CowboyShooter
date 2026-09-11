using System;
using TMPro;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;

    private float loadingAnimationTimer = 0f;
    private float loadingChangeAnimationTime = 0.5f;
    private int currentDots = 0;
    void Update()
    {
        loadingAnimationTimer += Time.deltaTime;
        if(loadingAnimationTimer >= loadingChangeAnimationTime)
        {
            loadingAnimationTimer = 0f;
            currentDots = (currentDots + 1) % 4;
            loadingText.text = "Loading" + new string('.', currentDots);
        }
    }
}
