using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ObstacleTests
{
    private ObstacleSpawn _spawner;

    [SetUp]
    public void SetUp()
    {
        _spawner = new GameObject().AddComponent<ObstacleSpawn>();
        Rigidbody testObstacle = _spawner.gameObject.AddComponent<Rigidbody>();
        _spawner.Construct(testObstacle);
    }

    [TearDown]
    public void TearDown()
    {
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
        {
            go.SetActive(true);
            Object.Destroy(go);
        }
    }

    [Test]
    public void SpawnObstacle_CreatesNewObstacle()
    {
        _spawner.SpawnObstacle();

        Assert.AreEqual(2, Object.FindObjectsOfType<Rigidbody>().Length);
    }

    [UnityTest]
    public IEnumerator SpawnObstacle_NewObstacleHasSpawnVelocity()
    {
        _spawner.SpawnObstacle();
        yield return new WaitForSeconds(0.05f);

        Object.Destroy(_spawner.gameObject);
        Rigidbody obstacle = Object.FindObjectOfType<Rigidbody>();

        Assert.IsTrue(obstacle.velocity.magnitude > 0);
    }
}
