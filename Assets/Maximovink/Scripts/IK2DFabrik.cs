using UnityEngine;

[ExecuteAlways]
public class IK2DFabrik : MonoBehaviour
{
    public int ChainLength = 2;

    public Transform Target;

    public int Iterations = 10;

    private float[] BonesLength;
    private float CompleteLength;
    private Transform[] Bones;
    private Vector2[] Positions;

    public bool Inverse = false;

    public float Delta = 0.01f;

    void Awake()
    {
        Init();
    }

    private void OnValidate()
    {
        

    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        var current = transform;
        for (int i = 0; i < ChainLength && current != null && current.childCount > 0; i++)
        {
            var child = current.GetChild(0);
            var scale = Vector3.Distance(current.position, child.position) * 0.1f;
            UnityEditor.Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, child.position - current.position), new Vector3(scale, Vector3.Distance(child.position, current.position), scale));
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);

            current = child;
        }

    }
#endif

    public Vector2 DirFromAngle(float angleInDegree)
    {
        return new Vector2(Mathf.Sin(angleInDegree * Mathf.Deg2Rad), Mathf.Cos(angleInDegree * Mathf.Deg2Rad));
    }

    private Vector2 PerpendicularLine(Vector2 P1, Vector2 P2)
    {
        var v = P2 - P1;
        return new Vector2(-v.y, v.x) / Mathf.Sqrt(v.x * v.x + v.y * v.y);
    }

    void Init()
    {
        Bones = new Transform[ChainLength + 1];
        Positions = new Vector2[ChainLength + 1];
        BonesLength = new float[ChainLength];

        var current = transform;
        CompleteLength = 0;
        for (var i = 0; i < Bones.Length; i++)
        {
            Bones[i] = current;
            Positions[i] = Bones[i].position;

            if (i < ChainLength)
            {
                current = current.GetChild(0);
            }

            if (i > 0)
            {
                BonesLength[i - 1] = Vector2.Distance(Bones[i - 1].position, Bones[i].position);
                CompleteLength += BonesLength[i - 1];
            }
        }
    }

    private void Update()
    {
        ResolveIK();
    }


    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    void ResolveIK()
    {
        if (Target == null)
            return;

        if (BonesLength.Length != ChainLength || !Target.gameObject.activeSelf)
        {
            Init();
        }

        for (int i = 0; i < Bones.Length; i++)
        {
            Positions[i] = Bones[i].position;
        }

        Vector2 targetPos = Target.position;

        if (Vector2.Distance(transform.position, targetPos) > CompleteLength)
        {
            var direction = ((Vector2)Target.position - Positions[0]).normalized;

            for (int i = 1; i < Positions.Length; i++)
                Positions[i] = Positions[i - 1] + direction * BonesLength[i - 1];
        }

        for (int iteration = 0; iteration < Iterations; iteration++)
        {
            for (int i = Positions.Length - 1; i > 0; i--)
            {
                if (i == Positions.Length - 1)
                {
                    Positions[i] = targetPos;
                }
                else
                {
                    Positions[i] = Positions[i + 1] + (Positions[i] - Positions[i + 1]).normalized * BonesLength[i];
                }


                //Positions[i] = P0 + DirFromAngle(angle).normalized*BonesLength[i-1];
            }

            for (int i = 1; i < Positions.Length; i++)
                Positions[i] = Positions[i - 1] + (Positions[i] - Positions[i - 1]).normalized * BonesLength[i - 1];

            if (Vector2.Distance(Positions[Positions.Length - 1], targetPos) < Delta)
                break;
        }

        for (int i = 1; i < Positions.Length - 1; i++)
        {
            var center = (Positions[i - 1] + Positions[i + 1]) / 2;

            var distToCenter = Vector2.Distance(Positions[i], center);

            Positions[i] = center + distToCenter * (PerpendicularLine(Positions[i - 1], Positions[i + 1]) * (Inverse ? -1 : 1)).normalized;
        }

        for (int i = 0; i < Positions.Length; i++)
        {
            Bones[i].position = Positions[i];
        }
    }
}
