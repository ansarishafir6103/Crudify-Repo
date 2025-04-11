using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crudify.Models;

public partial class TblEmployee
{
    [Key]
    public int EmployeeId { get; set; }
    [Display(Name = "First Name")]
    public string? EmployeeFirstName { get; set; }
    [Display(Name = "Last Name")]
    public string? EmployeeLastName { get; set; }
    [Display(Name = "Date Of Birth")]
    public DateTime EmployeeDateOfBirth { get; set; }
    [Display(Name = "Email")]
    public string EmployeeEmail { get; set; } = null!;
    [Display(Name = "Contact")]
    public string? EmployeePhone { get; set; }
    [Display(Name = "Image")]
    public string? EmployeePhoto { get; set; }
    [Display(Name = "Zip Code")]
    public string? EmployeePincode { get; set; }
    [Display(Name = "Gender")]
    public string? EmployeeGender { get; set; }
    [Display(Name = "Countries")]
    public int? CountryId { get; set; }
    [Display(Name = "States")]
    public int? StateId { get; set; }
    [Display(Name = "Cities")]
    public int? CityId { get; set; }
    [Display(Name = "Departments")]
    public int? DepartmentId { get; set; }

    public virtual TblCity? City { get; set; }

    public virtual TblCountry? Country { get; set; }

    public virtual TblDepartment? Department { get; set; }

    public virtual TblState? State { get; set; }
    public virtual ICollection<TblFavouriteHobby> TblFavouriteHobbies { get; set; } = new List<TblFavouriteHobby>();
}
