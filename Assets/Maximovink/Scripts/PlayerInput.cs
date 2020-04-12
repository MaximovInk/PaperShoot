using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerInput : MonoBehaviour
{
    private Character Character;

    public float Speed = 3f;
    public float JumpSpeed = 200f;
    public float SitSpeed = 20f;

    public Transform HeadTarget;
    public Transform ArmTarget;

    public float LimbsFactor = 0.2f;

    private DoubleJumpAbility ability;

    public float ArmMaxDistance;

    public Gun Gun1;
    public Gun Gun2;

    private Vector2 IN;

    private void Awake()
    {
        Character = GetComponent<Character>();

        ability = new DoubleJumpAbility();
        ability.SendCommand(JumpSpeed);

        Character.abilities.Add(ability);
    }

    private void FixedUpdate()
    {
        var mult = Input.GetKey(KeyCode.LeftShift) ? 1.5f : 1;

        if (IN.x > 0)
        {
            if (Character.RotIsNormal)
                Character.SetVelocityX(Speed * mult);

        }

        if (IN.x < 0)
        {
            if (Character.RotIsNormal)
                Character.SetVelocityX(-Speed * mult);

        }

        if (IN.y>0)
        {
            if (Character.IsGround && Character.RotIsNormal)
            {
                Character.SetVelocityY(JumpSpeed);
                Character.SetAngularVelocity(0);
                Character.SetRotation(0);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            ability.SendCommand(new AxisInput(0, 1));
        }
        
        if (IN.y < 0)
        {
            if (Character.IsGround && Character.RotIsNormal)
                Character.AddForce(new Vector2(0, -SitSpeed), ForceMode2D.Force);

        }
    }

    private void Update()
    {
        HeadTarget.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ArmTarget.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        IN = Vector2.zero;
        var mult = Input.GetKey(KeyCode.LeftShift);

        if (HeadTarget.transform.position.x < transform.position.x)
        {
            Character.Flip = true;
        }
        if (HeadTarget.transform.position.x > transform.position.x)
        {
            Character.Flip = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            //Character.Flip = false;
            IN.x += 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            //Character.Flip = true;
            IN.x -= 1;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Gun1.Fire(Character.Flip);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Gun2.Fire(Character.Flip);
        }

        if (mult)
            IN.x *= 1.5f;

        if (Input.GetKeyDown(KeyCode.W))
        {
            IN.y += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            IN.y -= 1;
        }
    }
}
