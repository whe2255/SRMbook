using System.ComponentModel.DataAnnotations;

namespace SrmBook.Models;

public class PartnerManagement
{
    [Key]
    public int PM_NUM { get; set; }
    [RegularExpression(@"^\+?[0-9]{1,3}-?[0-9]{1,4}-?[0-9]{1,4}$", ErrorMessage = "올바른 전화번호 형식을 입력해주세요.")]
    public string PM_CONTACT { get; set; }
    public string PUBLISHER { get; set; }
    public string PM_ADD { get; set; }
}