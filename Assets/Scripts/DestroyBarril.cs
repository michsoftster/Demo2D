using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBarril : MonoBehaviour
{
    public GameObject Explosion;
   
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Player")
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

    }
}
