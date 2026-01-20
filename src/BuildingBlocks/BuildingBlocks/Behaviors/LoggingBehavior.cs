using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TReq, TRes>(ILogger<LoggingBehavior<TReq, TRes>> logger)
    : IPipelineBehavior<TReq, TRes> where TReq : IRequest<TRes> where TRes : notnull
{
    public async Task<TRes> Handle(TReq request, RequestHandlerDelegate<TRes> next, CancellationToken cancellationToken)
    {
        Log(LogLevel.Information, $"[START] Handle Req={typeof(TReq).Name} - Resp={typeof(TRes).Name} - Req={request}");

        var timer = new Stopwatch();

        timer.Start();

        var response = await next(cancellationToken);

        timer.Stop();

        var elapsed = timer.Elapsed;

        // if the request is greater than 3 seconds, then log the warnings
        if (elapsed.Seconds > 3)
        {
            Log(LogLevel.Warning, $"[PERFORMANCE] The request {typeof(TReq).Name} took {elapsed.Seconds} seconds.");
        }

        Log(LogLevel.Information, $"[END] Handled {typeof(TReq).Name} with {typeof(TRes).Name}");

        return response;
    }

    private void Log(LogLevel level, string message)
    {
        switch (level)
        {
            case LogLevel.Information:
                if (logger.IsEnabled(LogLevel.Information)) logger.LogInformation("{Message}", message);
                break;
            case LogLevel.Trace:
            case LogLevel.Debug:
            case LogLevel.Warning:
            case LogLevel.Error:
            case LogLevel.Critical:
            case LogLevel.None:
            default:
                throw new ArgumentOutOfRangeException(nameof(level), level, null);
        }
    }
}
