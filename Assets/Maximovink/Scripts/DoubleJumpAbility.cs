using UnityEngine;

public class DoubleJumpAbility : Ability
{
    private RagdollCharacter character;

    public bool Can = false;
    private bool lastIsGround = false;

    public float Delay = 0.1f;
    private float timer;

    private float force = 0f;

    public override void Init(RagdollCharacter character)
    {
        this.character = character;
    }

    protected override void ExecuteCommand(object command)
    {
        var axisInput = (command as AxisInput);
        var floatInput = (command as float?);


        if (axisInput != null && axisInput.Y > 0 && Can && timer > Delay)
        {
            jumpIsDirty = true;

            Can = false;
        } 

        if (floatInput != null)
        {
            force = (float)floatInput;
        }
    }

    private bool jumpIsDirty = false;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (jumpIsDirty)
        {
            character.AddForce(new Vector2(0, force));
            jumpIsDirty = false;
        }

        var isGround = character.IsGround;

        if (!isGround)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }

        if (isGround != lastIsGround&& character.RotIsNormal)
        {
            lastIsGround = isGround;
            if (!isGround)
            {
                Can = true;
            }
        }
    }
}
