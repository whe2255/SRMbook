using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SrmBook.Models;

public class BookOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "발주 번호")]
    public int ORDER_NUM { get; set; }
    
    [Display(Name = "ID")]
    public string USER_ID { get; set; }

    [Display(Name = "발주 날짜")]
    [Required(ErrorMessage = "날짜를 입력하세요")]
    public DateTime ORDER_DATE { get; set; }

    [Display(Name = "발주량")]
    [Required(ErrorMessage = "구매 수량을 입력하세요")]
    public int BOOK_QUANTITY { get; set; }

    [Display(Name = "발주 총 가격")]
    public Decimal TOTAL_PRICE { get; set; }

    [Display(Name = "도서 제목")]
    [Required(ErrorMessage = "도서제목을 입력해주세요")]
    public string BOOK_NAME { get; set; }

    [Display(Name = "도서 번호")]
    [Required(ErrorMessage = "도서번호를 입력해주세요")]
    public int BOOK_NUM { get; set; }
}