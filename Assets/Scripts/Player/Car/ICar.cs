using UnityEngine;
using System.Collections;

public interface ICar
{
    float Acceleration { get; set; }
    float SteerDir { get; set; }
    float Braking { get; }
    float Fuel { get; set; }
    bool InPit { get; set; }

    void Construct(Transform _centreOfMass);

    /// <summary>
    /// Sets the car to accelerate for a given amount of time (in seconds).
    /// </summary>
    IEnumerator Accelerate(float time);

    /// <summary>
    /// Sets the car to brake for a given amount of time (in seconds).
    /// </summary>
    IEnumerator Brake(float time);

    /// <summary>
    /// Sets the car to turn in a direction between 1 and -1 for a given amount of time (in seconds)
    /// </summary>
    IEnumerator Turn(float dir, float time);

    /// <summary>
    /// Gets an integer representation of the car's fuel property.
    /// </summary>
    int GetFuelInt();

    /// <summary>
    /// Gets the car's speed in miles per hour (MPH).
    /// </summary>
    int GetSpeedInMPH();

    /// <summary>
    /// Resets the car's acceleration and steering controls.
    /// </summary>
    void ResetControl();

    /// <summary>
    /// Locks or unlocks the car's physics properties, controlling its ability to move.
    /// </summary>
    void SetCarLock(bool locked);
}
