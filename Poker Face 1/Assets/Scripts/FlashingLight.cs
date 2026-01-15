using UnityEngine;
using System.Collections;

public class CoroutineFlashingLight : MonoBehaviour
{
    public Light lightComponent;
    public float onDuration = 0.5f;
    public float offDuration = 0.5f;

    private Coroutine flashCoroutine;

    void Start()
    {
        if (lightComponent == null)
            lightComponent = GetComponent<Light>();

        StartFlashing();
    }

    public void StartFlashing()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    public void StopFlashing()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }
        lightComponent.enabled = true;
    }

    IEnumerator FlashRoutine()
    {
        while (true)
        {
            lightComponent.enabled = true;
            yield return new WaitForSeconds(Random.Range(1f, 3f));

            lightComponent.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }
    }

    // Custom flash pattern - SOS
    public void FlashSOS()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(SOSPattern());
    }

    IEnumerator SOSPattern()
    {
        while (true)
        {
            // S (3 short)
            for (int i = 0; i < 3; i++)
            {
                lightComponent.enabled = true;
                yield return new WaitForSeconds(0.2f);
                lightComponent.enabled = false;
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(0.4f);

            // O (3 long)
            for (int i = 0; i < 3; i++)
            {
                lightComponent.enabled = true;
                yield return new WaitForSeconds(0.6f);
                lightComponent.enabled = false;
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(0.4f);

            // S (3 short)
            for (int i = 0; i < 3; i++)
            {
                lightComponent.enabled = true;
                yield return new WaitForSeconds(0.2f);
                lightComponent.enabled = false;
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(2f);
        }
    }
}