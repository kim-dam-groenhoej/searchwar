using System;
using System.Collections.Generic;

/// <summary>
/// Subclass til CurlSiteMapNode
/// </summary>
namespace SearchWar.SiteMap
{
    public class CustomSiteMapNode : CustomSiteMapSystem
    {

        public CustomSiteMapNode()
        {

            
            
        }

        #region deleteSiteMapNodes
        /// <summary>
        /// Delete paths in SiteMapNodes
        /// </summary>
        public void DeleteAllSiteMapNodes() {

            DeleteSiteMapNodes();

        }


        /// <summary>
        /// Add siteMapNodeId to List for Delete Paths
        /// </summary>
        /// <param name="siteMapNodeId">Insert SiteMapId</param>
        public new void AddSiteMapNodeId_Delete(int siteMapNodeId) {

            AddSiteMapNodeId(siteMapNodeId);

        }


        /// <summary>
        /// Remove siteMapNodeId from List
        /// </summary>
        /// <param name="siteMapNodeId"></param>
        public new void RemoveSiteMapNodeId_Delete(int siteMapNodeId) {

            RemoveSiteMapNodeId(siteMapNodeId);

        }


        /// <summary>
        /// Clear List of SiteMapNodeIds
        /// </summary>
        public new void ClearSiteMapNodeIds_Delete() {

            ClearSiteMapNodeIds();

        } 
        #endregion

        #region createSiteMapNode
        /// <summary>
        /// Insert a path in cSiteMapNode
        /// </summary>
        /// <param name="path">Insert URL (Ex. ~/default.aspx)</param>
        /// <param name="title">Inser title of URL</param>
        /// <param name="langaugeId">Insert ID of that langauge it write in</param>
        /// <param name="userId">Insert UserId</param>
        /// <param name="showSiteMapNode">Show URL in menu or not? (true)Show - (false)Dont show in menu</param>
        /// <param name="sort"></param>
        public void CreateSiteMapNode(string path,
            string title,
            int langaugeId,
            Guid userId,
            bool showSiteMapNode, 
            int sort,
            SiteMapNodeMetatagsObject m) {

            CreateSiteMapNode_Single(path,
                userId,
                showSiteMapNode,
                null,
                title,
                langaugeId,
                sort, 
                m);

        }

        /// <summary>
        /// Insert a path in cSiteMapNode (Insert RoleIds with AddRoleId)
        /// </summary>
        /// <param name="path">Insert URL (Ex. ~/default.aspx)</param>
        /// <param name="title">Inser title of URL</param>
        /// <param name="langaugeId">Insert ID of that langauge it write in</param>
        /// <param name="userId">Insert UserId</param>
        /// <param name="showSiteMapNode">Show URL in menu or not? (true)Show - (false)Dont show in menu</param>
        /// <param name="siteMapNodeSubId">If you want this URL like a sub for a URL, then insert ID of the URL</param>
        /// <param name="sort"></param>
        public void CreateSiteMapNode(string path,
            string title,
            Guid userId,
            int langaugeId,
            bool showSiteMapNode,
            int? siteMapNodeSubId, 
            int sort, 
            SiteMapNodeMetatagsObject m) {

            CreateSiteMapNode_Single(path,
                userId,
                showSiteMapNode,
                siteMapNodeSubId,
                title,
                langaugeId, 
                sort, 
                m);

        } 
        #endregion

        #region updateSiteMapNode
        /// <summary>
        /// Update a path in cSiteMapNode
        /// (Insert RoleIds with AddRoleId)
        /// (Remove RoleIds with RemoveRoleId)
        /// </summary>
        /// <param name="siteMapNodeId">Insert SiteMapId</param>
        /// <param name="path">Insert URL (Ex. ~/default.aspx)</param>
        /// <param name="title">Inser title of URL</param>
        /// <param name="langaugeId">Insert ID of that langauge it write in</param>
        /// <param name="userId">Insert UserId</param>
        /// <param name="showSiteMapNode">Insert showSiteMapNode</param>
        /// <param name="newLangId">Insert newLangId</param>
        /// <param name="sort"></param>
        public void UpdateSiteMapNode(int siteMapNodeId,
            string path,
            string title,
            int langaugeId,
            Guid userId,
            bool showSiteMapNode,
            int? newLangId, 
            int sort, 
            SiteMapNodeMetatagsObject m) {

            UpdateSiteMap(siteMapNodeId,
                langaugeId,
                path,
                userId,
                showSiteMapNode,
                null,
                title,
                newLangId, 
                sort, 
                m);

        }

        /// <summary>
        /// Update a path in cSiteMapNode
        /// (Insert RoleIds with AddRoleId and Remove RoleIds with RemoveRoleId)
        /// </summary>
        /// <param name="siteMapNodeId">Insert SiteMapId</param>
        /// <param name="path">Insert URL (Ex. ~/default.aspx)</param>
        /// <param name="title">Inser title of URL</param>
        /// <param name="langaugeId">Insert ID of that langauge it write in</param>
        /// <param name="userId">Insert UserId</param>
        /// <param name="showSiteMapNode">Show URL in menu or not? (true)Show - (false)Dont show in menu</param>
        /// <param name="siteMapNodeSubId">If you want this URL like a sub for a URL, then insert ID of the URL</param>
        /// <param name="newLangId">Insert newLangId</param>
        /// <param name="sort"></param>
        public void UpdateSiteMapNode(int siteMapNodeId,
            string path,
            string title,
            int langaugeId,
            Guid userId,
            bool showSiteMapNode,
            int? siteMapNodeSubId,
            int? newLangId, 
            int sort, 
            SiteMapNodeMetatagsObject m) {

            UpdateSiteMap(siteMapNodeId,
                langaugeId,
                path,
                userId,
                showSiteMapNode,
                siteMapNodeSubId,
                title,
                newLangId, 
                sort, 
                m);

        } 
        #endregion

        #region getSiteMapNode
        /// <summary>
        /// Get all MainPaths (MainPaths is paths without a SiteMapSubId)
        /// </summary>
        /// <param name="langId">Insert langId</param>
        /// <param name="showall">(if false: Get all nodes where siteMapNodeShow is false) (if true: you got all nodes)</param>
        /// <returns>Return a list of anonymous objects</returns>
        public List<cSiteMapNode> GetSiteMapNodes(int langId, 
            bool showall) {

            return GetMainSiteMapNodes(langId, showall);

        }

        /// <summary>
        /// Get all SubPaths
        /// </summary>
        /// <param name="langId">Insert langId</param>
        /// <param name="siteMapNodeId">Insert SiteMapId</param>
        /// <returns>Return a list of anonymous objects</returns>
        public List<cSiteMapNode> GetSiteMapNodes(int langId, int siteMapNodeId)
        {

            return GetSubSiteMapNodes(langId,
                siteMapNodeId);

        }

        /// <summary>
        /// Get Currentpath
        /// </summary>
        /// <param name="langId">Insert LangId</param>
        /// <returns>Return a anonymous object</returns>
        public cSiteMapNode GetCurrentSiteMapNode(int langId)
        {

            return SearchSiteMapNode(null,
                langId);

        }


        /// <summary>
        /// Get parent path
        /// </summary>
        /// <param name="path">Insert path</param>
        /// <returns>Return anonymous object</returns>
        public cSiteMapNode GetParentSiteMapNode(string path)
        {

            return SearchParentSiteMapNode(path);

        }


        public cSiteMapNode GetSiteMapNode(int siteMapNodeId,
            int langId)
        {

            return GetSiteMapNode_Single(siteMapNodeId, langId);

        }
        #endregion

    }
}