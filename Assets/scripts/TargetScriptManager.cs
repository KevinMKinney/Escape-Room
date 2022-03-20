using System.Collections.Generic;
using UnityEngine;

public class TargetScriptManager : MonoBehaviour
{
    public static TargetScriptManager instance;
    public event System.Action onReset = () => { };
    public event System.Action newColor = () => { };
    public List<Material> materials = new();
    public Stack<Material> targetColorOrder = new();
    public Stack<Material> availableTargetMaterial = new();
    public GameObject gameObjectToEnable;

    public void ResetTarget()
    {
        availableTargetMaterial.Clear();
        List<Material> targetMaterials = new();
        foreach (Material material in materials)
        {
            targetMaterials.Add(material);
        }
        targetMaterials.Rearrange();
        foreach (Material material in targetMaterials)
        {
            availableTargetMaterial.Push(material);
        }

        targetColorOrder.Clear();
        foreach (Material copy in materials)
        {
            targetColorOrder.Push(copy);
        }
    }
    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        materials.Rearrange();
        ResetTarget();
        newColor();
    }

    public void FiredAtTarget(Material self)
    {
        if (targetColorOrder.Count != 0)
        {
            Material target = targetColorOrder.Pop();
            if (self != target)
            {
                ResetTarget();
                onReset();
            }
            else if (targetColorOrder.Count == 0)
            {
                gameObjectToEnable.SetActive(true);
                onReset = () => { };
            }
        }
    }
}
