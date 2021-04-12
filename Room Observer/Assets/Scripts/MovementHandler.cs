﻿using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] [Range(0f, 10f)] private float speed = 5f;
    [SerializeField] [Range(0f, 10f)] private float xBounds = 6f;
    [SerializeField] [Range(0f, 10f)] private float yBounds = 6f;

    private bool _facingRight;
    public float Move { get; private set; }

    private void Update()
    {
        Debug.Log($"death: {GameController.Instance.IsDeath} coolDown {GameController.Instance.CoolDown} playing {GameController.Instance.Playing}");
        if (GameController.Instance.IsDeath || GameController.Instance.CoolDown || !GameController.Instance.Playing)
        {
            return;
        }
        
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        Move = Mathf.Abs(horizontal) + Mathf.Abs((vertical));
        
        DoMove(horizontal, vertical);

        if (horizontal > 0 && !_facingRight)
        {
            Flipping();
        }
        else if (horizontal < 0 && _facingRight)
        {
            Flipping();
        }
    }

    private void DoMove(float horizontal, float vertical)
    {
        var dirNormal = new Vector2(horizontal, vertical).normalized;

        var fixedSpeed = speed * Time.deltaTime;
        var position = transform.position;

        dirNormal.x *= fixedSpeed;
        dirNormal.x += position.x;

        dirNormal.y *= fixedSpeed;
        dirNormal.y += position.y;
        
        var direction = SetSceneLimits(dirNormal);

        position = direction;
        transform.position = position;
    }

    private Vector2 SetSceneLimits(Vector2 dirNormal)
    {
        var h = Mathf.Clamp(dirNormal.x, -xBounds, xBounds);
        var v = Mathf.Clamp(dirNormal.y, -yBounds, yBounds);
        return new Vector2(h, v);
    }

    private void Flipping()
    {
        _facingRight = !_facingRight;

        var t = transform;
        var scale = t.localScale;
        scale.x *= -1f;
        
        t.localScale = scale;
    }
}
