using UnityEngine;
using System.Collections;

public interface ICar
{
    float Acceleration { get; set; }
    float SteerDir { get; set; }
    float Braking { get; }
    float Fuel { get; set; }
    bool InPit { get; set; }
    
    IEnumerator Accelerate(float time);
    IEnumerator Brake(float time);
    void Construct(Transform _centreOfMass);
    int GetFuelInt();
    int GetSpeedInMPH();
    void ResetControl();
    void SetCarLock(bool locked);
    IEnumerator Turn(float dir, float time);
}
