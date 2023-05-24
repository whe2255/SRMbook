using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SrmBook.Models;

public class BookUser
{
    [Key]
    public int USER_NUM { get; set; }

    [Required(ErrorMessage = "사용자 이름을 입력하세요.")]
    public string USER_NAME { get; set; }

    [Required(ErrorMessage = "사용자 아이디를 입력하세요.")]
    public string USER_ID { get; set; }

    [Required(ErrorMessage = "사용자 비밀번호를 입력하세요.")]
    public string USER_PW { get; set; }

    [Required(ErrorMessage = "사용자 분류를 선택하세요.")]
    public string USER_TYPE { get; set; }

    [Required(ErrorMessage = "EMAIL을 입력하세요")]
    [EmailAddress]
    public string USER_EMAIL { get; set; }

    [Required(ErrorMessage = "출판사명을 입력하세요")]
    public string PUBLISHER { get; set; }
}