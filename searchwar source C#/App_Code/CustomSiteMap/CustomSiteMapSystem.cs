using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Security;
using Searchwar_netModel;
using SearchWar.LangSystem;

/// <summary>
/// CustomSiteMap with LINQ (Database)
/// </summary>
namespace SearchWar.SiteMap
{
    public class CustomSiteMapSystem
    {
        
        public CustomSiteMapSystem()
        {
            _siteMapNodeIds = new List<int>();
        }


        #region searchSiteMapNode

        /// <summary>
        /// Searching after path in database
        /// </summary>
        /// <param name="path">Insert path</param>
        /// <param name="siteMapNodeId"></param>
        /// <param name="langId">Insert langId</param>
        /// <returns>Return anonymous object</returns>
        protected cSiteMapNode GetSiteMapNode_Single(int siteMapNodeId,
            int langId) {
            Searchwar_netEntities db = new Searchwar_netEntities();

                // Get UserRoles
                string[] roleNamesByUser = Roles.GetRolesForUser();

                LangaugeSystem ls = new LangaugeSystem();
                string langShortName = ls.CurrentLang;

                // Get the cSiteMapNode by the "path"
                cSiteMapNode cSiteMapNodeNode = (from s in db.SW_SiteMapNode
                                          join sd in db.SW_SiteMapNodeData
                                              on s.SiteMapNodeId equals sd.SiteMapNodeId
                                          where s.SiteMapNodeId == siteMapNodeId &&
                                                sd.LangId == langId &&
                                                (s.SW_SiteMapNodeRoles.Count() != 0 ? s.SW_SiteMapNodeRoles.Where(r => roleNamesByUser.Contains(r.aspnet_Roles.RoleName)).Count() > 0 : true) == true
                                          select new cSiteMapNode
                                          {
                                              SiteMapNodeId = (int)s.SiteMapNodeId,
                                              SiteMapNodeSubId = (int?)s.SiteMapNodeSubId,
                                              SiteMapNodePath = (string)s.SiteMapNodePath,
                                              SiteMapNodeRewrittedPath = (string)sd.SiteMapNodeRewriteUrl == null ? "" : "~/" + sd.SiteMapNodeRewriteUrl,
                                              SiteMapNodeAddedUserId = (Guid)s.SiteMapNodeAddedUserId,
                                              SiteMapNodeEditUserId = (Guid)s.SiteMapNodeEditUserId,
                                              SiteMapNodeShow = (bool)s.SiteMapNodeShow,
                                              SiteMapNodeAddedDate = (DateTime)s.SiteMapNodeAddedDate,
                                              SiteMapNodeEditDate = (DateTime)s.SiteMapNodeEditDate,
                                              SiteMapNodeTitle = (string)sd.SiteMapNodeTitle,
                                              SiteMapNodeSort = (int)s.SiteMapNodeSort,
                                              LangId = (int)sd.LangId,
                                              SiteMapNodeRoles = s.SW_SiteMapNodeRoles.Where(r => r.SiteMapNodeId == s.SiteMapNodeId)
                                              // Check if user is allowed to see SiteMapNodes
                                          }).SingleOrDefault<cSiteMapNode>();
                
                // Checking cSiteMapNode is in databasen
                if (cSiteMapNodeNode != null) {

                    return cSiteMapNodeNode;

                } else {

                    throw (new ArgumentNullException("siteMapNodeId", "Cant find '" + siteMapNodeId + "' in database"));

                }

        }

        /// <summary>
        /// Searching after path in database
        /// </summary>
        /// <param name="path">Insert path</param>
        /// <param name="langId">Insert langId</param>
        /// <returns>Return anonymous object</returns>
        protected cSiteMapNode SearchSiteMapNode(string path,
            int langId) {
            Searchwar_netEntities db = new Searchwar_netEntities();

                // Get the CurrentPath if "path" is null
                if (string.IsNullOrEmpty(path)) {

                    path = HttpContext.Current.Application["rawurl"].ToString();
                    
                }

                // Get UserRoles
                string[] roleNamesByUser = Roles.GetRolesForUser();

                // Get the cSiteMapNode by the "path"
                cSiteMapNode cSiteMapNodeNode = (from s in db.SW_SiteMapNode
                                          join sd in db.SW_SiteMapNodeData
                                              on s.SiteMapNodeId equals sd.SiteMapNodeId
                                          where s.SiteMapNodePath.ToLower() == path.ToLower() &&
                                                sd.LangId == langId &&
                                                (s.SW_SiteMapNodeRoles.Count() != 0 ? s.SW_SiteMapNodeRoles.Where(r => roleNamesByUser.Contains(r.aspnet_Roles.RoleName)).Count() > 0 : true) == true
                                          select new cSiteMapNode
                                                     {
                                                         SiteMapNodeId = (int)s.SiteMapNodeId,
                                                         SiteMapNodeSubId = (int?)s.SiteMapNodeSubId,
                                                         SiteMapNodePath = (string)s.SiteMapNodePath,
                                                         SiteMapNodeAddedUserId = (Guid)s.SiteMapNodeAddedUserId,
                                                         SiteMapNodeEditUserId = (Guid)s.SiteMapNodeEditUserId,
                                                         SiteMapNodeShow = (bool)s.SiteMapNodeShow,
                                                         SiteMapNodeAddedDate = (DateTime)s.SiteMapNodeAddedDate,
                                                         SiteMapNodeEditDate = (DateTime)s.SiteMapNodeEditDate,
                                                         SiteMapNodeTitle = (string)sd.SiteMapNodeTitle,
                                                         SiteMapNodeSort = (int)s.SiteMapNodeSort,
                                                         LangId = (int)sd.LangId,
                                                         SiteMapNodeRoles = s.SW_SiteMapNodeRoles.Where(r => r.SiteMapNodeId == s.SiteMapNodeId)
                                                         // Check if user is allowed to see SiteMapNodes
                                                     }).SingleOrDefault<cSiteMapNode>();

                // Checking cSiteMapNode is in databasen
                if (cSiteMapNodeNode != null) {

                    return cSiteMapNodeNode;

                } else {

                    throw (new ArgumentNullException("path", "Cant find '" + path + "' in database"));

                }

        }


        /// <summary>
        /// Get parentpath of the inserted path
        /// </summary>
        /// <param name="path">Insert path</param>
        /// <returns>Return parentpath - anonymous object</returns>
        protected cSiteMapNode SearchParentSiteMapNode(string path)
        {
            Searchwar_netEntities db = new Searchwar_netEntities();

                // Get UserRoles
                string[] roleNamesByUser = Roles.GetRolesForUser();

                // Find the parentCSiteMapNode by "path"
                cSiteMapNode parentCSiteMapNode = (from s in db.SW_SiteMapNode
                                            join sd in db.SW_SiteMapNodeData
                                                on s.SiteMapNodeId equals sd.SiteMapNodeId
                                            join ss in db.SW_SiteMapNode
                                                on s.SiteMapNodeId equals ss.SiteMapNodeId
                                            where ss.SiteMapNodePath.ToLower() == path.ToLower() &&
                                                  (s.SW_SiteMapNodeRoles.Count() != 0 ? s.SW_SiteMapNodeRoles.Where(r => roleNamesByUser.Contains(r.aspnet_Roles.RoleName)).Count() > 0 : true) == true
                                            select new cSiteMapNode
                                                       {
                                                           SiteMapNodeId = (int)s.SiteMapNodeId,
                                                           SiteMapNodeSubId = (int?)s.SiteMapNodeSubId,
                                                           SiteMapNodePath = (string)s.SiteMapNodePath,
                                                           SiteMapNodeAddedUserId = (Guid)s.SiteMapNodeAddedUserId,
                                                           SiteMapNodeEditUserId = (Guid)s.SiteMapNodeEditUserId,
                                                           SiteMapNodeShow = (bool)s.SiteMapNodeShow,
                                                           SiteMapNodeAddedDate = (DateTime)s.SiteMapNodeAddedDate,
                                                           SiteMapNodeEditDate = (DateTime)s.SiteMapNodeEditDate,
                                                           SiteMapNodeTitle = (string)sd.SiteMapNodeTitle,
                                                           LangId = (int)sd.LangId,
                                                           SiteMapNodeRoles = s.SW_SiteMapNodeRoles.Where(r => r.SiteMapNodeId == s.SiteMapNodeId).ToList
                                           <SW_SiteMapNodeRole>()
                                                           // Check if user is allowed to see SiteMapNodes
                                                       }).SingleOrDefault < cSiteMapNode>();


                // Checking cSiteMapNode is in database
                if (parentCSiteMapNode != null) {

                    return parentCSiteMapNode;

                } else {

                    throw (new ArgumentNullException("Path", "Found no ParentPath in database for '" + path + "'"));

                }

        } 
        #endregion

        #region getSiteMapNodes
        /// <summary>
        /// Get all MainPaths (MainPaths is paths without a SiteMapSubId)
        /// </summary>
        /// <returns>Return a list of anonymous objects</returns>
        protected List<cSiteMapNode> GetMainSiteMapNodes(int langId, 
            bool siteMapNodeShow) {
            Searchwar_netEntities db = new Searchwar_netEntities();

                // Get UserRoles
            LangaugeSystem ls = new LangaugeSystem();
                string[] roleNamesByUser = Roles.GetRolesForUser();
                string langShortName = ls.CurrentLang;

                // Get all SiteMaps
                return (from s in db.SW_SiteMapNode
                        join sd in db.SW_SiteMapNodeData
                            on s.SiteMapNodeId equals sd.SiteMapNodeId
                        where s.SiteMapNodeSubId == null
                              && (siteMapNodeShow == true ? true : s.SiteMapNodeShow == true)
                              && sd.LangId == langId &&
                              // Check if user is allowed to see SiteMaps
                              (s.SW_SiteMapNodeRoles.Count() != 0
                                   ? s.SW_SiteMapNodeRoles.Where(r => roleNamesByUser.Contains(r.aspnet_Roles.RoleName)).
                                         Count() > 0
                                   : true) == true
                        orderby s.SiteMapNodeSort
                        select new cSiteMapNode
                                   {
                                       SiteMapNodeId = (int) s.SiteMapNodeId,
                                       SiteMapNodePath = (string) s.SiteMapNodePath,
                                       SiteMapNodeRewrittedPath =
                                           String.IsNullOrEmpty(sd.SiteMapNodeRewriteUrl)
                                               ? ""
                                               : (string)"~/" + sd.SiteMapNodeRewriteUrl,
                                       SiteMapNodeAddedUserId = (Guid) s.SiteMapNodeAddedUserId,
                                       SiteMapNodeEditUserId = (Guid) s.SiteMapNodeEditUserId,
                                       SiteMapNodeShow = (bool) s.SiteMapNodeShow,
                                       SiteMapNodeAddedDate = (DateTime) s.SiteMapNodeAddedDate,
                                       SiteMapNodeEditDate = (DateTime) s.SiteMapNodeEditDate,
                                       SiteMapNodeTitle = (string) sd.SiteMapNodeTitle,
                                       LangId = (int) sd.LangId,
                                       SiteMapNodeRoles =
                                           s.SW_SiteMapNodeRoles.Where(r => r.SiteMapNodeId == s.SiteMapNodeId)
                                   }).ToList<cSiteMapNode>();

        }


        public List<SiteMapNodeRegex> GetAllSiteMapNodeRegexAndUrl()
        {

            // using statement free the memory after metode is done
            Searchwar_netEntities db = new Searchwar_netEntities();
                // Get UserRoles
                string[] roleNamesByUser = Roles.GetRolesForUser();

                // Get all SiteMaps
                return (from s in db.SW_SiteMapNode
                        join sd in db.SW_SiteMapNodeData
                        on s.SiteMapNodeId equals sd.SiteMapNodeId
                        select new SiteMapNodeRegex
                        {
                            Name = System.Data.Objects.SqlClient.SqlFunctions.StringConvert(sd.SiteMapNodeId + 0.0) + System.Data.Objects.SqlClient.SqlFunctions.StringConvert(sd.LangId + 0.0),
                            Regex = (string)sd.SiteMapNodeRewriteRegex,
                            ToUrl = (string)s.SiteMapNodeRewriteToUrl
                        }).ToList<SiteMapNodeRegex>();


        }


        /// <summary>
        /// Get all SubPaths
        /// </summary>
        /// <param name="langId">Insert langId</param>
        /// <param name="siteMapNodeId">Insert SiteMapId</param>
        /// <returns>Return a list of anonymous objects</returns>
        protected List<cSiteMapNode> GetSubSiteMapNodes(int langId,
            int siteMapNodeId) {

            Searchwar_netEntities db = new Searchwar_netEntities();

                // Get UserRoles
                string[] roleNamesByUser = Roles.GetRolesForUser();

                // Get all SiteMaps
                LangaugeSystem ls = new LangaugeSystem();

                return (from s in db.SW_SiteMapNode
                        join sd in db.SW_SiteMapNodeData
                        on s.SiteMapNodeId equals sd.SiteMapNodeId
                        where s.SiteMapNodeSubId == siteMapNodeId
                        && s.SiteMapNodeShow == true
                        && sd.LangId == langId &&
                            // Check if user is allowed to see SiteMaps
                        (s.SW_SiteMapNodeRoles.Count() != 0 ? s.SW_SiteMapNodeRoles.Where(r => roleNamesByUser.Contains(r.aspnet_Roles.RoleName)).Count() > 0 : true) == true
                        select new cSiteMapNode
                        {
                            SiteMapNodeId = (int)s.SiteMapNodeId,
                            SiteMapNodeSubId = (int?)s.SiteMapNodeSubId,
                            SiteMapNodePath = (string)s.SiteMapNodePath,
                            SiteMapNodeRewrittedPath = String.IsNullOrEmpty(sd.SiteMapNodeRewriteUrl) ? "" : (string)"~/" + sd.SiteMapNodeRewriteUrl,
                            SiteMapNodeAddedUserId = (Guid)s.SiteMapNodeAddedUserId,
                            SiteMapNodeEditUserId = (Guid)s.SiteMapNodeEditUserId,
                            SiteMapNodeShow = (bool)s.SiteMapNodeShow,
                            SiteMapNodeAddedDate = (DateTime)s.SiteMapNodeAddedDate,
                            SiteMapNodeEditDate = (DateTime)s.SiteMapNodeEditDate,
                            SiteMapNodeTitle = (string)sd.SiteMapNodeTitle,
                            LangId = (int)sd.LangId,
                            SiteMapNodeRoles = s.SW_SiteMapNodeRoles.Where(r => r.SiteMapNodeId == s.SiteMapNodeId).ToList
                                           <SW_SiteMapNodeRole>()
                        }).ToList<cSiteMapNode>();
            
        } 
        #endregion


        #region deleteSiteMapNodes
        private List<int> _siteMapNodeIds;

        /// <summary>
        /// Add siteMapNodeId to List for Delete Paths
        /// </summary>
        /// <param name="siteMapNodeId">Insert SiteMapId</param>
        protected void AddSiteMapNodeId(int siteMapNodeId) {

            _siteMapNodeIds.Add(siteMapNodeId);

        }


        /// <summary>
        /// Remove siteMapNodeId from List
        /// </summary>
        /// <param name="siteMapNodeId"></param>
        protected void RemoveSiteMapNodeId(int siteMapNodeId) {

            _siteMapNodeIds.Remove(siteMapNodeId);

        }


        /// <summary>
        /// Clear List of SiteMapNodeIds
        /// </summary>
        protected void ClearSiteMapNodeIds() {

            _siteMapNodeIds.Clear();

        }



        /// <summary>
        /// Delete paths in SiteMapNodes
        /// </summary>
        protected void DeleteSiteMapNodes() {

            // using statement free the memory after metode is done
            using (Searchwar_netEntities db = new Searchwar_netEntities()) {

                // Check RemoveRoleIds for empty
                if (_siteMapNodeIds.Count() > 0) {

                    IQueryable<SW_SiteMapNode> deleteSiteMapNodes = (from s in db.SW_SiteMapNode
                                                                     where _siteMapNodeIds.Contains(s.SiteMapNodeId)
                                                                     select s).AsQueryable<SW_SiteMapNode>();

                    IQueryable<SW_SiteMapNodeData> deleteSiteMapNodesData = (from sd in db.SW_SiteMapNodeData
                                                                             where _siteMapNodeIds.Contains(sd.SiteMapNodeId)
                                                                             select sd).AsQueryable<SW_SiteMapNodeData>();

                    IQueryable<SW_SiteMapNodeRole> DeleteSiteMapNodesRoles = (from r in db.SW_SiteMapNodeRoles
                                                                              where _siteMapNodeIds.Contains(r.SiteMapNodeId)
                                                                              select r).AsQueryable<SW_SiteMapNodeRole>();

                    IQueryable<SW_SiteMapNodeMetaTag> deleteSiteMapNodesMetaTags = (from m in db.SW_SiteMapNodeMetaTags
                                                                                    where _siteMapNodeIds.Contains(m.MetaTagsId)
                                                                                    select m).AsQueryable<SW_SiteMapNodeMetaTag>();

                    IQueryable<SW_SiteMapNodeMetaTagsData> deleteSiteMapNodesMetaTagsData = (from md in db.SW_SiteMapNodeMetaTagsData
                                                                                             where _siteMapNodeIds.Contains(md.MetaTagsId)
                                                                                             select md).AsQueryable<SW_SiteMapNodeMetaTagsData>();

                    // Delete all SiteMapNodesMetaTagsData
                    foreach (var item in deleteSiteMapNodesMetaTagsData)
                    {
                        db.SW_SiteMapNodeMetaTagsData.DeleteObject(item);
                    }

                    // Delete all SiteMapNodesMetaTags
                    foreach (var item in deleteSiteMapNodesMetaTags)
                    {
                        db.SW_SiteMapNodeMetaTags.DeleteObject(item);
                    }

                    // Delete all SiteMapNodesRoles
                    foreach (var item in DeleteSiteMapNodesRoles)
                    {
                        db.SW_SiteMapNodeRoles.DeleteObject(item);
                    }

                    // Delete all SiteMapNodesData
                    foreach (var item in deleteSiteMapNodesData)
                    {
                        db.SW_SiteMapNodeData.DeleteObject(item);
                    }

                    // Delete all SiteMapNodes
                    foreach (var item in deleteSiteMapNodes)
                    {
                        db.SW_SiteMapNode.DeleteObject(item);
                    }

                    db.SaveChanges();

                }

            }

        } 
        #endregion


        /// <summary>
        /// Update a path in cSiteMapNode
        /// (Insert RoleIds with AddRoleId and Remove RoleIds with RemoveRoleId)
        /// </summary>
        /// <param name="siteMapNodeId">Insert SiteMapId</param>
        /// <param name="path">Insert URL (Ex. ~/default.aspx)</param>
        /// <param name="userId">Insert UserId</param>
        /// <param name="showSiteMapNode">Show URL in menu or not? (true)Show - (false)Dont show in menu</param>
        /// <param name="siteMapNodeSubId">If you want this URL like a sub for a URL, then insert ID of the URL</param>
        /// <param name="title">Inser title of URL</param>
        /// <param name="langaugeId">Insert ID of that langauge it write in</param>
        /// <param name="newLangId">Insert newLangId</param>
        protected void UpdateSiteMap(int siteMapNodeId,
            int langaugeId,
            string path,
            Guid userId,
            bool showSiteMapNode,
            int? siteMapNodeSubId,
            string title,
            int? newLangId,
            int sort,
            SiteMapNodeMetatagsObject m)
        {
            DateTime dateTimeNow = TimeZoneManager.DateTimeNow;

            // using statement free the memory after metode is done
            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {
                SW_SiteMapNode siteMapNode = (from s in db.SW_SiteMapNode
                                              where s.SiteMapNodeId.Equals(siteMapNodeId)
                                              select s).SingleOrDefault();

                // Check SiteMapId in database
                if (siteMapNode != null)
                {

                    siteMapNode.SiteMapNodePath = path;
                    siteMapNode.SiteMapNodeShow = showSiteMapNode;
                    siteMapNode.SiteMapNodeSubId = siteMapNodeSubId;
                    siteMapNode.SiteMapNodeEditUserId = userId;
                    siteMapNode.SiteMapNodeEditDate = dateTimeNow;
                    siteMapNode.SiteMapNodeSort = sort;

                    SW_SiteMapNodeData siteMapNodeData = siteMapNode.SW_SiteMapNodeData.Single<SW_SiteMapNodeData>(d => d.SiteMapNodeId.Equals(siteMapNodeId) && d.LangId.Equals(langaugeId));
                    siteMapNodeData.SiteMapNodeTitle = title;
                    if (newLangId.HasValue)
                    {
                        siteMapNodeData.LangId = newLangId.Value;
                    }

                    db.SaveChanges();

                    object checkMetaTags =
                        db.SW_SiteMapNodeMetaTags.Where(mt => mt.MetaTagsId == siteMapNodeId).SingleOrDefault();

                    if (m != null && checkMetaTags != null)
                    {
                        
                        new SearchWar.SiteMap.MetaTags.SiteMapNodeMetaTags().UpdateMetaTags(siteMapNodeId,
                            langaugeId, 
                            m.MetaTagTitle, 
                            m.MetaTagDescription, 
                            m.MetaTagKeywords, 
                            m.MetaTagLanguage, 
                            m.MetaTagAuthor, 
                            m.MetaTagPublisher, 
                            m.MetaTagCopyright, 
                            m.MetaTagRevisitAfter, 
                            m.MetaTagRobots, 
                            m.MetaTagCache, 
                            m.MetaTagCacheControl, 
                            null, 
                            userId);

                    }


                }
                else
                {

                    throw (new ArgumentNullException("siteMapNodeId", "Cant find SiteMap '" + siteMapNodeId + "' in database"));

                }
            }
        }


        /// <summary>
        /// Insert a path in cSiteMapNode (Insert RoleIds with AddRoleId)
        /// </summary>
        /// <param name="path">Insert URL (Ex. ~/default.aspx)</param>
        /// <param name="userId">Insert UserId</param>
        /// <param name="showSiteMapNode">Show URL in menu or not? (true)Show - (false)Dont show in menu</param>
        /// <param name="siteMapNodeSubId">If you want this URL like a sub for a URL, then insert ID of the URL</param>
        /// <param name="title">Inser title of URL</param>
        /// <param name="langaugeId">Insert ID of that langauge it write in</param>
        /// <param name="metatagsObject">to add metatags to sitemap (just left it null if you dont want metatags with)</param>
        protected void CreateSiteMapNode_Single(string path,
            Guid userId,
            bool showSiteMapNode,
            int? siteMapNodeSubId,
            string title, 
            int langaugeId, 
            int sort, 
            SiteMapNodeMetatagsObject m)
        {
            DateTime dateTimeNow = TimeZoneManager.DateTimeNow;

            // using statement free the memory after metode is done
            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {

                SW_SiteMapNode createSiteMapNode = new SW_SiteMapNode
                                                       {
                                                           SiteMapNodePath = path,
                                                           SiteMapNodeAddedUserId = userId,
                                                           SiteMapNodeEditUserId = userId,
                                                           SiteMapNodeShow = showSiteMapNode,
                                                           SiteMapNodeEditDate = dateTimeNow,
                                                           SiteMapNodeAddedDate = dateTimeNow,
                                                           SiteMapNodeSort = sort
                                                       };

                SW_SiteMapNodeData createSiteMapNodeData = new SW_SiteMapNodeData
                                                               {
                                                                   LangId = langaugeId
                                                               };


                // Check title for empty
                if (!string.IsNullOrEmpty(title))
                {
                    createSiteMapNodeData.SiteMapNodeTitle = title;
                }


                // Check SubId for empty
                if (siteMapNodeSubId.HasValue)
                {
                    SW_SiteMapNode siteMapNodeSub = (from s in db.SW_SiteMapNode
                                                     where s.SiteMapNodeId.Equals(siteMapNodeSubId)
                                                     select s).SingleOrDefault();

                    // Check path is in database
                    if (siteMapNodeSub != null)
                    {

                        createSiteMapNode.SiteMapNodeSubId = siteMapNodeSubId;

                    }
                    else
                    {

                        throw (new ArgumentNullException("siteMapNodeSubId", "Cant find SiteMapId '" + siteMapNodeSubId + "' in database"));

                    }
                }

                db.SW_SiteMapNode.AddObject(createSiteMapNode);
                db.SaveChanges();

                // Insert SiteMapId
                createSiteMapNodeData.SiteMapNodeId = createSiteMapNode.SiteMapNodeId;

                db.SW_SiteMapNodeData.AddObject(createSiteMapNodeData);
                db.SaveChanges();


                if (m != null)
                {
                    
                    new SearchWar.SiteMap.MetaTags.SiteMapNodeMetaTags().CreateMetaTags(createSiteMapNode.SiteMapNodeId, 
                        m.MetaTagTitle, 
                        m.MetaTagDescription, 
                        m.MetaTagKeywords, 
                        m.MetaTagLanguage, 
                        m.MetaTagAuthor, 
                        m.MetaTagPublisher, 
                        m.MetaTagCopyright, 
                        m.MetaTagRevisitAfter, 
                        m.MetaTagRobots, 
                        m.MetaTagCache,
                        m.MetaTagCacheControl, 
                        userId, 
                        langaugeId);

                }
            }


        }
    }
}