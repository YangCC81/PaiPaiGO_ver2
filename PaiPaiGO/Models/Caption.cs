using System;
using System.Collections.Generic;

namespace PaiPaiGO.Models;

public partial class Caption
{
    public int CaptionId { get; set; }

    public string TagName { get; set; } = null!;

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;
}
