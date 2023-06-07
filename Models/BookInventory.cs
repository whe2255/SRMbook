using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SrmBook.Models;

public class BookInventory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "도서 번호")]
    public int BOOK_NUM { get; set; }

    [Display(Name = "분류명")]
    public string BOOK_CLASS { get; set; }

    [Display(Name = "도서명")]
    public string BOOK_NAME { get; set; }

    [Display(Name = "저자")]
    public string BOOK_WRITER { get; set; }

    [Display(Name = "가격")]
    public int BOOK_PRICE { get; set; }

    [Display(Name = "재고 수량")]
    public int BOOK_QUANTITY { get; set; }
}