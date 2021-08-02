using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Collections.Generic;
using System.Linq;
using Entity;
using DTO;
using Common.Mappers;

namespace ReadLater5.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CategoriesController(ICategoryService categoryService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _categoryService = categoryService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Categories
        public IActionResult Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden);
            }
            List<CategoryDTO> model = _categoryService
                                        .GetCategories(_userManager.GetUserId(HttpContext.User))
                                        .Select(c => CategoryMapper.MapEntityToDto(c))
                                        .ToList();
            return View(model);
        }

        // GET: Categories/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Category category = _categoryService.GetCategory((int)id);
            if (category == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            if (category.UserId != _userManager.GetUserId(HttpContext.User))
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden);
            }
            return View(CategoryMapper.MapEntityToDto(category));
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden);
            }
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryDTO dtoCategory)
        {
            if (ModelState.IsValid)
            {
                Category category = CategoryMapper.MapDtoToEntity(dtoCategory);
                category.UserId = _userManager.GetUserId(HttpContext.User);
                _categoryService.CreateCategory(category);
                return RedirectToAction("Index");
            }

            return View(dtoCategory);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Category category = _categoryService.GetCategory((int)id);
            if (category == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            if (category.UserId != _userManager.GetUserId(HttpContext.User))
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden);
            }
            return View(CategoryMapper.MapEntityToDto(category));
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {
                Category category = _categoryService.GetCategory(categoryDTO.ID);
                if (category != null)
                {
                    category.Name = categoryDTO.Name;
                    _categoryService.UpdateCategory(category);
                }                
                return RedirectToAction("Index");
            }
            return View(categoryDTO);
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Category category = _categoryService.GetCategory((int)id);
            if (category == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            if (category.UserId != _userManager.GetUserId(HttpContext.User))
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden);
            }
            return View(CategoryMapper.MapEntityToDto(category));
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Category category = _categoryService.GetCategory(id);
            _categoryService.DeleteCategory(category);
            return RedirectToAction("Index");
        }
    }
}
