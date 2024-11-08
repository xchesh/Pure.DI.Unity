using UnityEngine;

public class DemoMonoBehaviourRuntime : MonoBehaviour
{
    private IService _service;

    public void Inject(IService service)
    {
        _service = service;
    }
}
