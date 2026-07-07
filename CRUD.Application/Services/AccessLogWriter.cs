using CRUD.Application.Interfaces;
using CRUD.Application.Optionss;
using CRUD.Domain.Models;
using Microsoft.Extensions.Options;

namespace CRUD.Application.Services
{
    public class AccessLogWriter : IAcessLogWriter
    {
        private readonly IAcessLogRepository _repository;
        private readonly AccessLogOptions _options;

        public AccessLogWriter(IAcessLogRepository repository, IOptions<AccessLogOptions> options)
        {
            _repository = repository;
            _options = options.Value;
        }

        public async Task Log(AcessLog log, CancellationToken ct = default)
        {
            if (!_options.Enabled)
            {
                return;
            }

            if (_options.ExcludedPaths.Any(path =>
                    log.ApiPath.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            await _repository.AddAsync(log, ct);
        }
    }
}
