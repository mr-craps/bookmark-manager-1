using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using DTO;
using Common.Mappers;

namespace ReadLater5.Controllers
{
    public class BookmarksController : Controller
    {
        private readonly IBookmarkService _bookmarkService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public BookmarksController(IBookmarkService bookmarkService, ICategoryService categoryService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Bookmarks
        public IActionResult Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden);
            }

            List<BookmarkDTO> model = _bookmarkService
                                            .GetBookmarks(_userManager.GetUserId(HttpContext.User))
                                            .Select(b => BookmarkMapper.MapEntityToDto(b))
                                            .ToList();
            return View(model);
        }

        // GET: Bookmarks/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            if (bookmark == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            if (bookmark.UserId != _userManager.GetUserId(HttpContext.User))
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden);
            }
            return View(BookmarkMapper.MapEntityToDto(bookmark));
        }

        // GET: Bookmarks/Create
        public IActionResult Create()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden);
            }

            ViewBag.Categories = GetCategoriesForDropdown(_userManager.GetUserId(HttpContext.User));

            return View();
        }

        // POST: Bookmarks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookmarkDTO dtoBookmark)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = _userManager.GetUserId(HttpContext.User);

                Bookmark bookmark = BookmarkMapper.MapDtoToEntity(dtoBookmark);                
                bookmark.UserId = currentUserId;
                bookmark.CreateDate = DateTime.UtcNow;

                //bookmark.CategoryId = dtoBookmark.CategoryId;

                _bookmarkService.CreateBookmark(bookmark);

                return RedirectToAction("Index");
            }

            ViewBag.Categories = GetCategoriesForDropdown(_userManager.GetUserId(HttpContext.User));

            return View(dtoBookmark);
        }

        // GET: Bookmarks/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            if (bookmark == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            if (bookmark.UserId != _userManager.GetUserId(HttpContext.User))
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden);
            }

            ViewBag.Categories = GetCategoriesForDropdown(_userManager.GetUserId(HttpContext.User));

            return View(BookmarkMapper.MapEntityToDto(bookmark));
        }

        // POST: Bookmarks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookmarkDTO bookmarkDto)
        {
            if (ModelState.IsValid)
            {
                Bookmark bookmark = _bookmarkService.GetBookmark(bookmarkDto.ID);

                if (bookmark != null)
                {
                    bookmark.URL = bookmarkDto.URL;
                    bookmark.ShortDescription = bookmarkDto.ShortDescription;
                    bookmark.CategoryId = bookmarkDto.CategoryId;

                    _bookmarkService.UpdateBookmark(bookmark);
                }
                
                return RedirectToAction("Index");
            }

            ViewBag.Categories = GetCategoriesForDropdown(_userManager.GetUserId(HttpContext.User));

            return View(bookmarkDto);
        }

        // GET: Bookmarks/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            if (bookmark == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            if (bookmark.UserId != _userManager.GetUserId(HttpContext.User))
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden);
            }
            return View(BookmarkMapper.MapEntityToDto(bookmark));
        }

        // POST: Bookmarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Bookmark bookmark = _bookmarkService.GetBookmark(id);
            _bookmarkService.DeleteBookmark(bookmark);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult _AddCategory(string categoryName)
        {
            Category category = new Category()
            { 
                Name = categoryName, 
                UserId = _userManager.GetUserId(HttpContext.User) 
            };

            Category savedCategory = _categoryService.CreateCategory(category);

            return Ok(this.Json(new { ID = savedCategory.ID, Name = savedCategory.Name }));
        }

        private List<SelectListItem> GetCategoriesForDropdown(string userId)
        {
            List<Category> categories = _categoryService.GetCategories(userId);
            List<SelectListItem> listCategories = new List<SelectListItem>();
            //listCategories.Add(new SelectListItem("-- Select Category --", "0"));
            foreach (Category c in categories)
            {
                listCategories.Add(new SelectListItem(c.Name, c.ID.ToString()));
            }
            return listCategories;
        }
    }
}
