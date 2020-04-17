using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb2d;

    public float Damage = 1f;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        var vec = collision.GetContact(0).normal;
        //var vec = savedPos-(Vector2)transform.position;
        var rot = Quaternion.Euler(Mathf.Atan2(-vec.y, vec.x) * Mathf.Rad2Deg,90, -90);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            var blood = Instantiate(GameManager.Instance.BloodParticle, collision.GetContact(0).point, rot);
            Destroy(blood, 3f);
        }
        else
        {
            var spark = Instantiate(GameManager.Instance.SparkParticle, collision.GetContact(0).point, rot);
            Destroy(spark, 3f);
        }

        //Instantiate(GameManager.Instance.FleshHole, collision.GetContact(0).point, Quaternion.identity);

        Destroy(gameObject);
    }

    private Vector2 savedPos; 
    private void LateUpdate()
    {
        savedPos = transform.position;
    }
}