using UnityEngine;

public class MenuItemAnimation : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        if(rectTransform.anchoredPosition.x > 500f)
        {
            rectTransform.anchoredPosition = new Vector2(-500f, rectTransform.anchoredPosition.y);
        }
    }
}
