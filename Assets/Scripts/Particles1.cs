using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles1 : MonoBehaviour
{
    public ParticleSystem particleSystem; // Assign in Inspector.
    public float pulseDuration = 1f; // Duration for the particle system to stay on.

    // Start is called before the first frame update

    [SerializeField] Playermovement_2 PM;
    void Start()
    {
        if (particleSystem == null)
            particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }

    // Update is called once per frame
    public void StartPulse()
    {
        StartCoroutine(Pulse());
    }

    private IEnumerator Pulse()
    {
        // hello
        particleSystem.Play();
        yield return new WaitForSeconds(pulseDuration);
        particleSystem.Stop();
    }
    public void Update()
    {
        if(PM.Bleed == true)
        {
            particleSystem.Play();
            StartPulse();
        }
    }

}
