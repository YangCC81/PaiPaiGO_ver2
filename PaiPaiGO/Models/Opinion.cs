using System;
using System.Collections.Generic;

namespace PaiPaiGO.Models;

public partial class Opinion
{
    public string Ratingnumber { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Date { get; set; } = null!;

    public int MissionId { get; set; }

    public string ReportMemberId { get; set; } = null!;

    public string ReportedMemberId { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string State { get; set; } = null!;

    public int? Score { get; set; }

    public virtual Mission Mission { get; set; } = null!;

    public virtual Member ReportMember { get; set; } = null!;

    public virtual Member ReportedMember { get; set; } = null!;
}
