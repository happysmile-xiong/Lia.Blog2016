using Lia.Blog.Application.Interfaces;
using Lia.Blog.Domain.Entity;
using Lia.Blog.Domain.Model;
using Lia.Blog.Utils;
using Lia.Blog.Web.Models;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Lia.Blog.Web.Controllers.Front
{
    [Authorize]
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;

        public BlogController()
        {
            _blogService = ServiceLocator.Current.GetInstance<IBlogService>();
            _categoryService = ServiceLocator.Current.GetInstance<ICategoryService>();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public JsonResult GetList()
        {
            var parameter = new BlogParameter()
            {
                PageIndex = RequestHelper.Query("p").ToInt(0),
                PageSize = RequestHelper.Query("s").ToInt(0),
                OrderBy = RequestHelper.Query("orderby"),
                IsAsc = RequestHelper.Query("sort").ToLower().Equals("asc"),
            };
            var list = new List<BlogModel>().Bind(_blogService.GetBlogList(parameter));
            return Json(new { Items = list, TotalCount = parameter.RecordCount }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Form()
        {
            GetCategories();
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> Form(BlogItem view)
        {
            GetCategories();

            if (CurrentUser == null || string.IsNullOrEmpty(CurrentUser.Id))
                return Content("<script>alert('用户信息无效，请重新登录');location.href='/Front/Account/Login';</script>");

            if (string.IsNullOrEmpty(view.Title))
            {
                ModelState.AddModelError("", "标题不能为空");
                return View();
            }
            if (string.IsNullOrEmpty(view.Body))
            {
                ModelState.AddModelError("", "内容不能为空");
                return View();
            }
            if (string.IsNullOrEmpty(view.CategoryId))
            {
                ModelState.AddModelError("", "请选择分类");
                return View();
            }
            if (ModelState.IsValid)
            {
                var model = new BlogInfo()
                {
                    Title = view.Title,
                    Body = view.Body,
                    CategoryId = view.CategoryId,
                    AuthorId = CurrentUser.Id,
                    Url = "1234.html",
                    IsForwarding = false
                };
                await _blogService.Insert(model);
                return Content("<script>alert('添加成功');location.href='/Front/Home/Index';</script>");
            }
            return View();
        }

        public void GetCategories()
        {
            var cateList = _categoryService.GetCategories();
            ViewBag.Categories = new SelectList(cateList, "Id", "CategoryName");
        }
    }
}
