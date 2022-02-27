using System.Collections;
using UnityEngine;

public class PitStop : MonoBehaviour
{
    private const float CooldownTime = 5f;
    private const float RefuelInterval = 0.5f;
    private const float RefuelAmount = 10f;

    public bool IsFree { get; set; } = true;

    private Car _car;
    private CarAI _carAI;

    [SerializeField]
    private Checkpoint _pitExit;
    private bool _isColliding = false;

    public void Construct(Checkpoint pitExit)
    {
        _pitExit = pitExit;
    }

    /// <summary>
    /// Updates the car to be serviced and performs repairs when the pit stop is triggered.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (_isColliding) return;
        _isColliding = true;

        GameObject collidedObject = other.gameObject;
        if (collidedObject.CompareTag("Player") || collidedObject.CompareTag("AI"))
        {
            IPlayerManager player = collidedObject.GetComponent<IPlayerManager>();
            if (player.CurrentControl != ControlMode.AI) return;

            IsFree = false;
            _car = (Car)player.PlayerCar;
            _carAI = _car.GetComponent<CarAI>();

            _car.InPit = true;
            StartCoroutine(ServiceCar());
            _carAI.SetTarget(_pitExit);
        }
    }

    /// <summary>
    /// Performs repairs on a car over time.
    /// </summary>
    private IEnumerator ServiceCar()
    {
        _car.transform.position = transform.position;
        while (_car.Fuel < 100f)
        {
            _car.SetCarLock(true);
            _car.Fuel = Mathf.Clamp(_car.Fuel + RefuelAmount, _car.Fuel, 100f);
            yield return new WaitForSeconds(RefuelInterval);
        }
        
        // Release the serviced car from the pits and wait before accepting another car
        _car.SetCarLock(false);
        _car.InPit = false;
        yield return new WaitForSeconds(CooldownTime);
        
        _isColliding = false;
        IsFree = true;
    }
}
