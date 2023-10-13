using System;
using System.Collections.Generic;

namespace PaiPaiGO.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Caption> Captions { get; set; } = new List<Caption>();

    public virtual ICollection<Mission> Missions { get; set; } = new List<Mission>();
}
