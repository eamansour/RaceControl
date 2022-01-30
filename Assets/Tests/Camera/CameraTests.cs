using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;

[Category("CameraTests")]
public class CameraTests
{
    private CameraController _cameraController;
    private IInputController _inputController;

    [SetUp]
    public void SetUp()
    {
        _cameraController = new GameObject().AddComponent<CameraController>();
        _inputController = Substitute.For<IInputController>();

        _inputController.MousePosition.Returns(GetScreenCenter());
        _inputController.VerticalInput.Returns(0);
        _inputController.VerticalInput.Returns(0);
        _cameraController.Construct(_inputController);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_cameraController.gameObject);
    }

    // Helper method to retrieve the center of the screen
    private Vector3 GetScreenCenter()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;
        return new Vector3(x, y, 0);
    }

    private void SetMousePosition(int x, int y, int z)
    {
        _inputController.MousePosition.Returns(new Vector3(x, y, z));
    }

    [UnityTest]
    public IEnumerator CameraIncreasesXPosition_WithPositiveVerticalInput()
    {
        _inputController.VerticalInput.Returns(1f);

        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(_cameraController.transform.position.x > 0f);
    }

    [UnityTest]
    public IEnumerator CameraDecreasesXPosition_WithNegativeVerticalInput()
    {
        _inputController.VerticalInput.Returns(-1f);

        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(_cameraController.transform.position.x < 0f);
    }

    [UnityTest]
    public IEnumerator CameraIncreasesZPosition_WithNegativeHorizontalInput()
    {
        _inputController.HorizontalInput.Returns(-1f);

        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(_cameraController.transform.position.z > 0f);
    }

    [UnityTest]
    public IEnumerator CameraDecreasesZPosition_WithPositiveHorizontalInput()
    {
        _inputController.HorizontalInput.Returns(1f);

        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(_cameraController.transform.position.z < 0f);
    }

    [UnityTest]
    public IEnumerator CameraIncreasesXPosition_WithMouseAtTopOfScreen()
    {
        SetMousePosition(Screen.width / 2, Screen.height, 0);

        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(_cameraController.transform.position.x > 0f);
    }

    [UnityTest]
    public IEnumerator CameraDecreasesXPosition_WithMouseAtBottomOfScreen()
    {
        SetMousePosition(Screen.width / 2, 0, 0);

        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(_cameraController.transform.position.x < 0f);
    }

    [UnityTest]
    public IEnumerator CameraIncreasesZPosition_WithMouseAtLeftOfScreen()
    {
        SetMousePosition(0, Screen.height / 2, 0);

        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(_cameraController.transform.position.z > 0f);
    }

    [UnityTest]
    public IEnumerator CameraDecreasesZPosition_WithMouseAtRightOfScreen()
    {
        SetMousePosition(Screen.width, Screen.height / 2, 0);

        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(_cameraController.transform.position.z < 0f);
    }

    [UnityTest]
    public IEnumerator CameraStays_WhenMouseIsWithinScreenBounds()
    {
        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(_cameraController.transform.position == Vector3.zero);
    }
}
