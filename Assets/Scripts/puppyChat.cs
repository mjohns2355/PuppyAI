using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using LLMUnity;

public class puppyChat : MonoBehaviour
{

    public LLMClient llm;
    public string mood = "happy";

    public Text chatbotText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            chatbotText.text = "Thinking...";
            Task chatTask = llm.Chat("the " + mood + " puppy says something about being " + mood, DebugText);
        }
    }

    void DebugText(string msg)
    {
        string[] thoughts = msg.Split('<');
        Debug.Log("puppy chatbot: " + msg);
        chatbotText.text = thoughts[0];
    }

    public void SwitchMood(string changedMood){
        mood = changedMood;
    }
}
