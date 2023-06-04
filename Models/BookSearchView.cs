using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SrmBook.Models;
//장르별 검색용 뷰
public class BookSearchView
{
    public List<BookClassification> BookClassification { get; set; }
    public List<BookInventory> BookInventory { get; set; }
    public SelectList Genre { get; set; }
    public string BookGenre { get; set; }
    public string SearchString { get; set; }

}