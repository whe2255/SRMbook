using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SrmBook.Models;
//발주와 재고의 복합 모델
public class BookComposite

{
    public IEnumerable<BookOrder> BookOrders { get; set; }
    public IEnumerable<BookInventory> BookInventory { get; set; }

}