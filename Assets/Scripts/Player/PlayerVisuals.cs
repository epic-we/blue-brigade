using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private AudioClip[] _stepSounds;
    [SerializeField] private ParticleSystem _hitPs;
    public void PlayHitParticles()
    {
        _hitPs.Play();
    }

    public void PlayWalkSound()
    {
        AudioSystem.PlaySound(_stepSounds);
    }
}
