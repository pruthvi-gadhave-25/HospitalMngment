namespace HospitalManagement.Helpers
{
    public class PagedResult
    {
    
        public async Task PagedResultService(int pageIndex, int pageSize)
        {
            int skip  =  (pageSize - 1) * pageIndex;
            int take =  (pageSize - 1) * pageIndex;
        }


    }
}
