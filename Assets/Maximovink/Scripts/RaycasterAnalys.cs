using UnityEngine;
using System.Collections;

public class RaycasterAnalys : MonoBehaviour
{
    public float MaxLenght = 0f;
    public float MinLenght = 10f;

    public LayerMask layerMask;

    private void FixedUpdate()
    {
        var raycast = Physics2D.Raycast(transform.position,Vector2.down,Mathf.Infinity,layerMask);

        if (raycast)
        {
            MaxLenght = Mathf.Max(MaxLenght, raycast.distance);
            MinLenght = Mathf.Min(MinLenght, raycast.distance);

        }
    }
}
