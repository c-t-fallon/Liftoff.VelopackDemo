using CommunityToolkit.Mvvm.ComponentModel;
using Liftoff.VelopackDemo.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liftoff.VelopackDemo.Pages;

public partial class CounterViewModel : ObservableObject, ICounterViewModel
{
    [ObservableProperty]
    private int currentCount = 0;

    public void IncrementCount()
    {
        CurrentCount++;
    }
}
