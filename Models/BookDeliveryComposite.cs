
namespace SrmBook.Models;
//발주와 배송의 복합 뷰 모델
public class BookDeliveryComposite

{
    public IEnumerable<BookOrder> BookOrder { get; set; }
    public IEnumerable<BookDelivery> BookDelivery { get; set; }
    public PageInfo OrderPageInfo { get; set; }
    public PageInfo DeliveryPageInfo { get; set; }

    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}