using System.ComponentModel.DataAnnotations;

namespace SrmBook.Models;

public class LoginView
{
    [Required(ErrorMessage = "사용자 아이디를 확인하세요")]
    public string USER_ID { get; set; }

    [Required(ErrorMessage = "사용자 비밀번호를 확인하세요")]
    public string USER_PW { get; set; }
}