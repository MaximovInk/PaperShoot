using UnityEngine;

public class RotationResetter : MonoBehaviour
{
    private void LateUpdate()
    {
        if (Quaternion.identity != transform.rotation)
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
