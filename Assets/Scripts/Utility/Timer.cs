using UnityEngine;

/// <summary>
/// Reusable timer class for handling time-based logic
/// </summary>
public class Timer
{
    private float duration;
    private float currentTime;
    private bool isRunning;
    
    public float Duration => duration;
    public float CurrentTime => currentTime;
    public float RemainingTime => Mathf.Max(0f, duration - currentTime);
    public float Progress => duration > 0 ? Mathf.Clamp01(currentTime / duration) : 1f;
    public bool IsRunning => isRunning;
    public bool IsFinished => currentTime >= duration;
    
    /// <summary>
    /// Create a new timer with specified duration
    /// </summary>
    public Timer(float duration)
    {
        this.duration = duration;
        this.currentTime = 0f;
        this.isRunning = false;
    }
    
    /// <summary>
    /// Start or restart the timer
    /// </summary>
    public void Start()
    {
        currentTime = 0f;
        isRunning = true;
    }
    
    /// <summary>
    /// Start the timer with a new duration
    /// </summary>
    public void Start(float newDuration)
    {
        duration = newDuration;
        Start();
    }
    
    /// <summary>
    /// Pause the timer
    /// </summary>
    public void Pause()
    {
        isRunning = false;
    }
    
    /// <summary>
    /// Resume the timer
    /// </summary>
    public void Resume()
    {
        isRunning = true;
    }
    
    /// <summary>
    /// Stop and reset the timer
    /// </summary>
    public void Stop()
    {
        isRunning = false;
        currentTime = 0f;
    }
    
    /// <summary>
    /// Reset the timer without stopping it
    /// </summary>
    public void Reset()
    {
        currentTime = 0f;
    }
    
    /// <summary>
    /// Update the timer (call this in Update or FixedUpdate)
    /// </summary>
    public void Tick(float deltaTime)
    {
        if (isRunning)
        {
            currentTime += deltaTime;
        }
    }
    
    /// <summary>
    /// Update the timer with Time.deltaTime
    /// </summary>
    public void Tick()
    {
        Tick(Time.deltaTime);
    }
    
    /// <summary>
    /// Check if timer is finished and auto-reset if specified
    /// </summary>
    public bool CheckFinished(bool autoReset = false)
    {
        if (IsFinished)
        {
            if (autoReset)
            {
                Reset();
            }
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Set the duration of the timer
    /// </summary>
    public void SetDuration(float newDuration)
    {
        duration = newDuration;
    }
}

