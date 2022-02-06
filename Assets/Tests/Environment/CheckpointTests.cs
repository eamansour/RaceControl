using NUnit.Framework;
using UnityEngine;

[Category("EnvironmentTests")]
public class CheckpointTests
{
    private Checkpoint _checkpoint;

    [SetUp]
    public void SetUp()
    {
        _checkpoint = new GameObject().AddComponent<Checkpoint>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_checkpoint.gameObject);
    }

    [Test]
    public void GetPosition_ReturnsCheckpointPosition(
        [Values(-1, 0, 1, 5)] int x,
        [Values(-1, 0, 1, 10)] int y,
        [Values(-1, 0, 1, 500)] int z
    )
    {
        Vector3 newPosition = new Vector3(x, y, z);

        _checkpoint.transform.position = newPosition;
        Assert.AreEqual(newPosition, _checkpoint.GetPosition());
    }

    [Test]
    public void GetRotation_ReturnsCheckpointRotation()
    {
        Vector3 rotation = new Vector3(5, 2, 0);
        _checkpoint.transform.localEulerAngles = rotation;
        Vector3 newRotation = _checkpoint.GetRotation();

        Assert.AreEqual(rotation.x, (int)newRotation.x);
        Assert.AreEqual(rotation.y, (int)newRotation.y);
        Assert.AreEqual(rotation.z, (int)newRotation.z);
    }
}
