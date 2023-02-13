using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopList.Data;
using TopList.Entity.EntityModels;
using TopList.Repo;
using TopList.Services;
using TopList.Services.Events;
using TopList.ViewModels;
using Microsoft.Net.Http.Headers;

namespace TopList.Controllers
{
    public class CategoriesController : Controller
    {
        private int _pageSize;
        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly IMediaService _mediaService;
        public CategoriesController(IMediaService mediaService,IConfiguration config,IRepository<Company> companyRepository, ApplicationDbContext context, ICategoryService categoryService, IRepository<Category> categoryRepository)
        {
            _context = context;
            _categoryService= categoryService;
            _categoryRepository= categoryRepository;
            _companyRepository = companyRepository;
            _pageSize = config.GetValue<int>("10");
            _mediaService= mediaService;
        }

        public async Task<IActionResult> Post(CategoryForm model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = model.Name,
                    Slug = model.Slug,
                    MetaTitle = model.MetaTitle,
                    MetaKeywords = model.MetaKeywords,
                    MetaDescription = model.MetaDescription,
                    Description = model.Description,
                    ParentId = model.ParentId,
                    IncludeInMenu = model.IncludeInMenu,
                    IsPublished = model.IsPublished
                };

                await SaveCategoryImage(category, model);
                await _categoryService.Create(category);
                return View(model);
            }

            return BadRequest(ModelState);
        }

        public async Task<IActionResult> Get()
        {
            var gridData = await _categoryService.GetAll();
            return View(gridData);
        }

        public IActionResult CategoryDetail(long id, TopList.ViewModels.SearchOption searchOption)
        {
            var category = _categoryRepository.Query().FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return Redirect("~/Error/FindNotFound");
            }

            var model = new CompanysByCategory
            {
                CategoryId = category.Id,
                ParentCategorId = category.ParentId,
                CategoryName = category.Name,
                CategorySlug = category.Slug,
                CategoryMetaTitle = category.MetaTitle,
                CategoryMetaKeywords = category.MetaKeywords,
                CategoryMetaDescription = category.MetaDescription,
                CurrentSearchOption = searchOption,
                FilterOption = new FilterOption()
            };

            var query = _companyRepository
                .Query()
                .Where(x => x.Categories.Any(c => c.CategoryId == category.Id) && x.IsPublished);

            if (query.Count() == 0)
            {
                model.TotalCompany = 0;
                return View(model);
            }

            AppendFilterOptionsToModel(model, query);

           

            var categories = searchOption.GetCategories();
            if (categories.Any())
            {
                query = query.Where(p => p.Categories.Select(c => c.CategoryId).Intersect(_categoryRepository.Query().Where(cat => categories.Contains(cat.Slug)).Select(c => c.Id)).Any());
            }

         

            model.TotalCompany = query.Count();
            var currentPageNum = searchOption.Page <= 0 ? 1 : searchOption.Page;
            var offset = (_pageSize * currentPageNum) - _pageSize;
            while (currentPageNum > 1 && offset >= model.TotalCompany)
            {
                currentPageNum--;
                offset = (_pageSize * currentPageNum) - _pageSize;
            }

           

            var companys = query
                .Include(x => x.ThumbnailImage)
                .Skip(offset)
                .Take(_pageSize)
                .Select(x => CompanyThumbnail.FromCompany(x))
                .ToList();

            foreach (var company in companys)
            {
                company.Name = model.CategoryName;
                company.ThumbnailUrl = _mediaService.GetThumbnailUrl(company.ThumbnailImage);
            }

            model.Companys = companys;
            model.CurrentSearchOption.PageSize = _pageSize;
            model.CurrentSearchOption.Page = currentPageNum;

            return View(model);
        }
        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Categories.Include(c => c.Parent).Include(c => c.ThumbnailImage);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Parent)
                .Include(c => c.ThumbnailImage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["ThumbnailImageId"] = new SelectList(_context.Medias, "Id", "Id");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Slug,MetaTitle,MetaKeywords,MetaDescription,Description,DisplayOrder,IsPublished,IncludeInMenu,IsDeleted,ParentId,ThumbnailImageId,Id")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name", category.ParentId);
            ViewData["ThumbnailImageId"] = new SelectList(_context.Medias, "Id", "Id", category.ThumbnailImageId);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name", category.ParentId);
            ViewData["ThumbnailImageId"] = new SelectList(_context.Medias, "Id", "Id", category.ThumbnailImageId);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Name,Slug,MetaTitle,MetaKeywords,MetaDescription,Description,DisplayOrder,IsPublished,IncludeInMenu,IsDeleted,ParentId,ThumbnailImageId,Id")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name", category.ParentId);
            ViewData["ThumbnailImageId"] = new SelectList(_context.Medias, "Id", "Id", category.ThumbnailImageId);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Parent)
                .Include(c => c.ThumbnailImage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(long id)
        {
          return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        private void AppendFilterOptionsToModel(CompanysByCategory model, IQueryable<Company> query)
        {



            model.FilterOption.Categories = query
                .SelectMany(x => x.Categories)
                .GroupBy(x => new
                {
                    x.Category.Id,
                    x.Category.Name,
                    x.Category.Slug,
                    x.Category.ParentId
                })
                .Select(g => new FilterCategory
                {
                    Id = (int)g.Key.Id,
                    Name = g.Key.Name,
                    Slug = g.Key.Slug,
                    ParentId = g.Key.ParentId,
                    Count = g.Count()
                }).ToList();

          
        }
        private async Task SaveCategoryImage(Category category, CategoryForm model)
        {
            if (model.ThumbnailImage != null)
            {
                var fileName = await SaveFile(model.ThumbnailImage);
                if (category.ThumbnailImage != null)
                {
                    category.ThumbnailImage.FileName = fileName;
                }
                else
                {
                    category.ThumbnailImage = new Media { FileName = fileName };
                }
            }
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Value.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _mediaService.SaveMediaAsync(file.OpenReadStream(), fileName, file.ContentType);
            return fileName;
        }
    }
}
