using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject BloodParticle;
    public GameObject BloodLongParticle;
    public GameObject SparkParticle;
    public GameObject FleshHole;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
}