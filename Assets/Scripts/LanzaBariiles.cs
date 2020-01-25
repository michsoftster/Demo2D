using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzaBariiles : MonoBehaviour
{
    public Vector2 barrilDirection;
    public GameObject barril;
    public Transform posbarril;
    private GameObject tmpBarril;

    void Start()
    {
        StartCoroutine("creaBarril");
    }

    IEnumerator creaBarril()
    {
        tmpBarril = Instantiate(barril, posbarril.position, Quaternion.identity);
        tmpBarril.GetComponent<Rigidbody2D>().AddForce(barrilDirection);
        yield return new WaitForSeconds(3f);
        StartCoroutine("creaBarril");
    }

}
