using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform bar;

    public void SetSize(float sizeNormalized)
    {
        bar = transform.Find("Bar");
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }
}
