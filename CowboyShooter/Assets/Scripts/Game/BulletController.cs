using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed = 10f;
    void Update()
    {
        Move();
        CheckBounds();
    }

    private void Move()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void CheckBounds()
    {
        if (transform.position.y > 10f)
        {
            gameObject.SetActive(false);
        }
    }
}
