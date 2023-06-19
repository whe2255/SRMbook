using Microsoft.AspNetCore.Mvc.Rendering;

namespace SrmBook.Models;
//발주와 배송의 복합 뷰 모델
public class BookDeliveryComposite

{
    public List<BookOrder> BookOrder { get; set; }
    public List<BookDelivery> BookDelivery { get; set; }
}