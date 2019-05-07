public class Cooldown
{

    private float time;
    private float timer;

    private bool finished = true;


    public Cooldown()
    {
        time = 0;
        timer = 0;
    }

    public Cooldown(float _time)
    {
        time = _time;
        timer = 0;
    }

    public void Update(float deltatime)
    {
        if (!finished)
        {
            // else, decreses the timer
            timer -= deltatime;
            // if the time of the cooldown is over
            if (timer <= 0)
                finished = true;
                
        }
    }

    public void SetTime(float _time)
    {
        time = _time;
    }

    public bool IsFinish()
    {
        return finished;
    }

    public void Start()
    {
        finished = false;
        timer = time;
    }

    public void AddTime(float _time)
    {
        timer += _time;
    }
}
