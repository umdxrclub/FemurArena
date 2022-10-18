using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    public void PlayHurtSound()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.Play();
    }
}