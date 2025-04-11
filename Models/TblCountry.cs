using System;
using System.Collections.Generic;

namespace Crudify.Models;

public partial class TblCountry
{
    public int CountryId { get; set; }

    public string CountryShortname { get; set; } = null!;

    public string CountryName { get; set; } = null!;

    public int CountryPhonecode { get; set; }

    public virtual ICollection<TblEmployee> TblEmployees { get; set; } = new List<TblEmployee>();

    public virtual ICollection<TblState> TblStates { get; set; } = new List<TblState>();
}
