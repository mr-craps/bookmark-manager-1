using Entity;
using System.Collections.Generic;

namespace Services
{
    public interface IBookmarkService
    {
        Bookmark CreateBookmark(Bookmark bookmark);
        List<Bookmark> GetBookmarks(string userId);
        Bookmark GetBookmark(int Id);
        Bookmark GetBookmark(string ShortDescription, string UserId);
        void UpdateBookmark(Bookmark bookmark);
        void DeleteBookmark(Bookmark bookmark);
        void DeleteBookmark(int Id);
    }
}
