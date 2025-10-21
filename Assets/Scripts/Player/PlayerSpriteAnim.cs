using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script-based animation for player (similar to WolfAnim)
/// </summary>
public class PlayerSpriteAnim : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;      // Các sprite khung hình animation
    [SerializeField] float frameTime = 0.15f; // Thời gian giữa mỗi frame

    PlayerMovement playerMovement;
    SpriteRenderer spriteRenderer;

    int frameIndex = 0;
    float timer;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Kiểm tra xem nhân vật có đang di chuyển không
        bool isMoving = playerMovement != null &&
                        !playerMovement.IsHarvesting() &&
                        playerMovement.GetVelocity().sqrMagnitude > 0.01f;

        if (isMoving)
        {
            // Lặp animation khung hình
            if (Time.time > timer)
            {
                spriteRenderer.sprite = sprites[frameIndex % sprites.Length];
                frameIndex++;
                timer = Time.time + frameTime;
            }
        }
        else
        {
            // Dừng lại, giữ sprite đầu tiên (đứng yên)
            spriteRenderer.sprite = sprites[0];
            frameIndex = 0;
        }

        // Lật hướng theo chuyển động
        float moveX = playerMovement.GetVelocity().x;
        if (moveX > 0.1f)
            spriteRenderer.flipX = false;
        else if (moveX < -0.1f)
            spriteRenderer.flipX = true;
    }
}
