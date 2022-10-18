using System.Collections;
using UnityEngine;

/// <summary>
/// A gun is an interactable object that when activated fires bullets.
/// </summary>
public class Gun : MonoBehaviour
{
    /// <summary>
    /// The prefab of the bullet.
    /// </summary>
    public Rigidbody bulletPrefab;
    
    /// <summary>
    /// The force to apply to the bullet.
    /// </summary>
    public float bulletForce = 10f;
    
    /// <summary>
    /// The time to wait before destroying the bullet.
    /// </summary>
    public float timeBeforeDestroyingBullet = 3f;
    
    /// <summary>
    /// The transform to fire bullets out of.
    /// </summary>
    public Transform fireTransform;
    
    /// <summary>
    /// Fires a bullet out of the gun.
    /// </summary>
    public void Fire()
    {
        // ??? - Create bullet
        
        // ??? - Add bullet force
        
        // ??? - Start a coroutine to destroy the bullet after "timeBeforeDestroyingBullet" time.
    }

    /// <summary>
    /// A coroutine to destroy a GameObject after a specified amount of time. This is used to ensure that bullets that
    /// miss their targets are eventually destroyed.
    /// </summary>
    /// <param name="obj">The GameObject to destroy</param>
    /// <param name="time">The time before destroying the bullet</param>
    private IEnumerator DestroyAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        if (obj)
            Destroy(obj);
    }
}
