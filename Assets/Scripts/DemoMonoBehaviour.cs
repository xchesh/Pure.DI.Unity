using System;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class DemoMonoBehaviour : MonoBehaviour
{
    private void Awake()
    {
        Demo.Initialize();
    }
}