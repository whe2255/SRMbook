using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SrmBook.Models;

public class BookClassification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BOOK_NUM { get; set; }

    [Display(Name = "분류명")]
    public string BOOK_CLASS { get; set; }

    [Display(Name = "도서명")]
    public string BOOK_NAME { get; set; }

    [Display(Name = "저자")]
    public string BOOK_WRITER { get; set; }

    [Display(Name = "가격")]
    public int BOOK_PRICE { get; set; }

    [Display(Name = "표지")]
    public byte[] BOOK_IMAGE { get; set; }


}