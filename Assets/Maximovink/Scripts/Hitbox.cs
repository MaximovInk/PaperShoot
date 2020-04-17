using UnityEngine;
using System.Collections;

public class Hitbox : MonoBehaviour
{
    //private MonoBehaviour[] monoBehaviours;

    private void Awake()
    {
       // monoBehaviours = GetComponents<MonoBehaviour>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        /*if(monoBehaviours != null)
        for (int i = 0; i < monoBehaviours.Length; i++)
        {
            Destroy(monoBehaviours[i]);
        }
        monoBehaviours = null;

        gameObject.AddComponent<Rigidbody2D>();
        gameObject.layer = LayerMask.NameToLayer("Entity");
        Destroy(this);*/
    }
}
