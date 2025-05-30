using System;
using System.Threading;
using System.Threading.Tasks;

public static class AsyncExtensions
{
    public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<bool>();
        using (ct.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
        {
            if (task != await Task.WhenAny(task, tcs.Task))
                throw new OperationCanceledException(ct);
        }
        return await task;
    }
}