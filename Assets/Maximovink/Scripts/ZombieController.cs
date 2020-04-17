using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public float Damage = 1f;
    public float DamageDelay = 0.5f;
    private float DamageTimer = 0f;

    public bool LLeg { get => lleg; set { lleg = value; UpdateLimbs(); } } 
    private bool lleg = true;
    public bool RLeg { get => rleg; set { rleg = value; UpdateLimbs(); } }
    private bool rleg = true;
    public bool LArm { get => larm; set { 
             larm = value; UpdateLimbs();
        } }
    private bool larm = true;
    public bool RArm { get => rarm; set { 
            rarm = value; UpdateLimbs();
           
        } }
    private bool rarm = true;
    public bool Head { get => head; set { head = value; UpdateLimbs(); } }
    private bool head = true;

    public float AttackDistance = 1f;
    private float startAttackDistance;

    public Object[] DestroyOnLegsRemove;
    public Rigidbody2D rb;

    public Transform enemy;

    protected Character character;

    public LayerMask AttackMask;

    public float JumpDistance = 2f;
    public float JumpSpeed = 1f;
    public float MoveSpeed = 1f;

    public float CrouchSpeed;

    public bool IsCrouch { get => !lleg || !rleg; }

    public float MoveStopThreshold = 1f;

    private float moveDelta;

    public float StopDelay = 1f;
    private float stopTimer = 0f;

    public Transform LeftX;
    public Transform RightX;

    private void Start()
    {
        startAttackDistance = AttackDistance;
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (character.IsDead)
            return;

        if (enemy == null)
            return;

        if (enemy.position.x < transform.position.x)
            character.Flip = true;
        if (enemy.position.x > transform.position.x)
            character.Flip = false;
    }

    private float la = 0;
    private float ra = 0;

    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }

    private void FixedUpdate()
    {
        if (character.IsDead)
            return;

        la = Mathf.Lerp(la,Mathf.Abs( Mathf.Sin(Time.time*6f)),Time.fixedDeltaTime*10f);
        ra = Mathf.Lerp(ra,Mathf.Abs( Mathf.Cos(Time.time*6f)), Time.fixedDeltaTime * 10f);



        if (enemy != null)
        {
            if (larm)
            {
                var pos = enemy.position + new Vector3(0, la, 0);
                pos = (Vector2)transform.position + Vector2.ClampMagnitude((Vector2)pos - (Vector2)transform.position, 1.3f);

                character.LArmTarget.position = pos;
            }

            if (rarm)
            {
                var pos = enemy.position + new Vector3(0, ra, 0);
                pos = (Vector2)transform.position + Vector2.ClampMagnitude((Vector2)pos - (Vector2)transform.position, 1.3f);

                character.RArmTarget.position = pos;
            }
                

            character.HeadTarget.position = enemy.position + new Vector3(0, 0.8f, 0);

            DamageTimer += Time.fixedDeltaTime;
            if (DamageTimer > DamageDelay)
            {

                var attackDir = enemy.transform.position - character.HeadLook.transform.position;

                var attackRaycast = Physics2D.RaycastAll(character.HeadLook.transform.position, attackDir.normalized, Mathf.Min(attackDir.magnitude, AttackDistance));

                if (attackRaycast.Length > 1)
                {
                    for (int i = 0; i < attackRaycast.Length; i++)
                    {
                        var main = attackRaycast[i].collider.GetComponentInParent<Character>();
                        if (main != null && main != character)
                        {
                            main.Health -= Damage;
                            DamageTimer = 0f;
                        }
                    }
                }
            }


            if (lleg || rleg || rarm || larm)
            {
                var dist = enemy.position.x - character.transform.position.x;

                if (Mathf.Abs(dist) > AttackDistance)
                {
                    var dir = Mathf.Sign(dist);



                    if (!IsCrouch)
                    {

                        var raycast = Physics2D.Raycast(transform.position - new Vector3(0, character.PlayerHeight * 0.6f, 0), new Vector2(dir, 0), Mathf.Min(dist, JumpDistance), character.GroundMask);
                        if (raycast && character.IsGround)
                        {
                            character.SetVelocityY(JumpSpeed);
                        }
                    }
                    else
                    {
                        rb.rotation = Mathf.Lerp(rb.rotation, dir > 0 ? -90 : 90, Time.deltaTime * 5f);
                    }


                    character.SetVelocityX(dir * (IsCrouch ? CrouchSpeed : MoveSpeed));
                    moveDelta = character.Rigidbody2D.velocity.x;
                    if (moveDelta == 0)
                    {
                        stopTimer += Time.fixedDeltaTime;

                        if (stopTimer > StopDelay)
                        {
                            enemy = null;
                        }
                    }
                    else
                    {
                        stopTimer = 0;
                    }
                }
                else
                {
                    stopTimer = 0;
                }
            }

            
        }
    }

    private void UpdateLimbs()
    {
        if (!lleg || !rleg)
        {
            rb.freezeRotation = false;
            rb.angularDrag = 1f;
            if (DestroyOnLegsRemove != null)
                for (int i = 0; i < DestroyOnLegsRemove.Length; i++)
                {
                    Destroy(DestroyOnLegsRemove[i]);
                }
            DestroyOnLegsRemove = null;
        }
        if (!larm && !rarm)
        {
            AttackDistance = startAttackDistance / 3f;
        }
    }
}
