using PagedList;

namespace Crudify.Models
{
    public class EmployeeListViewModel
    {
        public string? SearchString { get; set; }
        public IPagedList<TblEmployee>? Employees { get; set; }
    }
}
