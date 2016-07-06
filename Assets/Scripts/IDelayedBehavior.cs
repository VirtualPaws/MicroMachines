using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public interface IDelayedBehavior
{
    void setTimer(float time);
    void update();
    bool isFinished();
}

public class DelayedBlurReset : IDelayedBehavior
{
    private float startTime = Time.time;
    private float timer = 2; //seconds
    private float blurValue = 0.3f;

    public void setTimer(float time)
    {
        startTime = Time.time;
        timer = time;
    }

    public void update()
    {
        if (Time.time - startTime > timer)
        {
            Camera.main.GetComponent<MotionBlur>().blurAmount = blurValue;
        }
    }

    public bool isFinished()
    {
        return Time.time - startTime > timer;
    }

    public void setBlurValue(float value)
    {
        blurValue = value;
    }
}

public class DelayedControlReset : IDelayedBehavior
{
    private float startTime = Time.time;
    private float timer = 2; //seconds
    private Driving driving;

    public void setTimer(float time)
    {
        startTime = Time.time;
        timer = time;
    }

    public void update()
    {
        if (Time.time - startTime > timer)
        {
            driving.enabled = true;
        }
    }

    public bool isFinished()
    {
        return Time.time - startTime > timer;
    }

    public DelayedControlReset forScript(Driving dr)
    {
        driving = dr;
        return this;
    }

    public DelayedControlReset withTimer(float time)
    {
        setTimer(time);
        return this;
    }
}