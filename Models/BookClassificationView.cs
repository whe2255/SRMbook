using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SrmBook.Models;

public class BookClassificationView
{
    public List<BookClassification> BookClassification { get; set; }
    public SelectList Genre { get; set; }
    public string BookGenre { get; set; }
    public string SearchString { get; set; }
}