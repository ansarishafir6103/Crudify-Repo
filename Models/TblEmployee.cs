using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crudify.Models;

public partial class TblEmployee
{
    [Key]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
    [Display(Name = "First Name")]
    public string? EmployeeFirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
    [Display(Name = "Last Name")]
    public string? EmployeeLastName { get; set; }

    [Required(ErrorMessage = "Date of Birth is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Date Of Birth")]
    public DateTime EmployeeDateOfBirth { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [Display(Name = "Email")]
    public string EmployeeEmail { get; set; } = null!;

    [Phone(ErrorMessage = "Invalid phone number")]
    [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 digits")]
    [Display(Name = "Contact")]
    public string? EmployeePhone { get; set; }

    [Display(Name = "Image")]
    public string? EmployeePhoto { get; set; }

    [StringLength(10, ErrorMessage = "Zip Code cannot exceed 10 characters")]
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
