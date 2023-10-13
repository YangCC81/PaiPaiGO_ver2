using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace PaiPaiGO.Models;

public partial class Mission
{
    public int MissionId { get; set; }

    public int Category { get; set; }

    public string Tags { get; set; } = null!;

    public string OrderMemberId { get; set; } = null!;

    public string? AcceptMemberId { get; set; }

    public string MissionName { get; set; } = null!;

    public decimal MissionAmount { get; set; }

    public string Postcode { get; set; } = null!;

    public string FormattedMissionAmount { get; set; } = null!;

    public string LocationCity { get; set; } = null!;

    public string LocationDistrict { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime DeliveryDate { get; set; }

    public TimeSpan DeliveryTime { get; set; }

    public DateTime DeadlineDate { get; set; }

    public TimeSpan DeadlineTime { get; set; }

    public string MissionDescription { get; set; } = null!;

    public string? DeliveryMethod { get; set; }

    public string? ExecutionLocation { get; set; }

    public string MissionStatus { get; set; } = null!;

    public DateTime? OrderTime { get; set; }

    public string? AcceptTime { get; set; }

    public byte[]? ImagePath { get; set; }

    public virtual Abandoned? Abandoned { get; set; }

    public virtual Category CategoryNavigation { get; set; } = null!;

    public virtual ICollection<Opinion> Opinions { get; set; } = new List<Opinion>();

}
//以下瑋珊的

public class MissionCategoyViewModel {
    public List<SelectListItem> MissionCategory { get; set; } = new List<SelectListItem> {
    };

}

public class MissionStatusViewModel {
    public List<SelectListItem> MissionStatus { get; set; } = new List<SelectListItem> {
    };
}

public class MissionStatusChangeModel {
    public int MissionId { get; set; }
    public string NewStatus { get; set; }
    public string InitialStatus { get; set; }
}
