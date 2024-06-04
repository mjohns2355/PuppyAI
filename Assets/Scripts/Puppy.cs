using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Puppy : MonoBehaviour
{
    public string mood = "happy";
    public string goal = "ball";
    public Transform[] goals;
    private NavMeshAgent myAgent;
    private int currentGoal = 0;
    private float timer = 6;
    private int happiness = 0;
    private Animator myAnim;
    public TextMeshProUGUI score;
    private Transform[] priorityGoals;
    private Transform priority;
    private TreatSpawner ts;
    private Transform prevPriority;
    public Color moodColor;
    public puppyChat pupLLM;

    // Start is called before the first frame update
    void Start()
    {
        moodColor = Color.yellow;
        myAgent = GetComponent<NavMeshAgent>();
        myAnim = GetComponent<Animator>();
        myAgent.speed = Random.Range(1.5f, 3f);
        myAnim.speed = myAgent.speed;
        score.gameObject.SetActive(true);
        ts = GameObject.FindGameObjectWithTag("Treats").GetComponent<TreatSpawner>();
        priorityGoals = ts.treats;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pupLLM.paused)
        {

            if (priority == null || !priority.gameObject.activeInHierarchy)
            {
                priority = goals[currentGoal];
            }
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                prevPriority = priority;
                priority = null;
                foreach (Transform t in priorityGoals)
                {
                    if (t.gameObject.activeInHierarchy)
                    {
                        if (t != prevPriority)
                            priority = t;
                    }
                }
                if (priority == null)
                {
                    priority = goals[currentGoal];
                }
                timer = Random.Range(4, 8);
                currentGoal = Random.Range(0, goals.Length);
                if (priority == null)
                {
                    priority = goals[currentGoal];
                }
                happiness--;
                UpdateMood();
                score.text = happiness.ToString();
                //  Debug.Log("Happiness down: " + happiness);
                myAgent.speed = Random.Range(1.5f, 3f);
                myAnim.speed = myAgent.speed;
            }
            if (Vector3.Distance(transform.position, priority.position) < 0.55f)
            {
                if (priority.tag == "treat")
                {
                    // ts.TreatGone(priority);
                    priority.gameObject.SetActive(false);
                    priority = goals[currentGoal];
                }
                happiness++;
                UpdateMood();
                score.text = happiness.ToString();
                //  Debug.Log("Happiness up: " + happiness);
                timer = Random.Range(2, 4);
                currentGoal = Random.Range(0, goals.Length);
                priority = goals[currentGoal];
                myAgent.speed = Random.Range(1.5f, 3f);
                myAnim.speed = myAgent.speed;
            }

            myAgent.destination = priority.position;
            goal = priority.tag;
        }
        else
        {
            myAnim.speed = 0;
        }
    }

    void UpdateMood()
    {
        if (happiness > 20)
        {
            mood = "delighted";
            moodColor = Color.blue;
        }
        else if (happiness > 15)
        {
            mood = "joyous";
            moodColor = Color.cyan;
        }
        else if (happiness > 8)
        {
            mood = "happy";
            moodColor = Color.green;
        }
        else if (happiness > 4)
        {
            mood = "fine";
            moodColor = Color.yellow;
        }
        else if (happiness > 1)
        {
            mood = "sad";
            moodColor = Color.red;
        } else if(happiness > -1)
        {
            mood = "wimpering";
            moodColor = Color.red;
        }
        else
        {
            mood = "broken hearted";
            moodColor = Color.red;
        }
    }

    public void AlertTreat(Transform t)
    {
        priority = t;
    }

    public void TreatGone(Transform treat)
    {
        if(priority == treat)
        {
            priority = null;
            foreach (Transform t in priorityGoals)
            {
                if (t.gameObject.activeInHierarchy)
                {
                    priority = t;
                }
            }
        }

    }
}
