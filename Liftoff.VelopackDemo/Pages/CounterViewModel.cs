using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liftoff.VelopackDemo.Pages;

public partial class CounterViewModel : ObservableObject
{
    [ObservableProperty]
    private int currentCount = 0;

    public void IncrementCount()
    {
        CurrentCount++;
    }
}
