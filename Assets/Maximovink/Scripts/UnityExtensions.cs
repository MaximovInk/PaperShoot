using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtensions
{
    public static void AddTorqueAtPosition(this Rigidbody2D rb, float torque, Vector2 rotationPoint, ForceMode2D forceMode = ForceMode2D.Force)
    {
        rb.AddForceAtPosition(-Vector2.up * torque * rb.inertia, rotationPoint + Vector2.right, forceMode);
        rb.AddForceAtPosition(Vector2.up * torque * rb.inertia, rotationPoint - Vector2.right, forceMode);
    }
}

