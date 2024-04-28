using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreatSpawner : MonoBehaviour
{

    public GameObject[] puppies;
    public Transform[] treats;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartOver()
    {
        SceneManager.LoadScene(0);
    }

    private void OnMouseDown()
    {
        foreach(Transform t in treats)
        {
            t.gameObject.SetActive(true);
            t.GetComponent<treat>().Spawn();
        }
        foreach(GameObject p in puppies)
        {
            p.GetComponent<Puppy>().AlertTreat(treats[Random.Range(0, treats.Length)]);
        }
    }

    public void TreatGone(Transform t)
    {
        foreach(GameObject p in puppies)
        {
            p.GetComponent<Puppy>().TreatGone(t);
        }
    }
}
