using UnityEngine;
using UnityEngine.Events;

public class HitboxPart : MonoBehaviour
{
    public UnityEvent OnHit;
    public UnityEvent OnDestroy;

    public float DestroyDamage;
    private float GettedDamage;

    private Character character;

    public bool CanDestroy = true;

    private void Awake()
    {
        character = GetComponentInParent<Character>();
    }

    public void Destroy()
    {
        Destroy(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            var dmg = collision.gameObject.GetComponent<Bullet>().Damage;
            GettedDamage += dmg;

            character.Health -= dmg;

            OnHit.Invoke();

            if (GettedDamage >= DestroyDamage)
            {
                if (CanDestroy)
                {
                    var blood = Instantiate(GameManager.Instance.BloodLongParticle, collision.GetContact(0).point, Quaternion.identity);
                    var copy = Instantiate(gameObject, transform.position, transform.rotation);

                    blood.transform.SetParent(copy.transform);

                    var toDestroy = copy.GetComponentsInChildren<MonoBehaviour>();
                    for (int i = 0; i < toDestroy.Length; i++)
                    {
                        Destroy(toDestroy[i]);
                    }

                    copy.AddComponent<Rigidbody2D>();
                    gameObject.SetActive(false);
                }

                OnDestroy.Invoke();
            }

        }

    }
}
