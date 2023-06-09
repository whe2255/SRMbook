using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SrmBook.Models;

public class BookDelivery
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "배송 번호")]
    public int DELIVERY_NUM { get; set; }

    [Display(Name = "발주 번호")]
    public int ORDER_NUM { get; set; }

    [Display(Name = "ID")]
    public string USER_ID { get; set; }

    [Display(Name = "배송 날짜")]
    [Required(ErrorMessage = "날짜를 입력하세요")]
    public DateTime DELIVERY_DATE { get; set; }

    [Display(Name = "발주량")]
    [Required(ErrorMessage = "구매 수량을 입력하세요")]
    public int BOOK_QUANTITY { get; set; }

    [Display(Name = "도서 번호")]
    [Required(ErrorMessage = "도서번호를 입력해주세요")]
    public int BOOK_NUM { get; set; }

    [Display(Name = "도서 제목")]
    [Required(ErrorMessage = "도서제목을 입력해주세요")]
    public string BOOK_NAME { get; set; }

    [Required(ErrorMessage = "주소를 입력하세요")]
    public string ADDRESS { get; set; }

    [Required(ErrorMessage = "연락처를 입력하세요")]
    [RegularExpression(@"^\d{3}-\d{3,4}-\d{4}$", ErrorMessage = "유효한 전화번호 형식이 아닙니다, -를 입력해주세요")]
    public string TEL { get; set; }

    [Display(Name = "배송 상태")]
    public DeliveryStatus STATUS { get; set; }

    public enum DeliveryStatus
    {
        [Display(Name = "배송중")]
        InTransit,
        [Display(Name = "배송 완료")]
        Delivered
    }
}