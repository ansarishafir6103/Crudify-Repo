using System;
using System.Collections.Generic;

namespace Crudify.Models;

public partial class TblHobby
{
    public int HobbyId { get; set; }

    public string? HobbyText { get; set; }

    public virtual ICollection<TblFavouriteHobby> TblFavouriteHobbies { get; set; } = new List<TblFavouriteHobby>();
}
