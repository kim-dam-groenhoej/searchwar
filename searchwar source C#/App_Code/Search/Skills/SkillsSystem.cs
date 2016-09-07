using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Searchwar_netModel;
using System.Reflection;

/// <summary>
/// Summary description for SkillsSystem
/// </summary>
namespace SearchWar.SearchEngine.Skills {
    public class SkillsSystem {
        public SkillsSystem() {
            //
            // TODO: Add constructor logic here
            //
        }

        public List<dynamic> GetSkills(int langId) {

            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {

                var r = (from s in db.SW_SearchWarSkill
                         join sd in db.SW_SearchWarSkillData on s.SearchWarSkillId equals sd.SearchWarSkillId
                         where sd.LangId == langId
                         orderby s.SearchWarSkillSort
                         select new
                         {
                             sd.SearchWarSkillId,
                             sd.SearchWarSkillName,
                             s.SearchWarSkillAddedDatetime,
                             s.SearchWarSkillAddedUserId,
                             s.SearchWarSkillEditDatetime,
                             s.SearchWarSkillEditUserId,
                             s.SearchWarSkillSort
                         });

                return r.AsEnumerable().ConvertListAnoToExpa();
            }
        }

        public dynamic GetSkill(string skillName,
            int langId) {

            skillName = HttpContext.Current.Server.HtmlDecode(skillName);

            using (Searchwar_netEntities db = new Searchwar_netEntities())
            {

                var r = (from s in db.SW_SearchWarSkill
                         join sd in db.SW_SearchWarSkillData on s.SearchWarSkillId equals sd.SearchWarSkillId
                         where sd.LangId == langId && sd.SearchWarSkillName == skillName
                         select new
                                    {
                                        sd.SearchWarSkillId,
                                        sd.SearchWarSkillName,
                                        s.SearchWarSkillAddedDatetime,
                                        s.SearchWarSkillAddedUserId,
                                        s.SearchWarSkillEditDatetime,
                                        s.SearchWarSkillEditUserId,
                                        s.SearchWarSkillSort
                                    }).SingleOrDefault();

                return r.ConvertAnoToExpa();
            }

        }
    }
}