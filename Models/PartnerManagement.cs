using System.ComponentModel.DataAnnotations;

namespace SrmBook.Models;

public class PartnerManagement
{
    [Key]
    public int PM_NUM { get; set; }
    public string PM_CONTACT { get; set; }
    public string PUBLISHER { get; set; }
    public string PM_ADD { get; set; }
}