
using UnityEngine;

public class SoundEventController : MonoBehaviour
{
    public AudioClip footstepSound;
    public AudioClip itemPickupSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Footstep()
    {
        audioSource.PlayOneShot(footstepSound);
    }

    public void PlayPickupSound()
    {
        audioSource.PlayOneShot(itemPickupSound);
    }
}
