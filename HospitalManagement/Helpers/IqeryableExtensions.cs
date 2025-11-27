using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Helpers
{
    public static class IqeryableExtensions
    {
        public  static async Task<PagedResult<T>>   ToPagedResult<T>(IQueryable<T> query ,int page, int pageSize)
        {
            var totalCount =  await query.CountAsync();

            var items = await  query.Skip((page-1)* pageSize).Take(page).ToListAsync();

            return new PagedResult<T>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

    }
}
