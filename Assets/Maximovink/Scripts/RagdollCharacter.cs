using System.Collections.Generic;
using UnityEngine;

public class RagdollCharacter : Character
{
    [Header("Скорость при которой персонаж начинает падать")]
    public float FallSpeed = 1f;
    [Header("Таймер начала выпрямления спины после падения")]
    public float forceDelay = 1f;
    [Header("Таймер начала поднятия тела после падения")]
    public float rotDelay = 0.5f;

    [Header("Высота персонажа")]
    public float PlayerHeight;

    private float rotTimer = 0;
    private float forceTimer = 0;
    [Header("Фактор силы поднятия")]
    public float ForceFactor = 0.7f;
    [Header("Фактор силы выпрямления")]
    public float RotFactor = 0.7f;
    [Header("Мертвая зона вращения")]
    public float RotThresold = 1f;
    [Header("Наклон, при котором персонаж падает")]
    public float RotFall = 30f;
    [Header("Максимальная дистанция (1-высота персонажа),на которой начнется выпрямление спниы")]
    public float MaxDistanceRotRelative = 2;

    private List<Rigidbody2D> limbs = new List<Rigidbody2D>();

    public bool stay;
    public bool ground;
    public bool normalRot;

    public Vector2 DragFactor = Vector2.one;
    public Vector2 LimbsDragFactor = Vector2.one;

    private void Start()
    {
        if (abilities != null)
            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities == null)
                    continue;

                abilities[i].Init(this);
            }
    }

    private void Update()
    {
        if (abilities != null)
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                abilities[i].Update();
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Rigidbody2D.velocity *= DragFactor;
        for (int i = 0; i < limbs.Count; i++)
        {
            limbs[i].velocity *= LimbsDragFactor;
        }

        stay = IsStay;
        normalRot = RotIsNormal;
        ground = IsGround;

        var raycast = Physics2D.Raycast(Rigidbody2D.transform.position, Vector2.down, PlayerHeight, GroundMask);

        IsGround = raycast && Mathf.Abs(Rigidbody2D.velocity.magnitude) < FallSpeed;
        RotIsNormal = Mathf.Abs(Rigidbody2D.rotation) < RotFall;

        if (IsGround)
        {
            if (RotIsNormal && IsMovingX)
            {
                CalculateFootPositions();
            }
            else
            {
                forceTimer = 0f;
                
                IsStay = false;
            }

            if (raycast.distance / PlayerHeight > MaxDistanceRotRelative)
            {
                rotTimer = 0f;
            }

            rotTimer += Time.deltaTime;
            forceTimer += Time.deltaTime;
        }
        else
        {
            forceTimer = 0;
            IsStay = false;
        }
        if (rotTimer > rotDelay)
        {
            rotTimer = rotDelay;

            var rot_err = -Rigidbody2D.rotation;

            if (Mathf.Abs(rot_err) > RotThresold)
                Rigidbody2D.angularVelocity = rot_err * RotFactor;

        }
        if (forceTimer > forceDelay)
        {
            forceTimer = forceDelay;

            var force_err = (PlayerHeight - raycast.distance) * 0.8f;

            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, force_err*ForceFactor+AddToVelocityY);

            AddToVelocityY = 0;
            IsStay = force_err < PlayerHeight * 0.2f;
        }
        else if(AddToVelocityY != 0)
        {
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, AddToVelocityY);

            AddToVelocityY = 0;
        }
        if (abilities != null)
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                abilities[i].FixedUpdate();
            }
        }
    }

    private float AddToVelocityY;


}
