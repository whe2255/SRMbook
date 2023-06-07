using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SrmBook.Models;

public class BookUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int USER_NUM { get; set; }

    [Required(ErrorMessage = "사용자 이름을 입력하세요."), StringLength(5)]
    [RegularExpression("^[^0-9]+$", ErrorMessage = "숫자는 사용할 수 없습니다.")]
    public string USER_NAME { get; set; }

    [Required(ErrorMessage = "사용자 아이디를 입력하세요."), StringLength(15)]
    public string USER_ID { get; set; }

    [Required(ErrorMessage = "사용자 비밀번호를 입력하세요.")] //정규표현식 사용
    [RegularExpression("^(?=.*[A-Za-z])(?=.*[0-9])(?=.*[!@#$%^&*(),.?':{}|<>])[A-Za-z0-9!@#$%^&*(),.?':{}|<>]+$"
    , ErrorMessage = "영어와 숫자, 특수문자를 사용해 주세요"), StringLength(15)]
    public string USER_PW { get; set; }

    [Required(ErrorMessage = "사용자 분류를 선택하세요.")]
    public string USER_TYPE { get; set; }

    [Required(ErrorMessage = "EMAIL을 입력하세요")]
    [EmailAddress(ErrorMessage = "올바른 EMAIL 형식이 아닙니다"), StringLength(25)]
    public string USER_EMAIL { get; set; }

    [Required(ErrorMessage = "출판사명을 입력하세요"), StringLength(15)]
    public string PUBLISHER { get; set; }
}