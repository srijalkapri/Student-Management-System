using CRUD.Domain.Models;

namespace CRUD.Application.Interfaces
{
    public interface IAcessLogWriter
    {

        Task Log(AcessLog log, CancellationToken ct = default);
    }

}
