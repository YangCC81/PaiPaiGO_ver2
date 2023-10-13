using System;
using System.Collections.Generic;

namespace PaiPaiGO.Models;

public partial class County
{
    public string? City { get; set; }

    public string Area { get; set; } = null!;

    public string Postcode { get; set; } = null!;

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
