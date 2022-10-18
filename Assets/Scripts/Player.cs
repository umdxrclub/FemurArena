using UnityEngine;

/// <summary>
/// A script that represents the player. The only thing this can do is play the hurt sound.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// Plays an audible hurt sound.
    /// </summary>
    public void PlayHurtSound()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.Play();
    }
}