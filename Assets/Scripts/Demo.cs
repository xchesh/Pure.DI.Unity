using System;
using Pure.DI;
using UnityEngine;

public interface IBox<out T>
{
    T Content { get; }
}

public interface ICat
{
    State State { get; }
}

public interface IService
{
}

public interface IServiceNext
{
}


public enum State
{
    Alive,
    Dead
}

public class CardboardBox<T> : IBox<T>
{
    public T Content { get; }

    public CardboardBox(T content)
    {
        Content = content;
    }

    public override string ToString() => $"[{Content}]";
}

public class ShroedingersCat : ICat
{
    private readonly Lazy<State> _superposition;

    // The decoherence of the superposition
    // at the time of observation via an irreversible process
    public ShroedingersCat(Lazy<State> superposition)
    {
        _superposition = superposition;
    }

    public State State => _superposition.Value;

    public override string ToString() => $"{State} cat";
}

public class Service : IService
{
    public Service()
    {
    }
}

public class ServiceNext : IServiceNext
{
    public ServiceNext(IService service)
    {
    }
}

internal partial class Composition
{
    private void Setup()
    {
        var e = DI.Setup()
            .Bind<ICat>().As(Lifetime.Singleton).To<ShroedingersCat>()
            // Represents a cardboard box with any content
            .Bind<IBox<TT>>().As(Lifetime.Singleton).To<CardboardBox<TT>>()
            .Bind<IService>().As(Lifetime.Singleton).To<Service>()
            .Bind<IServiceNext>().As(Lifetime.Singleton).To<ServiceNext>()
            .Bind<DemoMonoBehaviourRuntime>().As(Lifetime.Singleton).To((ctx) =>
            {
                var gameObject = new UnityEngine.GameObject("DemoMonoBehaviourRuntime");
                var demoMonoBehaviour = gameObject.AddComponent<DemoMonoBehaviourRuntime>();

                // var service = ??? // TODO: How to resolve service?
                //demoMonoBehaviour.Inject(service);

                return demoMonoBehaviour;
            })
            // Composition Root
            .Root<Demo>("Context");
    }
}

public class Demo
{
    private static Composition _composition;

    private readonly IBox<ICat> _box;
    private readonly IServiceNext _serviceNext;
    private readonly DemoMonoBehaviourRuntime _demoInstanceMonoBehaviour;

    public Demo(IBox<ICat> box, IServiceNext serviceNext, DemoMonoBehaviourRuntime demoInstanceMonoBehaviour)
    {
        _box = box;
        _serviceNext = serviceNext;
        _demoInstanceMonoBehaviour = demoInstanceMonoBehaviour;
    }

    public static void Initialize()
    {
        _composition = new Composition();
        _composition.Context.Run();
    }

    private void Run()
    {
        Debug.Log(_box);
        Debug.Log(_serviceNext);
        Debug.Log(_demoInstanceMonoBehaviour);
    }
}
