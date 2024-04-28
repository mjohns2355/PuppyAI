using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using LLMUnity;

public class puppyChat : MonoBehaviour
{

    public LLMClient llm;
    public string mood = "happy";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Task chatTask = llm.Chat("the " + mood + " puppy says something about being " + mood, DebugText);
        }
    }

    void DebugText(string msg)
    {
        Debug.Log("puppy chatbot: " + msg);
    }
}
