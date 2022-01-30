using System.Collections;
using UnityEngine;

public class TestHelper : MonoBehaviour
{
    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
