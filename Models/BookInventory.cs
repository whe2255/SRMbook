using System.ComponentModel.DataAnnotations;

namespace SrmBook.Models;

public class BookInventory
{
    [Key]
    public int BOOK_NUM { get; set; }
    public string BOOK_CLASS { get; set; }
    public string BOOK_NAME { get; set; }
    public string BOOK_WRITER { get; set; }
    public string PUBLISHER { get; set; }
    public int BOOK_PRICE { get; set; }
    public int BOOK_QUANTITY { get; set; }
}