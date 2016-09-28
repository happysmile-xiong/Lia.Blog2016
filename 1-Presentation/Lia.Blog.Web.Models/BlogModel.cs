using Lia.Blog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Web.Models
{
    public class BlogModel
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }

        public string AuthorName { get; set; }

        public string PostTime { get; set; }

        public int CommentNum { get; set; }

        public int ReadNum { get; set; }
    }

    public class BlogDetail {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public string CategoryName { get; set; }

        public string AuthorName { get; set; }
    }

    public class BlogItem {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        
        public string CategoryId { get; set; }

        public string PostTime { get; set; }
    }

    public static class BlogModelExtension
    {
        public static BlogModel SetBy(this BlogModel view, dynamic blog)
        {
            if (blog == null)
                return null;

            view.Id = blog.Id;
            view.Url = string.Format("/Front/Blog/{0}", blog.Url);
            view.Title = blog.Title;
            view.Summary = string.Concat(blog.Body.Length <= 150 ? blog.Body : blog.Body.Substring(0, 150), "...");
            view.AuthorName = blog.AuthorName;
            view.PostTime = blog.CreationTime.ToString("yyyy-MM-dd HH:mm");
            view.CommentNum = 0;
            view.ReadNum = 0;
            return view;
        }

        public static IList<BlogModel> Bind(this IList<BlogModel> list, IQueryable<BlogInfo> blogs,int count=0)
        {
            if (count <= 0)//(blogs == null || (!blogs.Any()))
                return list;
            
            var result = blogs.Select(b => new
            {
                Id = b.Id,
                Url = b.Url,
                Title = b.Title,
                Body = b.Body,
                AuthorName = b.User.NickName,
                CreationTime = b.CreationTime,
                CommentNum = 0,
                ReadNum = 0
            }).ToList();

            foreach(var item in result)
            {
                var view = new BlogModel().SetBy(item);
                if (view != null)
                    list.Add(view);
            }
            
            return list;
        }
    }
}
