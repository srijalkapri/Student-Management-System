using System;
using System.Linq;
using System.Threading.Tasks;
using CRUD.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Application.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> source,
        PaginationParameters parameters)
    {
        var totalCount = await source.CountAsync();

        var totalPages = (int)Math.Ceiling((double)totalCount / parameters.PageSize);

        var skip = (parameters.PageNumber - 1) * parameters.PageSize;
        var take = parameters.PageSize;

        var pagedData = await source
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return new PagedResult<T>
        {
            Items = pagedData,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}
