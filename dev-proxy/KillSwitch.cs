using System.ComponentModel;

namespace Microsoft.DevProxy;

public class KillSwitch(string path, CancellationToken? token) : BackgroundWorker
{
    private bool _cancelRequested = false;
    public async void Check()
    {
        token?.Register(OnCancellation);
        while (!_cancelRequested)
        {
            if (File.Exists(path))
            {
                KillSwitchEngaged?.Invoke(this, EventArgs.Empty);
            }
            await Task.Delay(2000);
        }
    }
    private void OnCancellation()
    {
        _cancelRequested = true;
    }
    public event EventHandler? KillSwitchEngaged;
}