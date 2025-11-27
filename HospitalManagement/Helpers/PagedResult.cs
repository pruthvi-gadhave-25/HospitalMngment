using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Helpers
{
    public   class PagedResult<T>
    {
    
        public IEnumerable<T>  Items { get; set; } =  new List<T>();
        public int PageSize { get; set; }

        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public static  async Task PagedResultService(int pageIndex, int pageSize)
        {
            int skip  =  (pageSize - 1) * pageIndex;
            int take =  (pageSize - 1) * pageIndex;
        }

       
        //public async Task<PagedResult<T>> GetDataAsync<T>(IQueryable<T>  query , int page =1,int pageSize = 10)
        //    where T : class
        //{
        //    var result = new PagedResult<T>
        //    {
        //        CurrentPage = page,
        //        PageSize = pageSize,
        //        TotalCount = await query.CountAsync()
        //    };

        //    result.Items = await query
        //        .Skip(page - 1)
        //        .Take(pageSize)
        //        .ToListAsync();
        //    return result;
        //}
      
    }
}
