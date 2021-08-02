using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Entity;
using DTO;
using Services;
using Common.Mappers;

namespace ReadLaterApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookmarksController : ControllerBase
    {
        private readonly ILogger<BookmarksController> _logger;
        private readonly IBookmarkService _bookmarkService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<IdentityUser> _userManager;

        public BookmarksController(ILogger<BookmarksController> logger, IBookmarkService bookmarkService, ICategoryService categoryService, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookmarkDTO>> GetAll()
        {
            return Ok(
                    _bookmarkService
                        .GetBookmarks(_userManager.GetUserId(HttpContext.User))
                        .Select(b => BookmarkMapper.MapEntityToDto(b))
                   );
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<BookmarkDTO> GetBookmarkById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            Bookmark b = _bookmarkService.GetBookmark(id);
            if (b == null)
            {
                return NotFound();
            }

            string currentUserId = _userManager.GetUserId(HttpContext.User);
            if (b.UserId.ToLower() != currentUserId.ToLower())
            {
                return Forbid();
            }

            return Ok(BookmarkMapper.MapEntityToDto(b));
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteBookmarkById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            Bookmark b = _bookmarkService.GetBookmark(id);
            if (b == null)
            {
                return NotFound();
            }

            string currentUserId = _userManager.GetUserId(HttpContext.User);
            if (b.UserId.ToLower() != currentUserId.ToLower())
            {
                return Forbid();
            }

            _bookmarkService.DeleteBookmark(id);

            return Ok();
        }

        [HttpPost]
        public ActionResult<BookmarkDTO> CreateBookmark([FromBody] BookmarkDTO dtoBookmark)
        {
            if (dtoBookmark.URL.Trim() == String.Empty || dtoBookmark.ShortDescription.Trim() == String.Empty || dtoBookmark.CategoryId <= 0)
            {
                return BadRequest("Invalid parameters!");
            }

            string currentUserId = _userManager.GetUserId(HttpContext.User);

            Bookmark bookmark = BookmarkMapper.MapDtoToEntity(dtoBookmark);
            bookmark.UserId = currentUserId;
            bookmark.CreateDate = DateTime.UtcNow;

            var existingCategory =
                    _categoryService.GetCategories(currentUserId)
                                    .Where(c => c.ID == dtoBookmark.CategoryId)
                                    .FirstOrDefault();

            if (existingCategory == null)
            {
                //Category not found or linked to another user!
                return BadRequest("Category not found!");
            }

            return Ok(BookmarkMapper.MapEntityToDto(_bookmarkService.CreateBookmark(bookmark)));
        }

        [HttpPut]
        public ActionResult<BookmarkDTO> UpdateBookmark([FromBody] BookmarkDTO dtoBookmark)
        {
            Bookmark bookmark = _bookmarkService.GetBookmark(dtoBookmark.ID);

            if (bookmark == null)
            {
                return NotFound();
            }

            string currentUserId = _userManager.GetUserId(HttpContext.User);

            if (bookmark.UserId.ToLower() != currentUserId.ToLower())
            {
                return Forbid();
            }

            var existingCategory =
                _categoryService.GetCategories(currentUserId)
                                .Where(c => c.ID == dtoBookmark.CategoryId)
                                .FirstOrDefault();

            if (existingCategory == null)
            {
                //Category not found or linked to another user!
                return BadRequest("Category not found!");
            }

            bookmark.URL = dtoBookmark.URL;
            bookmark.ShortDescription = dtoBookmark.ShortDescription;
            bookmark.CategoryId = dtoBookmark.CategoryId;            

            _bookmarkService.UpdateBookmark(bookmark);

            return Ok(BookmarkMapper.MapEntityToDto(bookmark));                        
        }
    }
}
