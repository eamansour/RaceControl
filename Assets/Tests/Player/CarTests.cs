using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[Category("PlayerTests")]
public class CarTests
{
    private Car _car;
    private TestHelper _testHelper;
    private GameObject _plane;
    private GameObject _playerObject;
    private Rigidbody _carRigidbody;

    [SetUp]
    public void SetUp()
    { 
        _plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        _playerObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Test/TestCar"));
        _carRigidbody = _playerObject.GetComponent<Rigidbody>();
        _car = _playerObject.GetComponent<Car>();
        _testHelper = _playerObject.AddComponent<TestHelper>();
    }

    [TearDown]
    public void TearDown()
    {
        Time.timeScale = 1f;
        GameObject.Destroy(_plane);
        GameObject.Destroy(_playerObject);
    }

    [Test]
    public void ResetControl_ShouldResetAccelerationAndSteering()
    {
        _car.Acceleration = 10f;
        _car.SteerDir = 1f;

        _car.ResetControl();

        Assert.AreEqual(0f, _car.Acceleration);
        Assert.AreEqual(0f, _car.SteerDir);
    }

    [Test]
    public void GetFuelInt_ShouldConvertFuelToNearestInteger()
    {
        _car.Fuel = 10.6f;
        int fuel = _car.GetFuelInt();
        
        Assert.AreEqual(11, fuel);
    }

    [UnityTest]
    public IEnumerator SetCarLockTrue_ShouldSetVelocityToZero()
    {
        _carRigidbody.velocity = new Vector3(10, 10, 10);
        
        _car.SetCarLock(true);
        yield return null;
        
        Assert.AreEqual(Vector3.zero, _carRigidbody.velocity);
    }

    [UnityTest]
    public IEnumerator Accelerate_ShouldIncreaseZVelocity()
    {
        _car.SetCarLock(false);
        _carRigidbody.velocity = Vector3.zero;

        _testHelper.RunCoroutine(_car.Accelerate(0.1f));
        yield return new WaitForSeconds(0.1f);
        Vector3 newVelocity = _carRigidbody.velocity;

        Assert.IsTrue(_carRigidbody.velocity.z > 0f);
    }

    [UnityTest]
    public IEnumerator Brake_ShouldReduceZVelocity()
    {
        _car.SetCarLock(false);
        _carRigidbody.velocity = new Vector3(0, 0, 10);

        _testHelper.RunCoroutine(_car.Brake(0.1f));
        yield return new WaitForSeconds(0.1f);
        Vector3 newVelocity = _carRigidbody.velocity;

        Assert.IsTrue(newVelocity.z < 10);
    }

    [UnityTest]
    public IEnumerator Turn_ShouldOnlyAffectSteerDirection()
    {
        _car.SetCarLock(false);
        _carRigidbody.velocity = Vector3.zero;

        _testHelper.RunCoroutine(_car.Turn(1, 0.5f));
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(1, _car.SteerDir);
        Assert.AreEqual(0, _car.Acceleration);
        Assert.AreEqual(0, _car.Braking);
    }

    [UnityTest]
    public IEnumerator GetSpeedInMPH_ShouldConvertMagnitudeToMPH()
    {
        _car.SetCarLock(false);
        _carRigidbody.velocity = new Vector3(0, 0, 10);
        int speed = _car.GetSpeedInMPH();
        yield return null;
        
        Assert.AreEqual(22, speed);
    }
}
