using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float MinScale;
    public float MaxScale;

    public Transform mask;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">Range 0-1</param>
    public void SetValue(float value)
    {
        mask.localScale = new Vector3(Mathf.Lerp(MinScale,MaxScale,value),mask.localScale.y,mask.localScale.z);
    }
}
