using Microsoft.AspNetCore.Mvc.Rendering;

namespace SrmBook.Models;

// 장르별 검색용 뷰
public class BookSearchView
{
    public List<BookClassification> BookClassification { get; set; }
    public List<BookInventory> BookInventory { get; set; }
    public SelectList Genre { get; set; }
    public string BookGenre { get; set; }
    public string SearchString { get; set; }

    // 페이징 처리를 위한 속성
    public List<BookClassification> PagedBooks { get; set; } // 페이징 처리된 도서 목록
    public List<BookInventory> PagedBooks2 { get; set; } // 페이징 처리된 도서 목록
    public int CurrentPage { get; set; } // 현재 페이지 번호
    public int PageSize { get; set; } // 페이지당 도서 개수
    public int TotalBooks { get; set; } // 전체 도서 개수

    public int TotalPages
    {
        get { return (int)Math.Ceiling((double)TotalBooks / PageSize); }
    }

    public bool HasPreviousPage
    {
        get { return CurrentPage > 1; }
    }

    public bool HasNextPage
    {
        get { return CurrentPage < TotalPages; }
    }
}
