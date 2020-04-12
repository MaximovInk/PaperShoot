using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public Transform BulletStart;
    public Rigidbody2D BulletPrefab;
    public float force = 10f;

    public float AccuracityAdd = 5f;

    private float StartAngle;
    private float CurrentAngle;

    public float MaxAngle = 20f;

    public float SpeedNormilzing = 2f;

    public float AnimTimer = 0.1f;
    public GameObject FireAnim;

    private void Awake()
    {
        StartAngle = transform.localEulerAngles.z;
        CurrentAngle = StartAngle;
    }

    private void Update()
    {
        CurrentAngle = Mathf.Lerp(Mathf.Clamp( CurrentAngle,StartAngle,StartAngle+MaxAngle),StartAngle,Time.deltaTime);

        transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.x,transform.localRotation.y,CurrentAngle));
    }

    public void Fire(bool flipped)
    {
        var bullet = Instantiate(BulletPrefab, BulletStart.transform.position, BulletStart.rotation);
        bullet.velocity = bullet.transform.right*force*(flipped?-1:1);
        Destroy(bullet.gameObject, 10f);

        CurrentAngle += AccuracityAdd;

        StopAllCoroutines();
        FireAnim.SetActive(true);
        StartCoroutine(SetActiveTimer());
    }

    IEnumerator SetActiveTimer()
    {

        yield return new WaitForSeconds(AnimTimer);

        FireAnim.SetActive(false);
    }
    
}
