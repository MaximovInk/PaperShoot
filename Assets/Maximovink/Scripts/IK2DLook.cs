using UnityEngine;

[ExecuteInEditMode]
public class IK2DLook : MonoBehaviour
{
    public Transform Target;

    public float AngleStart;

    public bool Inverse = false;

    void Update()
    {
        if (Target == null)
            return;

        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(Target.position.y - transform.position.y, Target.position.x - transform.position.x) * Mathf.Rad2Deg + AngleStart + (Inverse?180:0));
    }
}
