﻿using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    public bool IsGround { get; protected set; }
    public bool IsStay { get; protected set; }
    public bool RotIsNormal { get; protected set; }
    public bool IsMovingX { get; protected set; }

    public float FootsXFlip = -1f;
    public float FootsX = -0.4f;

    [Header("Поверхность")]
    public LayerMask GroundMask;
    [Header("Rigidbody персонажа")]
    public Rigidbody2D Rigidbody2D;
    public List<Ability> abilities = new List<Ability>();

    public float MoveXThreshold = 0.2f;

    [Header("Скорость перемещения ног")]
    public float FootsSpeed = 4f;

    [Header("Расстояние между ногами")]
    public float FootsDistance = 1f;

    [Header("For modifications")]
    public Transform HeadTarget;
    public Transform LArmTarget;
    public Transform RArmTarget;
    public Transform LLegTarget;
    public Transform RLegTarget;

    public Transform RHand;
    public Transform LHand;
    public Transform Body;
    public Transform LLeg;
    public Transform RLeg;

    public IK2DLook HeadLook;
    public List<Transform> FlipObjs = new List<Transform>();
    public List<Transform> FlipObjsY = new List<Transform>();
    public List<IK2DFabrik> FlipFabriks = new List<IK2DFabrik>();
    public List<SpriteRenderer> FlipSprites = new List<SpriteRenderer>();

    public float LegsInAirStopping = 0.8f;

    public bool Flip { get => flip; set { if (value != flip) DoFlip(); } }
    private bool flip = false;

    private void DoFlip()
    {
        flip = !flip;

        for (int i = 0; i < FlipObjs.Count; i++)
        {
            FlipObjs[i].localScale = new Vector3(FlipObjs[i].localScale.x * -1, 1, 1);
        }
        for (int i = 0; i < FlipObjsY.Count; i++)
        {
            FlipObjsY[i].localScale = new Vector3(1,FlipObjsY[i].localScale.y * -1, 1);
        }
        for (int i = 0; i < FlipFabriks.Count; i++)
        {
            FlipFabriks[i].Inverse = !FlipFabriks[i].Inverse;
        }
        for (int i = 0; i < FlipSprites.Count; i++)
        {
            FlipSprites[i].flipX = !FlipSprites[i].flipX;
        }
        HeadLook.Inverse = flip;
    }

    protected void CalculateFootPositions()
    {
        var x_r = Mathf.Round(Rigidbody2D.position.x);

        if (true)
        {
            var x = x_r + (x_r % 2 == 0 ? 0f : FootsDistance / 2f) + (Flip ? FootsXFlip : FootsX);

           // var dist = Mathf.Abs(x - LLegTarget.position.x) / 4f;

            LLegTarget.position = Vector2.Lerp(LLegTarget.position, new Vector2(x, LLegTarget.position.y), Time.deltaTime * FootsSpeed);
        }

        if (true)
        {
            var x = x_r + (x_r % 2 == 0 ? FootsDistance / 2f : 0f) + (Flip ? FootsXFlip : FootsX);


         //   var dist = Mathf.Abs(x - RLegTarget.position.x) / 4f;

            RLegTarget.position = Vector2.Lerp(RLegTarget.position, new Vector2(x, RLegTarget.position.y ), Time.deltaTime * FootsSpeed);
        }

    }

    protected virtual void FixedUpdate()
    {
        IsMovingX = Mathf.Abs(Rigidbody2D.velocity.x) > MoveXThreshold;



       /* var l_raycast = Physics2D.CircleCast(LLegTarget.position, 0.05f, Vector2.up, 0.05f ,GroundMask);

        var r_raycast = Physics2D.CircleCast(RLegTarget.position, 0.05f, Vector2.up, 0.05f, GroundMask);

        if (l_raycast)
        {
            LLegTarget.position += new Vector3(0,0.05f,0);
        }

        if (r_raycast)
        {
            RLegTarget.position += new Vector3(0, 0.05f, 0);
        }*/
    }

    public virtual void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
    {
        Rigidbody2D.AddForce(force, mode);
    }

    public virtual void SetVelocityX(float value)
    {
        Rigidbody2D.velocity = new Vector2(value, Rigidbody2D.velocity.y);
    }

    public virtual void SetVelocityY(float value)
    {
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, value);
    }

    public virtual void SetRotation(float value)
    {
        Rigidbody2D.rotation = value;
    }

    public virtual void SetAngularVelocity(float value)
    {
        Rigidbody2D.angularVelocity = value;
    }


}