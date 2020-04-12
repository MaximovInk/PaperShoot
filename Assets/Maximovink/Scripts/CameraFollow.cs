using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform clampTO;
    public float Speed = 2f;
    public float MaxDistance = 1f;

    private float z;

    private void Awake()
    {
        z = transform.position.z;
    }

    private void LateUpdate()
    {
        
        transform.position = (Vector3)Vector2.Lerp(
            transform.position,
            (Vector2)clampTO.position + Vector2.ClampMagnitude((Vector2)target.position-(Vector2)clampTO.position, MaxDistance),
            Time.deltaTime*Speed)+new Vector3(0,0,z);
        
        //transform.position = (Vector2)clampTO.position + Vector2.ClampMagnitude((Vector2)target.position-(Vector2)clampTO.position, MaxDistance);

        //transform.position = clampTO.position + Vector3.ClampMagnitude(clampTO.position - target.position, MaxDistance);
    }

}
