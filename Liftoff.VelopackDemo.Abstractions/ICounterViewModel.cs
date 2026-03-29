namespace Liftoff.VelopackDemo.Abstractions;

public interface ICounterViewModel
{
    int CurrentCount { get; set; }

    void IncrementCount();
}