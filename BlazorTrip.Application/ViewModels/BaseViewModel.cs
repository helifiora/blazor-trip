using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace BlazorTrip.Application.ViewModels;

public class BaseViewModel : ObservableObject, IDisposable
{
    protected readonly IMessenger Messenger;

    protected BaseViewModel(IMessenger messenger)
    {
        Messenger = messenger;
        Messenger.RegisterAll(this);
    }

    public void Dispose()
    {
        Messenger.UnregisterAll(this);
    }
}