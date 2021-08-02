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
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<IdentityUser> _userManager;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoryService categoryService, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoryDTO>> GetAll()
        {
            return Ok(
                    _categoryService
                        .GetCategories(_userManager.GetUserId(HttpContext.User))
                        .Select(b => CategoryMapper.MapEntityToDto(b))
                   );
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<CategoryDTO> GetCategoryById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            Category c = _categoryService.GetCategory(id);
            if (c == null)
            {
                return NotFound();
            }

            string currentUserId = _userManager.GetUserId(HttpContext.User);
            if (c.UserId.ToLower() != currentUserId.ToLower())
            {
                return Forbid();
            }

            return Ok(CategoryMapper.MapEntityToDto(c));
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteCategoryById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            Category c = _categoryService.GetCategory(id);
            if (c == null)
            {
                return NotFound();
            }

            string currentUserId = _userManager.GetUserId(HttpContext.User);
            if (c.UserId.ToLower() != currentUserId.ToLower())
            {
                return Forbid();
            }

            _categoryService.DeleteCategory(id);

            return Ok();
        }

        [HttpPost]
        public ActionResult<CategoryDTO> CreateCategory([FromBody] CategoryDTO dtoCategory)
        {
            if (dtoCategory.Name == null || dtoCategory.Name.Trim() == String.Empty)
            {
                return BadRequest();
            }

            Category category = CategoryMapper.MapDtoToEntity(dtoCategory);
            category.UserId = _userManager.GetUserId(HttpContext.User);

            return Ok(CategoryMapper.MapEntityToDto(_categoryService.CreateCategory(category)));
        }

        [HttpPut]
        public ActionResult<CategoryDTO> UpdateCategory([FromBody] CategoryDTO dtoCategory)
        {
            Category category = _categoryService.GetCategory(dtoCategory.ID);

            if (category == null)
            {
                return NotFound();
            }

            if (dtoCategory.Name == null || dtoCategory.Name.Trim() == String.Empty)
            {
                return BadRequest();
            }

            string currentUserId = _userManager.GetUserId(HttpContext.User);

            if (category.UserId.ToLower() != currentUserId.ToLower())
            {
                return Forbid();
            }            

            category.Name = dtoCategory.Name;

            _categoryService.UpdateCategory(category);

            return Ok(CategoryMapper.MapEntityToDto(category));
        }
    }
}
