using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Qu?n lý toàn b? animation cho sói: Idle, Move, Attack, Eat.
/// </summary>
[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(SpriteRenderer))]
public class WolfAnimationController : MonoBehaviour
{
    [Header("Animation Frames")]
    [SerializeField] private Sprite idleSprite;          // frame ??ng yên
    [SerializeField] private Sprite[] moveSprites;       // frame khi ch?y
    [SerializeField] private Sprite[] attackSprites;     // frame khi t?n công artifact
    [SerializeField] private Sprite[] eatSprites;        // frame khi ?n bush (dành cho isEater)

    [Header("Timing")]
    [SerializeField] private float moveFrameTime = 0.15f;
    [SerializeField] private float attackFrameTime = 0.15f;
    [SerializeField] private float eatFrameTime = 0.15f;

    private SpriteRenderer spriteRenderer;
    private EnemyAI enemyAI;

    private int currentFrame;
    private float nextFrameTime;
    private bool wasAttacking;
    private bool wasEating;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAI = GetComponent<EnemyAI>();
    }

    void Update()
    {
        bool isEater = GetPrivateBool("isEater");
        bool isMoving = enemyAI.isMoving;
        bool isAttacking = GetPrivateBool("attacking");
        bool isKillingBush = GetPrivateBool("killingBush");

        // c?p nh?t h??ng nhìn
        spriteRenderer.flipX = enemyAI.left;

        // Xác ??nh tr?ng thái ?u tiên
        // Eater có th? ?ang ?n bush (killingBush)
        if (isEater && isKillingBush)
        {
            PlayAnimation(eatSprites, eatFrameTime, ref wasEating);
            wasAttacking = false;
        }
        else if (!isEater && isAttacking)
        {
            PlayAnimation(attackSprites, attackFrameTime, ref wasAttacking);
            wasEating = false;
        }
        else if (isMoving)
        {
            PlayAnimation(moveSprites, moveFrameTime);
            wasAttacking = false;
            wasEating = false;
        }
        else
        {
            spriteRenderer.sprite = idleSprite;
            currentFrame = 0;
            wasAttacking = false;
            wasEating = false;
        }
    }

    // Animation có reset (Attack, Eat)
    private void PlayAnimation(Sprite[] frames, float frameTime, ref bool resetFlag)
    {
        if (frames == null || frames.Length == 0) return;

        if (!resetFlag)
        {
            currentFrame = 0;
            nextFrameTime = Time.time + frameTime;
            spriteRenderer.sprite = frames[currentFrame];
            resetFlag = true;
        }

        if (Time.time >= nextFrameTime)
        {
            currentFrame = (currentFrame + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrame];
            nextFrameTime = Time.time + frameTime;
        }
    }

    // Animation loop ??n gi?n (Move)
    private void PlayAnimation(Sprite[] frames, float frameTime)
    {
        if (frames == null || frames.Length == 0) return;

        if (Time.time >= nextFrameTime)
        {
            currentFrame = (currentFrame + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrame];
            nextFrameTime = Time.time + frameTime;
        }
    }

    // ??c giá tr? private field trong EnemyAI
    private bool GetPrivateBool(string fieldName)
    {
        var field = typeof(EnemyAI).GetField(fieldName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
            return (bool)field.GetValue(enemyAI);
        return false;
    }
}
