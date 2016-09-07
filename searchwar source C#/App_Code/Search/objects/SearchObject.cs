using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Searchwar_netModel;

/// <summary>
/// Summary description for SearchObject
/// </summary>
namespace SearchWar.SearchEngine {
    public class SearchObject {
        public SearchObject() {
            //
            // TODO: Add constructor logic here
            // 

        }

        private string _clanName;
        private SW_SearchWarContinentData _clanContinentData;
        private SW_SearchWarCountryData _clanCountryData;
        private SW_SearchWarSkillData _clanSkillData;
        private SW_SearchWarSkillData _searchSkillData;
        private SW_SearchWarGame _searchGame;
        private SW_SearchWarGameType _searchGameType;
        private string _searchMap;
        private int _searchXvs;
        private int _searchvsX;
        private SW_SearchWarContinentData _searchContinentData;
        private SW_SearchWarCountryData _searchCountryData;
        private DateTime _searchMatchStart;
        private DateTime _searchAddedDate;
        private DateTime _searchEditDate;
        private Guid _userID;
        private Guid _searchWarId;

        public string ClanName {
            get { return _clanName; }
            set { _clanName = value; }
        }

        public SW_SearchWarContinentData ClanContinentData {
            get { return _clanContinentData; }
            set { _clanContinentData = value; }
        }

        public SW_SearchWarCountryData ClanCountryData {
            get { return _clanCountryData; }
            set { _clanCountryData = value; }
        }

        public SW_SearchWarSkillData ClanSkillData {
            get { return _clanSkillData; }
            set { _clanSkillData = value; }
        }

        public SW_SearchWarSkillData SearchSkillData {
            get { return _searchSkillData; }
            set { _searchSkillData = value; }
        }

        public SW_SearchWarGame SearchGame {
            get { return _searchGame; }
            set { _searchGame = value; }
        }

        public SW_SearchWarGameType SearchGameType {
            get { return _searchGameType; }
            set { _searchGameType = value; }
        }

        public string SearchMap {
            get { return _searchMap; }
            set { _searchMap = value; }
        }

        public int SearchXvs {
            get { return _searchXvs; }
            set { _searchXvs = value; }
        }

        public int SearchvsX {
            get { return _searchvsX; }
            set { _searchvsX = value; }
        }

        public SW_SearchWarContinentData SearchContinentData {
            get { return _searchContinentData; }
            set { _searchContinentData = value; }
        }

        public SW_SearchWarCountryData SearchCountryData {
            get { return _searchCountryData; }
            set { _searchCountryData = value; }
        }

        public DateTime SearchMatchStart {
            get { return _searchMatchStart; }
            set { _searchMatchStart = value; }
        }

        public DateTime SearchAddedDate {
            get { return _searchAddedDate; }
            set { _searchAddedDate = value; }
        }

        public DateTime SearchEditDate {
            get { return _searchEditDate; }
            set { _searchEditDate = value; }
        }

        public Guid UserID {
            get { return _userID; }
            set { _userID = value; }
        }

        public Guid SearchWarID {
            get { return _searchWarId; }
            set { _searchWarId = value; }
        }
    }
}