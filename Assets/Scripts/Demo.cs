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

// Let's glue all together

internal partial class Composition
{
    // In fact, this code is never run, and the method can have any name or be a constructor, for example,
    // and can be in any part of the compiled code because this is just a hint to set up an object graph.
    // Here the setup is part of the generated class, just as an example.
    private void Setup() => DI.Setup()
        .Bind<ICat>().To<ShroedingersCat>()
        // Represents a cardboard box with any content
        .Bind<IBox<TT>>().To<CardboardBox<TT>>()
        .Bind<IService>().To<Service>()
        // Composition Root
        .Root<Demo>("Context");
}

public class Demo
{
    private static Composition _composition;

    private readonly IBox<ICat> _box;
    private readonly IService _service;

    public Demo(IBox<ICat> box, IService service)
    {
        _box = box;
        _service = service;
    }

    // Composition Root, a single place in an application
    // where the composition of the object graphs for an application take place
    public static void Initialize()
    {
        _composition = new Composition();
        _composition.Context.Run();
    }

    private void Run()
    {
        Debug.Log(_box);

        try
        {
            var service = _composition.Resolve<IService>();

            Debug.Log(service);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}