using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Gun : MonoBehaviour
{
    public Rigidbody bulletPrefab;
    public float bulletForce = 10f;
    public float timeBeforeDestroyingBullet = 3f;
    public Transform fireTransform;
    
    public void Fire()
    {
        Rigidbody bullet = Instantiate(bulletPrefab, fireTransform.position, fireTransform.rotation);
        bullet.AddRelativeForce(new Vector3(0f, 0f, bulletForce));
        StartCoroutine(DestroyAfterTime(bullet.gameObject, timeBeforeDestroyingBullet));
    }

    private IEnumerator DestroyAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        if (obj)
            Destroy(obj);
    }
}
