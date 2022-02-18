using UnityEngine;
public enum GasCanAnimationState
{
    TowardGenerator,
    Shaking,
    BackToPlayer,
    Held
}

public class GasCan : MonoBehaviour
{
    const int maxAnimationFrames = 300;
    int frameSinceLastPour = 5;
    int animationFrame = 0;
    Vector3 oldPostion;
    Vector3 newPostion;
    public Vector3 gasCanShakeOffSet = new Vector3(0, 2, 0);
    public float shakeHeight = 0.75f;
    OpenGasCap gasCap;
    [HideInInspector]
    public pick_up pickUp;
    public GasCanAnimationState state = GasCanAnimationState.Held;

    public void ContinuePouringGas()
    {
        frameSinceLastPour = 0;
        if (state == GasCanAnimationState.Held)
        {
            ChangeState(GasCanAnimationState.TowardGenerator);
        }
    }
    void ChangeState(GasCanAnimationState newState)
    {
        if (newState != state)
        {
            if (newState == GasCanAnimationState.TowardGenerator)
            {
                oldPostion = transform.position;
                newPostion = gasCap.transform.position + gasCanShakeOffSet;

            }
            animationFrame = 0;
            state = newState;
        }
    }
    void GasCanPourAnimation()
    {
        if (state == GasCanAnimationState.TowardGenerator)
        {
            float interpolateTime = (float)animationFrame / 30;
            //The product of a Lerp is called a lerpual, lol.
            Vector3 lerpual = Vector3.Lerp(oldPostion, newPostion, interpolateTime);
            transform.position = lerpual;
            if (interpolateTime >= 1)
            {
                state = GasCanAnimationState.Shaking;
            }
        }
        animationFrame++;
    }
    private void Update()
    {
        if (frameSinceLastPour < 2 && animationFrame < maxAnimationFrames)
        {
            pickUp.constrainHeldItem = false;

            frameSinceLastPour++;
            GasCanPourAnimation();
        }
        else if (state == GasCanAnimationState.Shaking)
        {
            state = GasCanAnimationState.BackToPlayer;
            animationFrame = 0;
            GasCanPourAnimation();

        }
        else if (state == GasCanAnimationState.BackToPlayer)
        {
            GasCanPourAnimation();

        }
    }
    private void Awake()
    {
        pickUp = GetComponent<pick_up>();
        gasCap = FindObjectOfType<OpenGasCap>();
    }
}
