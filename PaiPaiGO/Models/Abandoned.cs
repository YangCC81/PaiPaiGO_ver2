using System;
using System.Collections.Generic;

namespace PaiPaiGO.Models;

public partial class Abandoned
{
    public int MissionId { get; set; }

    public string MemberId { get; set; } = null!;

	public DateTime Date { get; set; }
	public string? Chat { get; set; }

	public virtual Member Member { get; set; } = null!;

    public virtual Mission Mission { get; set; } = null!;
}
