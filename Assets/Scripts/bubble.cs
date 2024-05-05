using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubble : MonoBehaviour
{
    Vector3 pos;
    public Transform pup;
    private RectTransform myRect;

    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        pos = Camera.main.WorldToScreenPoint(pup.position);
        pos.y += 80;
        myRect.position = pos;
    }
}
