using System.ComponentModel.DataAnnotations;

namespace SrmBook.Models
{ 	/* View를 위한 모델 작성 */
    public class BookLoginView
    {
        // 로그인 할때만 사용할 View  모델
        [Required(ErrorMessage ="사용자 ID를 입력해주세요")]
        public string USER_ID{get; set;}

        [Required(ErrorMessage = "사용자 비밀번호를 입력해주세요")]
        public string USER_PW { get; set; }
    }
}