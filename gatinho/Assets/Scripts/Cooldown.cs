using UnityEngine;

public class Cooldown
{

    private float time;
    private float timer;

    private bool finished = true;

    /// <summary>
    /// Constructor of the class with param
    /// _time: is the cooldown time
    /// </summary>
    public Cooldown(float _time)
    {
        time = _time;
        timer = 0;
    }

    /// <summary>
    /// Set the cooldown to a new time
    /// _time: is the new time of the cooldown
    /// </summary>
    public void SetTime(float _time)
    {
        time = _time;
    }

    /// <summary>
    /// Return if the cooldown is finished
    /// </summary>
    public bool IsFinished()
    {
        if (Time.time - timer >= 0)
            return true;
        return false;
    }

    /// <summary>
    /// Reset the cooldown
    /// </summary>
    public void Start()
    {
        timer = time + Time.time;
    }

    /// <summary>
    /// add time to the cooldown
    /// exemple: if something make the cooldown bigger
    /// </summary>
    public void AddTime(float _time)
    {
        timer += _time;
    }

    /// <summary>
    /// Return the percent of the cooldown
    /// </summary>
    public float Percent()
    {
        return 100 - (((timer - Time.time)/time)*100);
    }

    /// <summary>
    /// Return how many time is left to finish the cooldown
    /// </summary>
    public float TimeLeft()
    {
        return (timer - Time.time);
    }
}
