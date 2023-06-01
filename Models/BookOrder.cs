using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SrmBook.Models;

public class BookOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ORDER_NUM { get; set; }

    [ForeignKey("BookUser")]
    public string USER_ID { get; set; }

    [Required(ErrorMessage = "날짜를 입력하세요")]
    public DateTime ORDER_DATE { get; set; }

    [Required(ErrorMessage = "구매 수량을 입력하세요")]
    public int BOOK_QUANTITY { get; set; }

    public Decimal TOTAL_PRICE { get; set; }
    
    [Required(ErrorMessage = "도서제목을 입력해주세요")]
    public string BOOK_NAME { get; set; }

    [ForeignKey("BookInventory")]
    [Required(ErrorMessage = "도서번호를 입력해주세요")]
    public int BOOK_NUM { get; set; }
}