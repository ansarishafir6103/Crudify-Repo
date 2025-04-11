using System;
using System.Collections.Generic;

namespace Crudify.Models;

public partial class TblFavouriteHobby
{
    public int FavouriteHobbiesId { get; set; }

    public int? HobbyId { get; set; }

    public int? EmployeeId { get; set; }

    public virtual TblEmployee? Employee { get; set; }

    public virtual TblHobby? Hobby { get; set; }
}
