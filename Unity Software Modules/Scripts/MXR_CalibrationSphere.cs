using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MXR_CalibrationSphere : MonoBehaviour
{
    public float duration = 1f;
    
    public Vector3 startSize = new Vector3();
    public Vector3 targetSize = new Vector3();

    private bool once = false;

    void Update()
    {
        if (!once) { StartCoroutine(ScaleObject()); once = true; }
    }

    IEnumerator ScaleObject()
    {
        Vector3 startScale = this.transform.localScale;
        float time = 0f;
        Vector3 target = this.transform.localScale == targetSize ? startSize : targetSize;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        this.transform.localScale = target;
        StartCoroutine(ScaleObject());
    }

    private void OnDisable()
    {
        once = false;
    }
}
