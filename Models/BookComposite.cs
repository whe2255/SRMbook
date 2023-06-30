namespace SrmBook.Models
{
    public class BookComposite
    {
        public IEnumerable<BookOrder> BookOrders { get; set; }
        public IEnumerable<BookInventory> BookInventory { get; set; }
        public PageInfo OrderPageInfo { get; set; }
        public PageInfo InventoryPageInfo { get; set; }
    }

    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}
