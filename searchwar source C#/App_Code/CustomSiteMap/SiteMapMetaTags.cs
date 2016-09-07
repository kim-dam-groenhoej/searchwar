using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Searchwar_netModel;

/// <summary>
/// Summary description for SiteMapNodeMetaTags
/// </summary>

namespace SearchWar.SiteMap.MetaTags
{
    public class SiteMapNodeMetaTags
    {
        public SiteMapNodeMetaTags()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Get all metatags for cSiteMapNode
        /// </summary>
        /// <param name="siteMapNodeId">Insert SiteMapNodeId for Metatags</param>
        /// <param name="langId">Insert LangId</param>
        /// <returns></returns>
        public SiteMapNodeMetatagsObject GetMetaTags(int siteMapNodeId, int langId)
        {
            Searchwar_netEntities db = new Searchwar_netEntities();

                return (from m in db.SW_SiteMapNodeMetaTags
                                    join md in db.SW_SiteMapNodeMetaTagsData
                                    on m.MetaTagsId equals md.MetaTagsId
                                    where m.MetaTagsId == siteMapNodeId &&
                                    md.LangId == langId
                                    select new SiteMapNodeMetatagsObject
                                    {
                                        MetaId  = m.MetaTagsId,
                                        MetaTagAddedDate = m.MetaTagsAddedDate,
                                        MetaTagEditDate = m.MetaTagsEditDate,
                                        MetaTagAddedUserId = m.MetaTagsAddedUserId,
                                        MetaTagEditUserId = m.MetaTagsEditUserId,
                                        LangId = md.LangId,
                                        MetaTagTitle = string.IsNullOrEmpty(md.MetaTagsTitle) ? " " : md.MetaTagsTitle,
                                        MetaTagDescription = string.IsNullOrEmpty(md.MetaTagsDescription) ? " " : md.MetaTagsDescription,
                                        MetaTagKeywords = string.IsNullOrEmpty(md.MetaTagsKeywords) ? " " : md.MetaTagsKeywords,
                                        MetaTagLanguage = string.IsNullOrEmpty(md.MetaTagsLanguage) ? " " : md.MetaTagsLanguage,
                                        MetaTagAuthor = string.IsNullOrEmpty(md.MetaTagsAuthor) ? " " : md.MetaTagsAuthor,
                                        MetaTagPublisher = string.IsNullOrEmpty(md.MetaTagsPublisher) ? " " : md.MetaTagsPublisher,
                                        MetaTagCopyright = string.IsNullOrEmpty(md.MetaTagsCopyright) ? " " : md.MetaTagsCopyright,
                                        MetaTagRevisitAfter = string.IsNullOrEmpty(md.MetaTagsRevisitAfter) ? " " : md.MetaTagsRevisitAfter,
                                        MetaTagRobots = string.IsNullOrEmpty(md.MetaTagsRobots) ? " " : md.MetaTagsRobots,
                                        MetaTagCache = string.IsNullOrEmpty(md.MetaTagsCache) ? " " : md.MetaTagsCache,
                                        MetaTagCacheControl = string.IsNullOrEmpty(md.MetaTagsCacheControl) ? " " : md.MetaTagsCacheControl
                                    }).SingleOrDefault < SiteMapNodeMetatagsObject>();

        }


        /// <summary>
        /// Create MetaTags to a cSiteMapNode
        /// </summary>
        /// <param name="siteMapNodeId">Insert SiteMapNodeId</param>
        /// <param name="metaTagTitle">Insert Title</param>
        /// <param name="metaTagDescription">Insert Desc</param>
        /// <param name="metaTagKeywords">Insert keywords like "word, word, word"</param>
        /// <param name="metaTagLanguage">Insert lang like "da" or "en"</param>
        /// <param name="metaTagAuthor">Insert author</param>
        /// <param name="metaTagPublisher">Insert publisher</param>
        /// <param name="metaTagCopyright">Insert Copyright</param>
        /// <param name="metaTagRevisitAfter">Insert Revisitafter like "7 days"</param>
        /// <param name="metaTagRobots">Insert Robots like "index,follow"</param>
        /// <param name="metaTagCache">Insert cache like "no-cache"</param>
        /// <param name="metaTagCacheControl">Insert cachecontrol like "no-cache"</param>
        /// <param name="langaugeId">Insert ID of that langauge this metatags is write in</param>
        /// <param name="userId">Insert UserId</param>
        public void CreateMetaTags(int siteMapNodeId,
            string metaTagTitle,
            string metaTagDescription,
            string metaTagKeywords,
            string metaTagLanguage,
            string metaTagAuthor,
            string metaTagPublisher,
            string metaTagCopyright,
            string metaTagRevisitAfter,
            string metaTagRobots,
            string metaTagCache,
            string metaTagCacheControl,
            Guid userId,
            int langaugeId)
        {

            DateTime dateTimeNow = TimeZoneManager.DateTimeNow;


            // using statement free the memory after metode is done
            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {
                SW_SiteMapNode getCurrentSiteMapNode = (from s in db.SW_SiteMapNode
                                                        where s.SiteMapNodeId.Equals(siteMapNodeId)
                                                        select s).SingleOrDefault();
                
                // Check cSiteMapNode
                if (getCurrentSiteMapNode != null)
                {


                    SW_SiteMapNodeMetaTag createMeta = new SW_SiteMapNodeMetaTag
                                                           {
                                                               MetaTagsId = siteMapNodeId,
                                                               MetaTagsAddedUserId = userId,
                                                               MetaTagsEditUserId = userId,
                                                               MetaTagsAddedDate = dateTimeNow,
                                                               MetaTagsEditDate = dateTimeNow
                                                           };

                    SW_SiteMapNodeMetaTagsData createMetaData = new SW_SiteMapNodeMetaTagsData
                                                                    {
                                                                        MetaTagsTitle = metaTagTitle,
                                                                        MetaTagsDescription = metaTagDescription,
                                                                        MetaTagsKeywords = metaTagKeywords,
                                                                        MetaTagsLanguage = metaTagLanguage,
                                                                        MetaTagsAuthor = metaTagAuthor,
                                                                        MetaTagsPublisher = metaTagPublisher,
                                                                        MetaTagsCopyright = metaTagCopyright,
                                                                        MetaTagsRevisitAfter = metaTagRevisitAfter,
                                                                        MetaTagsRobots = metaTagRobots,
                                                                        MetaTagsCache = metaTagCache,
                                                                        MetaTagsCacheControl = metaTagCacheControl,
                                                                        LangId = langaugeId,
                                                                        MetaTagsId = siteMapNodeId
                                                                    };


                    db.SW_SiteMapNodeMetaTags.AddObject(createMeta);
                    db.SW_SiteMapNodeMetaTagsData.AddObject(createMetaData);
                    db.SaveChanges();

                }
                else
                {

                    throw (new ArgumentNullException("siteMapNodeId", "Cant find siteMapNodeId '" + siteMapNodeId + "' in database"));

                }
            }
        }

        /// <summary>
        /// Update MetaTags to a cSiteMapNode
        /// </summary>
        /// <param name="metaTagsId">Insert SiteMapNodeId</param>
        /// <param name="langId">Insert LangId</param>
        /// <param name="metaTagTitle">Insert Title</param>
        /// <param name="metaTagDescription">Insert Desc</param>
        /// <param name="metaTagKeywords">Insert keywords like "word, word, word"</param>
        /// <param name="metaTagLanguage">Insert lang like "da" or "en"</param>
        /// <param name="metaTagAuthor">Insert author</param>
        /// <param name="metaTagPublisher">Insert publisher</param>
        /// <param name="metaTagCopyright">Insert Copyright</param>
        /// <param name="metaTagRevisitAfter">Insert Revisitafter like "7 days"</param>
        /// <param name="metaTagRobots">Insert Robots like "index,follow"</param>
        /// <param name="metaTagCache">Insert cache like "no-cache"</param>
        /// <param name="metaTagCacheControl">Insert cachecontrol like "no-cache"</param>
        /// <param name="newLangId">Insert newLangId</param>
        /// <param name="userId">Insert UserId</param>
        public void UpdateMetaTags(int metaTagsId, 
            int langId,
            string metaTagTitle,
            string metaTagDescription,
            string metaTagKeywords,
            string metaTagLanguage,
            string metaTagAuthor,
            string metaTagPublisher,
            string metaTagCopyright,
            string metaTagRevisitAfter,
            string metaTagRobots,
            string metaTagCache,
            string metaTagCacheControl,
            int? newLangId,
            Guid userId)
        {

            DateTime dateTimeNow = TimeZoneManager.DateTimeNow;


            // using statement free the memory after metode is done
            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {


                SW_SiteMapNodeMetaTag updateMeta = db.SW_SiteMapNodeMetaTags.Single<SW_SiteMapNodeMetaTag>(m => m.MetaTagsId.Equals(metaTagsId));
                updateMeta.MetaTagsEditUserId = userId;
                updateMeta.MetaTagsEditDate = dateTimeNow;
                
                SW_SiteMapNodeMetaTagsData updateMetaData = updateMeta.SW_SiteMapNodeMetaTagsData.Single<SW_SiteMapNodeMetaTagsData>(d => d.MetaTagsId.Equals(metaTagsId) && d.LangId.Equals(langId));
                updateMetaData.MetaTagsTitle = metaTagTitle;
                updateMetaData.MetaTagsDescription = metaTagDescription;
                updateMetaData.MetaTagsKeywords = metaTagKeywords;
                updateMetaData.MetaTagsLanguage = metaTagLanguage;
                updateMetaData.MetaTagsAuthor = metaTagAuthor;
                updateMetaData.MetaTagsPublisher = metaTagPublisher;
                updateMetaData.MetaTagsCopyright = metaTagCopyright;
                updateMetaData.MetaTagsRevisitAfter = metaTagRevisitAfter;
                updateMetaData.MetaTagsRobots = metaTagRobots;
                updateMetaData.MetaTagsCache = metaTagCache;
                updateMetaData.MetaTagsCacheControl = metaTagCacheControl;
                if (newLangId.HasValue)
                {
                    updateMetaData.LangId = newLangId.Value;
                }


                db.SaveChanges();



            }

        }


    }
}