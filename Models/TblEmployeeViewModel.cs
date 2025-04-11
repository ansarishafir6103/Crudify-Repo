using Crudify.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace Crudify.Models
{
    public class TblEmployeeViewModel
    {
        public TblEmployee? Employee { get; set; }
        public List<int> SelectedHobbyIds { get; set; } = new List<int>();
        public List<TblHobby>? Hobbies { get; set; }
        public List<TblCountry>? CountriesList { get; set; }
        public List<TblState>? StatesList { get; set; }
        public List<TblCity>? CitiesList { get; set; }
        public List<TblDepartment>? DepartmentsList { get; set; }

    }
}

