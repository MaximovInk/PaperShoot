using UnityEngine;

public class IKCommonCharacter : Character
{
    protected override void Die()
    {
        base.Die();
        Rigidbody2D.freezeRotation = false;
        Destroy(MainCollider);
        Destroy(GetComponent<PlatformEffector2D>());
        AddPhysTo(RLegTarget);
        AddPhysTo(LLegTarget);
        AddPhysTo(RArmTarget);
        AddPhysTo(LArmTarget);
    }

    private void AddPhysTo(Transform t)
    {
        //t.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        var circle = t.gameObject.AddComponent<CircleCollider2D>();
        circle.radius = 0.1f;
        t.gameObject.AddComponent<Rigidbody2D>();
    }



    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsDead)
            return;

        var raycast = Physics2D.Raycast(Rigidbody2D.transform.position, Vector2.down, PlayerHeight, GroundMask);
        IsGround = IsStay = raycast;

        if (LLeg != null)
        {
            var startRayL = new Vector2(LLegTarget.transform.position.x, Rigidbody2D.transform.position.y);
            var l_leg_L = Physics2D.Raycast(startRayL, Vector2.down, PlayerHeight * 1.2f, GroundMask);
            if (l_leg_L)
            {
                LLegTarget.transform.position = new Vector3(LLegTarget.transform.position.x, Mathf.Lerp(LLegTarget.transform.position.y, l_leg_L.point.y, Time.fixedDeltaTime * 10f));
                LLeg.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(l_leg_L.normal.y, l_leg_L.normal.x) * Mathf.Rad2Deg - 90);
            }
            else if (IsGround)
            {
                LLegTarget.transform.position = new Vector3(LLegTarget.transform.position.x, Mathf.Lerp(LLegTarget.transform.position.y, raycast.point.y, Time.fixedDeltaTime * 10f));
                LLeg.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(raycast.normal.y, raycast.normal.x) * Mathf.Rad2Deg - 90);
            }
        }
        if (RLeg != null)
        {
            var startRayR = new Vector2(RLegTarget.transform.position.x, Rigidbody2D.transform.position.y);
            var l_leg_R = Physics2D.Raycast(startRayR, Vector2.down, PlayerHeight * 1.2f, GroundMask);
            if (l_leg_R)
            {
                RLegTarget.transform.position = new Vector3(RLegTarget.transform.position.x, Mathf.Lerp(RLegTarget.transform.position.y, l_leg_R.point.y, Time.fixedDeltaTime * 10f));
                RLeg.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(l_leg_R.normal.y, l_leg_R.normal.x) * Mathf.Rad2Deg - 90);
            }
            else if (IsGround)
            {
                RLegTarget.transform.position = new Vector3(RLegTarget.transform.position.x, Mathf.Lerp(RLegTarget.transform.position.y, raycast.point.y, Time.fixedDeltaTime * 10f));
                RLeg.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(raycast.normal.y, raycast.normal.x) * Mathf.Rad2Deg - 90);
            }
        }

        if (!RotIsNormal)
            RotIsNormal = true;

        if (IsGround && IsMovingX)
        {
            CalculateFootPositions();
        }
        if (!IsGround)
        {
            LLegTarget.transform.position = new Vector3(LLegTarget.transform.position.x, Mathf.Lerp(LLegTarget.transform.position.y, transform.position.y - PlayerHeight * 0.5f, Time.fixedDeltaTime * 10f));
            RLegTarget.transform.position = new Vector3(RLegTarget.transform.position.x, Mathf.Lerp(RLegTarget.transform.position.y, transform.position.y - PlayerHeight * 0.5f, Time.fixedDeltaTime * 10f));
        }


        return;
    }
}