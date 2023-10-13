using System;
using System.Collections.Generic;

namespace PaiPaiGO.Models;

public partial class Abandoned
{
    public int MissionId { get; set; }

    public string MemberId { get; set; } = null!;

    public string Date { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

    public virtual Mission Mission { get; set; } = null!;
}
