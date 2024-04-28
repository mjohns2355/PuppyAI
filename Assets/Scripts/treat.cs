using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treat : MonoBehaviour
{

    private Vector3 pos;
    private void Start()
    {
        pos = transform.position;
        gameObject.SetActive(false);
    }

    public void Spawn()
    {
        transform.position = pos;
        GetComponent<Rigidbody>().AddForce((Vector3.up - Vector3.forward) * 100);
    }


}
