using Lia.Blog.Application.Interfaces;
using Lia.Blog.Domain.Entity;
using Lia.Blog.Domain.Model;
using Lia.Blog.Utils;
using Lia.Blog.Web.Models;
using Microsoft.Practices.ServiceLocation;
using StackExchange.Profiling;
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
        private readonly IUserService _userService;

        public BlogController()
        {
            _blogService = ServiceLocator.Current.GetInstance<IBlogService>();
            _categoryService = ServiceLocator.Current.GetInstance<ICategoryService>();
            _userService = ServiceLocator.Current.GetInstance<IUserService>();
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
            var profiler = MiniProfiler.Current;
            using (profiler.Step("查询博客列表"))
            {
                var list = new List<BlogModel>().Bind(_blogService.GetBlogList(parameter), parameter.RecordCount);
                return Json(new { Items = list, TotalCount = parameter.RecordCount }, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult Detail(string id = "")
        {
            if (string.IsNullOrEmpty(id))
                return View(new BlogDetail());
            
            var profiler = MiniProfiler.Current;
            using (profiler.Step("根据博客id查询博客"))
            {
                var view = _blogService.GetList(t => t.Id.Equals(id)).Select(b => new BlogDetail()
                {
                    Url = b.Url,
                    Title = b.Title,
                    Body = b.Body,
                    AuthorName = b.User == null ? "" : b.User.NickName,
                    CategoryName = b.Category == null ? "" : b.Category.CategoryName
                }).FirstOrDefault();
                return View(view);
            }
        }


        public ActionResult Form(string id = "")
        {
            GetCategories();
            var model = _blogService.GetBlogById(id);
            var view = new BlogItem() { Id = model.Id, Title = model.Title, Body = model.Body, CategoryId = model.CategoryId };
            return View(view);
        }

        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> Form(BlogItem view)
        {
            GetCategories();
            if (CurrentUser == null || string.IsNullOrEmpty(CurrentUser.Id))
                return Javascript("用户信息无效，请重新登录", "/Front/Account/Login");
            if (string.IsNullOrEmpty(view.Title))
                return Error("标题不能为空");
            if (string.IsNullOrEmpty(view.Body))
                return Error("内容不能为空");
            if (string.IsNullOrEmpty(view.CategoryId))
                return Error("请选择分类");
            if (ModelState.IsValid)
            {
                var model = _blogService.GetBlogById(view.Id);
                var isAdd = false;
                if (model == null)//新增
                {
                    isAdd = true;
                    model = new BlogInfo();
                    model.AuthorId = CurrentUser.Id;
                    model.Url = string.Format("Form/{0}", model.Id);//"1234.html";
                    model.IsForwarding = false;
                }
                model.Title = view.Title;
                model.Body = view.Body;
                model.CategoryId = view.CategoryId;
                await _blogService.Save(model, isAdd);
                var msg = string.Concat(isAdd ? "添加" : "修改", "成功");
                return Javascript(msg, "/Front/Home/Index");
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
