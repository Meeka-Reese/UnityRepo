using UnityEngine;

public class ParticleSystemFrameScale : MonoBehaviour
{
    public int fps;
    ParticleSystem particle;
    private float timeElapsed = 0f;
    private float displayTime = 0f;

    private void OnEnable()
    {
        particle = GetComponent<ParticleSystem>();
    }
    void LateUpdate()
    {
        timeElapsed += Time.deltaTime;

        if ((timeElapsed - displayTime) > 1f / fps)
        {
            displayTime = timeElapsed;
            particle.Simulate(.1f, true, false, false);
            particle.Pause();
        }

    }
}
