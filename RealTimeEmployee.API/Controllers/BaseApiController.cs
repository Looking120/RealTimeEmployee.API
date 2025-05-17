using Microsoft.AspNetCore.Mvc;
using RealTimeEmployee.DataAccess.Models;

namespace RealTimeEmployee.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected void AddPaginationHeaders<T>(PaginatedResult<T> paginatedResult) where T : class
    {
        Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(new
        {
            totalCount = paginatedResult.TotalCount,
            pageSize = paginatedResult.PageSize,
            currentPage = paginatedResult.PageNumber,
            totalPages = paginatedResult.TotalPages,
            hasNext = paginatedResult.HasNext,
            hasPrevious = paginatedResult.HasPrevious
        }));

        Response.Headers.Append("Access-Control-Expose-Headers", "X-Pagination");
    }
}