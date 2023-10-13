using System;
using System.Collections.Generic;

namespace PaiPaiGO.Models;

public partial class Member
{
    public string MemberId { get; set; } = null!;

    public string MemberName { get; set; } = null!;

    public string MemberPhoneNumber { get; set; } = null!;

    public string MemberPostcode { get; set; } = null!;

    public string MemberCity { get; set; } = null!;

    public string MemberTownship { get; set; } = null!;

    public string MemberAddress { get; set; } = null!;

    public string MemberEmail { get; set; } = null!;

    public string MemberStatus { get; set; } = null!;

    public string MemberPassword { get; set; } = null!;

    public string? Gearing { get; set; }

    public virtual ICollection<Abandoned> Abandoneds { get; set; } = new List<Abandoned>();

    //public virtual County MemberPostcodeNavigation { get; set; } = null!;
    public virtual County? MemberPostcodeNavigation { get; set; }

    public virtual ICollection<Opinion> OpinionReportMembers { get; set; } = new List<Opinion>();

    public virtual ICollection<Opinion> OpinionReportedMembers { get; set; } = new List<Opinion>();
}
