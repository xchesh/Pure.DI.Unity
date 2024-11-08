using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class DemoMonoBehaviour : MonoBehaviour
{
    private IService _service;

    private void Awake()
    {
        Demo.Initialize();
    }
}
