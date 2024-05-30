using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using LLMUnity;
using TMPro;

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

    public bool paused = false;
    public GameObject surveyScreen;

    private string[] moods = new string[6];
    private string[] moodSelects = new string[6];

    private int moodCounter = 0;

    public Slider moodSelect;

    public TextMeshProUGUI[] guesses;
    public TextMeshProUGUI[] corrects;

    public GameObject summaryScreen;

    private int accurate;
    private int correctCount;
    public TextMeshProUGUI accuracy;

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

    public void UnPause()
    {
        switch (moodSelect.value)
        {
            case 0:
                moodSelects[moodCounter] = "broken hearted";
                break;
            case 1:
                moodSelects[moodCounter] = "crying";
                break;
            case 2:
                moodSelects[moodCounter] = "sad";
                break;
            case 3:
                moodSelects[moodCounter] = "fine";
                break;
            case 4:
                moodSelects[moodCounter] = "happy";
                break;
            case 5:
                moodSelects[moodCounter] = "joyous";
                break;
            case 6:
                moodSelects[moodCounter] = "delighted";
                break;
            default:
                break;
        }
        moodCounter++;
        if (moodCounter >= 6)
        {
            for(int i = 0; i < 6; i++)
            {
                guesses[i].text = moodSelects[i];
                corrects[i].text = moods[i];
                if(moodSelects[i] == moods[i])
                {
                    correctCount++;
                }
            }
            accurate = (int)(100*(correctCount / 6f));
            accuracy.text = "Accuracy: " + accurate + "%";
            summaryScreen.SetActive(true);
        } else
        {

            paused = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (chatbotText.text == "thinking..." && thoughtBubble.gameObject.activeInHierarchy)
            {

                thoughtBubble.gameObject.SetActive(false);
                Debug.Log("done");
                paused = true;
                
                surveyScreen.SetActive(true);
            }
            else if (timer > 4)
            {
                thoughtBubble.gameObject.SetActive(true);
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer > 4 && chatbotText.text != "thinking...")
                {
                    thoughtBubble.gameObject.SetActive(true);
                }
            }
            else if (isReady && !paused)
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

                Task chatTask = llm.Chat("you are a " + mood + " puppy thinking about being " + mood + " and looking for a " + goal + " that you want. " + haiku + " end with a heart icon", DebugText);
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
                else if (temp < 4)
                {
                    haiku = " you are a good pup. ";
                }
                else
                {
                    haiku = " at least 4 words. ";
                }
            }
            if (chatbotText.text == "thinking..." && thoughtFill.value < 1)
            {
                thoughtFill.value += Time.deltaTime * (1 - thoughtFill.value) / 2f;
            }
        }

    }

    void DebugText(string msg)
    {
        thoughtFill.gameObject.SetActive(false);
        if (msg.Contains('('))
        {
            if (timer < 0 && !isReady)
            {
                moods[moodCounter] = mood;
                timer = 7;
                isReady = true;
            }
            string[] thoughts = msg.Split('(');
            chatbotText.text = thoughts[0];
        } else if (msg.Contains('<'))
        {
            if (timer < 0 && !isReady)
            {
                moods[moodCounter] = mood;
                timer = 7;
                isReady = true;
            }
            string[] thoughts = msg.Split('<');
            chatbotText.text = thoughts[0];
        }
        else if (msg.Contains("Or"))
        {
            if (timer < 0 && !isReady)
            {
                moods[moodCounter] = mood;
                timer = 7;
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
                moods[moodCounter] = mood;
                timer = 7;
                isReady = true;
            }
        }
    }

    public void SwitchMood(string changedMood){
        mood = changedMood;
        isReady = true;
    }
}
