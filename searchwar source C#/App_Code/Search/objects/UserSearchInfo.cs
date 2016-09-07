using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// object with all user search information
/// </summary>
namespace SearchWar.SearchEngine {
    public class UserSearchInfo {
        public UserSearchInfo() {

        }

        // clan info
        private string _clanName;
        private int? _clanSkillID;
        private int _clanContinentID;
        private int _clanCountryID;
        // search info
        private int _searchGameID;
        private int? _searchGameModeID;
        private string _searchMap;
        private int? _searchSkillID;
        private int _searchContinentID;
        private int? _searchCountryID;
        private int _searchXvs;
        private int _searchvsX;
        private DateTime _searchMatchStart;
        // user info
        private string _userIpAddress;
        private Guid _userID;
        private Guid? _searchMatchID;


        public string UserIpAddress {
            get { return _userIpAddress; }
            set { _userIpAddress = value; }
        }

        public Guid UserID {
            get { return _userID; }
            set { _userID = value; }
        }

        public Guid? SearchMatchID {
            get { return _searchMatchID; }
            set { _searchMatchID = value; }
        }

        public string ClanName {
            get { return _clanName; }
            set { _clanName = value; }
        }

        public int? ClanSkillID {
            get { return _clanSkillID; }
            set { _clanSkillID = value; }
        }

        public int ClanContinentID {
            get { return _clanContinentID; }
            set { _clanContinentID = value; }
        }

        public int ClanCountryID {
            get { return _clanCountryID; }
            set { _clanCountryID = value; }
        }

        public int SearchGameID {
            get { return _searchGameID; }
            set { _searchGameID = value; }
        }

        public int? SearchGameModeID {
            get { return _searchGameModeID; }
            set { _searchGameModeID = value; }
        }

        public string SearchMap {
            get { return _searchMap; }
            set { _searchMap = value; }
        }

        public int? SearchSkillID {
            get { return _searchSkillID; }
            set { _searchSkillID = value; }
        }

        public int SearchContinentID {
            get { return _searchContinentID; }
            set { _searchContinentID = value; }
        }

        public int? SearchCountryID {
            get { return _searchCountryID; }
            set { _searchCountryID = value; }
        }

        public int SearchXvs {
            get { return _searchXvs; }
            set { _searchXvs = value; }
        }

        public int SearchvsX {
            get { return _searchvsX; }
            set { _searchvsX = value; }
        }

        public DateTime SearchMatchStart
        {
            get { return _searchMatchStart; }
            set { _searchMatchStart = value; }
        }

    }
}