using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Searchwar_netModel;

/// <summary>
/// Summary description for BlogSystem
/// </summary>
namespace SearchWar.BlogSystem {
    public class BlogSystem {
        public BlogSystem() {
            //
            // TODO: Add constructor logic here
            //
        }


        public void CreateBolig(string title,
            string text,
            int langid,
            Guid UserID)
        {

            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {

                SW_Blog createblog = new SW_Blog();
                createblog.BlogAddedByUserId = UserID;
                createblog.BlogEditByUserId = UserID;
                createblog.BlogAddedDate = TimeZoneManager.DateTimeNow;
                createblog.BlogEditDate = TimeZoneManager.DateTimeNow;

                db.SaveChanges();

                SW_BlogData createBlogData = new SW_BlogData();
                createBlogData.BlogId = createblog.BlogId;
                createBlogData.BlogText = text;
                createBlogData.LangId = langid;
                createBlogData.BlogTitle = title;
                
                db.SW_Blog.AddObject(createblog);

            }

        }


        public List<BlogObject> GetBlogs(int langId, 
            int pageIndex, 
            int pageLimit) {

            using (Searchwar_netEntities db = new Searchwar_netEntities()) {

                return (from b in db.SW_Blog
                        join bd in db.SW_BlogData on b.BlogId equals bd.BlogId
                        where bd.LangId == langId
                        select new BlogObject
                        {
                            BlogId = (int)b.BlogId,
                            BlogTitle = (string)bd.BlogTitle,
                            BlogText = (string)bd.BlogText,
                            BlogAddedDate = (DateTime)b.BlogAddedDate,
                            BlogEditDate = (DateTime)b.BlogEditDate,
                            BlogAddedUserId = (Guid)b.BlogAddedByUserId,
                            BlogEditUserId = (Guid)b.BlogEditByUserId
                        }).OrderBy(b => b.BlogAddedDate).Skip(pageIndex == 0 ? 0 : Convert.ToInt32((pageIndex * pageLimit))).Take(pageLimit).ToList<BlogObject>();
            
            }

        }
    }
}
