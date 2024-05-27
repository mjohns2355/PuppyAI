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
    public string goal = "ball";
    public string recentGoal = "";

    public Text chatbotText;
    public bool isReady = false;

    private float timer = 2;

    public GameObject[] puppies;
    private int currentPuppy = 0;

    public bubble thoughtBubble;
    Vector3 pos;
    RectTransform rt;
    public Slider thoughtFill;
    public Image thoughtColor;
    private string haiku = "";
    private float temp;

    private void Start()
    {
        thoughtBubble.gameObject.SetActive(false);
        rt = thoughtBubble.GetComponent<RectTransform>();
        Task chatTask = llm.Chat("you are a good puppy thinking about belly rubs and looking for treats and toys");

    }
    public void Ready()
    {
        isReady = true;
        timer = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (chatbotText.text == "thinking...")
        {

            thoughtBubble.gameObject.SetActive(false);

        } else if (timer > 4)
        {
            thoughtBubble.gameObject.SetActive(true);
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer < 3)
            {
                thoughtBubble.gameObject.SetActive(false);
            }
        }
        else if (isReady)
        {
            isReady = false;
            chatbotText.text = "thinking...";
            if (!puppies[currentPuppy].activeInHierarchy)
            {
                currentPuppy = 0;
            }
            thoughtBubble.pup = puppies[currentPuppy].transform;
            pos = Camera.main.WorldToScreenPoint(puppies[currentPuppy].transform.position);
            pos.y += 80;
            rt.position = pos;
            mood = puppies[currentPuppy].GetComponent<Puppy>().mood;
            goal = puppies[currentPuppy].GetComponent<Puppy>().goal;
            thoughtColor.color = puppies[currentPuppy].GetComponent<Puppy>().moodColor;
            thoughtFill.value = 0;
            thoughtFill.gameObject.SetActive(true);
            Debug.Log("mood: " + mood + " goal: " + goal);
            
            Task chatTask = llm.Chat("you are a " + mood + " puppy thinking about being " + mood + " and looking for a " + goal + " that you want. "+haiku+" end with a heart icon", DebugText);
            currentPuppy++;
            temp = Random.Range(0, 5);
            if (temp < 1)
            {
                haiku = " at least 6 words. ";
            }
            else if (temp < 3)
            {
                haiku = " you love your hooman. ";
            }
            else if(temp < 4)
            {
                haiku = " you are a good pup. ";
            } else
            {
                haiku = " at least 4 words. ";
            }
        } 
        if(chatbotText.text == "thinking..." && thoughtFill.value < 1)
        {
            thoughtFill.value += Time.deltaTime * (1 - thoughtFill.value) / 2f;
        }

    }

    void DebugText(string msg)
    {
        thoughtFill.gameObject.SetActive(false);
        if (msg.Contains('('))
        {
            if (timer < 0 && !isReady)
            {
                timer = 9;
                isReady = true;
            }
            string[] thoughts = msg.Split('(');
            chatbotText.text = thoughts[0];
        } else if (msg.Contains('<'))
        {
            if (timer < 0 && !isReady)
            {
                timer = 9;
                isReady = true;
            }
            string[] thoughts = msg.Split('<');
            chatbotText.text = thoughts[0];
        }
        else if (msg.Contains("Or"))
        {
            if (timer < 0 && !isReady)
            {
                timer = 9;
                isReady = true;
            }
            string[] thoughts = msg.Split('O');
            chatbotText.text = thoughts[0];
        }
        else
        {
            chatbotText.text = msg;
            if(msg.Length > 18 && timer <= 0 && !isReady)
            {
                timer = 9;
                isReady = true;
            }
        }
    }

    public void SwitchMood(string changedMood){
        mood = changedMood;
        isReady = true;
    }
}
