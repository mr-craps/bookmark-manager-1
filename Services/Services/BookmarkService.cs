using Data;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Services
{
    public class BookmarkService : IBookmarkService
    {
        private ReadLaterDataContext _ReadLaterDataContext;

        public BookmarkService(ReadLaterDataContext readLaterDataContext)
        {
            _ReadLaterDataContext = readLaterDataContext;
        }

        public Bookmark CreateBookmark(Bookmark bookmark)
        {
            var existingBookmark =
                _ReadLaterDataContext.Bookmarks.Where(b =>
                                                    b.ShortDescription.ToLower() == bookmark.ShortDescription.ToLower() &&
                                                    b.UserId.ToLower() == bookmark.UserId.ToLower()
                                                ).FirstOrDefault();

            if (existingBookmark != null)
            {
                return existingBookmark;
            }
            else
            {
                _ReadLaterDataContext.Add(bookmark);
                _ReadLaterDataContext.SaveChanges();
                return bookmark;
            }            
        }

        public void UpdateBookmark(Bookmark bookmark)
        {
            _ReadLaterDataContext.Update(bookmark);
            _ReadLaterDataContext.SaveChanges();
        }

        public List<Bookmark> GetBookmarks(string userId)
        {
            return _ReadLaterDataContext.Bookmarks.Where(b => b.UserId == userId).ToList();
        }

        public Bookmark GetBookmark(int Id)
        {
            return _ReadLaterDataContext.Bookmarks.Where(b => b.ID == Id).FirstOrDefault();
        }

        public Bookmark GetBookmark(string ShortDescription, string UserId)
        {
            return _ReadLaterDataContext.Bookmarks.Where(b => 
                                                        b.ShortDescription.ToLower() == ShortDescription.ToLower() &&
                                                        b.UserId.ToLower() == UserId.ToLower()
                                                   ).FirstOrDefault();
        }

        public void DeleteBookmark(Bookmark bookmark)
        {
            _ReadLaterDataContext.Bookmarks.Remove(bookmark);
            _ReadLaterDataContext.SaveChanges();
        }

        public void DeleteBookmark(int Id)
        {
            Bookmark bookmarkToRemove = _ReadLaterDataContext.Bookmarks.Where(b => b.ID == Id).FirstOrDefault();
            _ReadLaterDataContext.Remove(bookmarkToRemove);
            _ReadLaterDataContext.SaveChanges();
        }
    }
}
