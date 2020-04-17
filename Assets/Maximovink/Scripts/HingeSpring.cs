using UnityEngine;

public class HingeSpring : MonoBehaviour
{

    HingeJoint2D hinge;
    Rigidbody2D rb;

    public float force = 10;
    public float minAngle = 5;

    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {

        float targetForce = hinge.referenceAngle - hinge.jointAngle;
        targetForce = Mathf.Sign(targetForce) * Mathf.Max(0, Mathf.Abs(targetForce) - minAngle);

        rb.AddTorqueAtPosition(force * targetForce, transform.TransformPoint(hinge.anchor));
    }
}