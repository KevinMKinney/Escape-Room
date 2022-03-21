using System.Collections;
using UnityEngine;

public class TargetColorChange : MonoBehaviour
{
    public float startingY;
    void SetColor()
    {
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        Material material = TargetScriptManager.instance.availableTargetMaterial.Pop();
        mesh.sharedMaterial = material;
    }
    public void ResetTarget()
    {
        SetColor();
        StopCoroutine(GetComponent<trigger>().FlipTarget());
        StartCoroutine(GetComponent<trigger>().FlipTargetBack());
    }
    
    public void Start()
    {
        startingY = transform.parent.eulerAngles.y;
        TargetScriptManager.instance.onReset += ResetTarget;
        TargetScriptManager.instance.newColor += () =>
        {
            SetColor();
        };
    }
    public void TargitHit(float _)
    {
        StartCoroutine(RegisterHit());
    }
    public IEnumerator RegisterHit()
    {
        yield return new WaitForSeconds(2);
        TargetScriptManager.instance.FiredAtTarget(this.GetComponent<MeshRenderer>().sharedMaterial);
    }
}
