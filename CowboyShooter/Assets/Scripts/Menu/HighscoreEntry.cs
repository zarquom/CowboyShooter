using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userNameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite[] backgroundSprites;

    public void SetupEntry(string userName, int score, int index)
    {
        int playerPosition = index + 1;
        userNameText.text = $"{playerPosition}. {userName}";
        scoreText.text = score.ToString();
        backgroundImage.sprite = GetBackgroundSprite(index);
    }

    private Sprite GetBackgroundSprite(int index)
    {
        return index < 3 ? backgroundSprites[index] : backgroundSprites[3];
    }
}
