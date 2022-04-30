using System.Collections;
using UnityEngine;

public class trigger : MonoBehaviour
{
    // Start is called before the first frame update
    public Animation SlideDoor;
    bool done;
    public void TargitHit(int damage)
    {
        if (!done)
        {
            if (SlideDoor != null)
                SlideDoor.Play();
            StartCoroutine(FlipTarget());
        }
    }

    public IEnumerator FlipTarget()
    {
        if (!done)
        {
            done = true;
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0, transform.parent.eulerAngles.y);
            curve.AddKey(1, transform.parent.eulerAngles.y - 90);
            curve.SmoothTangents(0, 0);
            curve.SmoothTangents(1, 0);
            for (float i = 0; i < 1; i += 0.01f)
            {
                Vector3 newRotation = transform.parent.eulerAngles;
                newRotation.y = curve.Evaluate(i);
                transform.parent.eulerAngles = newRotation;
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public IEnumerator FlipTargetBack()
    {
        if (done)
        {
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0, transform.parent.eulerAngles.y);
            curve.AddKey(1, transform.parent.eulerAngles.y + 90);
            curve.SmoothTangents(0, 0);
            curve.SmoothTangents(1, 0);
            for (float i = 0; i < 1; i += 0.01f)
            {
                Vector3 newRotation = transform.parent.eulerAngles;
                newRotation.y = curve.Evaluate(i);
                transform.parent.eulerAngles = newRotation;
                yield return new WaitForFixedUpdate();
            }
            done = false;
        }
    }
}
