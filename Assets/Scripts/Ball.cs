using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private Rigidbody myRB;
    private Material myMat;
    private Color startColor;
    private Vector3 tempForce;

    public puppyChat pupLLM;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myMat = GetComponent<MeshRenderer>().material;
        startColor = myMat.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pupLLM.paused)
        {


            if (transform.position.y > 2)
            {
                Vector3 temp = transform.position;
                temp.y = 2;
                transform.position = temp;
                myRB.AddForce((Vector3.down));
            }
        }
    }

    private void OnMouseDown()
    {
        if (!pupLLM.paused)
        {
            tempForce.x = Random.Range(-1, 1);
            tempForce.z = Random.Range(-1, 1);
            myRB.AddForce((Vector3.up + tempForce) * 250);
        }
    }

    private void OnMouseOver()
    {
        if (!pupLLM.paused)
        {
            myMat.color = startColor * 2;
        }
    }

    private void OnMouseExit()
    {
        myMat.color = startColor;
    }
}
