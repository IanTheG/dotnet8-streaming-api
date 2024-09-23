using MediatR;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;


namespace api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class StreamController(
    IMediator mediator,
    ILogger<StreamController> logger
) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<StreamController> _logger = logger;


    [HttpGet("migrate")]
    public async IAsyncEnumerable<string> Stream(
        [FromQuery] string client,
        [FromQuery] string environment,
        [FromQuery] string cityCode,
        [FromQuery] string database,
        [FromQuery] string[] hub,
        [FromQuery] string loglevel,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (!Enum.TryParse<LogLevel>(loglevel, true, out var parsedLogLevel))
        {
            // Handle invalid 'loglevel' value
            // e.g., return error response or set a default value
            parsedLogLevel = LogLevel.Debug; // Use a default log level, if necessary
        }

        var request = new StreamRequest
        {
            Client = client,
            Environment = environment,
            CityCode = cityCode,
            Database = database,
            Hubs = hub,
            Loglevel = parsedLogLevel,
        };
        var stream = await _mediator.Send(request, cancellationToken);

        _logger.LogDebug(request.ToJson());

        await foreach (var msg in stream.WithCancellation(cancellationToken))
        {
            _logger.LogTrace(msg.ToString());
            yield return msg;
        }
    }
}

public class StreamRequest : IRequest<IAsyncEnumerable<string>>
{
    public required string Client { get; set; }
    public required string Environment { get; set; }
    public required string CityCode { get; set; }
    public required string Database { get; set; }
    public required string[] Hubs { get; set; }
    public required LogLevel Loglevel { get; set; }
}

public class StreamingRequestHandler(
    ILogger<StreamingRequestHandler> logger
) : IRequestHandler<StreamRequest, IAsyncEnumerable<string>>
{
    private readonly ILogger<StreamingRequestHandler> _logger = logger;

    public Task<IAsyncEnumerable<string>> Handle(StreamRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Streamer(request, cancellationToken));
    }

    private async IAsyncEnumerable<string> Streamer(StreamRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        int count = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            yield return $"Message {count += 1}";

            await Task.Delay(1000, cancellationToken);
        }
    }
}
