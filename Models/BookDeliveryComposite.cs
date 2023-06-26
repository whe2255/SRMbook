
namespace SrmBook.Models;
//발주와 배송의 복합 뷰 모델
public class BookDeliveryComposite

{
    public IEnumerable<BookOrder> BookOrder { get; set; }
    public IEnumerable<BookDelivery> BookDelivery { get; set; }
}