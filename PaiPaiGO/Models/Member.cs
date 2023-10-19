using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PaiPaiGO.Models;

public partial class Member
{
    public string MemberId { get; set; } = null!;

    [Required(ErrorMessage = "請輸入姓名。")]
    public string MemberName { get; set; } = null!;

    [Required(ErrorMessage = "請輸入行動電話號碼。")]
    [RegularExpression(@"^09\d{8}$", ErrorMessage = "請輸入有效的行動電話號碼。")]
    public string MemberPhoneNumber { get; set; } = null!;

    public string MemberPostcode { get; set; } = null!;

    public string MemberCity { get; set; } = null!;

    public string MemberTownship { get; set; } = null!;

    [Required(ErrorMessage = "請輸入詳細地址。")]
    public string MemberAddress { get; set; } = null!;

    [Required(ErrorMessage = "請輸入Email地址。")]
    [RegularExpression(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "請輸入有效的Email地址。")]
    public string MemberEmail { get; set; } = null!;

    public string MemberStatus { get; set; } = null!;

    [Required(ErrorMessage = "請輸入密碼。")]
    //[RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]{8,20}$", ErrorMessage = "您的密碼長度必須為 8-20 個字符，包含字母和數字，且不得包含空格、特殊字符或表情符號。")]
    public string MemberPassword { get; set; } = null!;

    public string? Gearing { get; set; }

    public string? Salt { get; set; }

    public virtual ICollection<Abandoned> Abandoneds { get; set; } = new List<Abandoned>();

    //public virtual County MemberPostcodeNavigation { get; set; } = null!;
    public virtual County? MemberPostcodeNavigation { get; set; }

    public virtual ICollection<Opinion> OpinionReportMembers { get; set; } = new List<Opinion>();

    public virtual ICollection<Opinion> OpinionReportedMembers { get; set; } = new List<Opinion>();
}


//瑋珊的
public class MemberStatusChangeModel {
    public int MemberId { get; set; }
    public string NewStatus { get; set; }
    public string InitialStatus { get; set; }
}

public class MemberOpinion {
    public Member member { get; set; }

    public Opinion opinion { get; set; }
}

