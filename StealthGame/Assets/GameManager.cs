using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public float speed = 1.0f;

    public float gravity = 1.0f;
    public float terminalVelocity = 10.0f;

    public List<Timer> timers = new List<Timer>();

    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(gameObject);

        if(instance != null && instance != this)
        {
            Destroy(this);
        }
		
        else
        {
            instance = this;
        }
	}
	
	// Update is called once per frame
	void Update () {

        UpdateTimers();
	}

    void AddTimer(float duration, TimerEvent e)
    {
        Timer t = new Timer(duration, e);
        AddTimer(t);
    }

    void AddTimer(Timer t)
    {
        timers.Add(t);
    }

    void UpdateTimers()
    {
        foreach(Timer t in timers)
        {
            if(t != null)
                t.Update();

            
        }

        foreach (Timer t in timers)
        {
            if (t.done)
            {
                timers.Remove(t);
            }
        }
    }
}

public delegate void TimerEvent();

public class Timer
{
    public float elapsed;
    public float duration;
    public bool done = false;

    public TimerEvent myEvent;

    public Timer()
    {
        duration = 1.0f;
        myEvent = null;
    }

    public Timer(float d, TimerEvent e)
    {
        duration = d;
        myEvent = e;
    }

    public void Update()
    {
        elapsed += Time.deltaTime;

        if(elapsed > duration)
        {
            myEvent();
            done = true;
        }
    }
}
