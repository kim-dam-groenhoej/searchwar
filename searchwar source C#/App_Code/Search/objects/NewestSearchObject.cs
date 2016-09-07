using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Searchwar_netModel;

/// <summary>
/// Summary description for NewestSearchObject
/// </summary>
namespace SearchWar.SearchEngine {

    [Serializable]
    public class NewestSearchObject {
        public NewestSearchObject() {
            //
            // TODO: Add constructor logic here
            //
        }


        private Guid _SearchWarId;
        private string _ClanName = null;
        private SW_SearchWarContinentData _ClanContinentData;
        private SW_SearchWarCountryData _ClanCountryData;
        private SW_SearchWarGame _SearchGame;
        private SW_SearchWarGameType _SearchGameType = null;
        private int _SearchXvs;
        private int _SearchvsX;
        private DateTime _SearchMatchStart;

        public Guid SearchWarID {
            get { return _SearchWarId; }
            set { _SearchWarId = value; }
        }

        public string ClanName {
            get { return _ClanName; }
            set { _ClanName = value; }
        }

        public SW_SearchWarContinentData ClanContinentData {
            get { return _ClanContinentData; }
            set {
                _ClanContinentData = value;
            }
        }

        public SW_SearchWarCountryData ClanCountryData {
            get { return _ClanCountryData; }
            set {
                _ClanCountryData = value;
            }
        }

        public SW_SearchWarGame SearchGame {
            get { return _SearchGame; }
            set {
                _SearchGame = value;
            }
        }

        public SW_SearchWarGameType SearchGameType {
            get { return _SearchGameType; }
            set {
                _SearchGameType = value;
            }
        }

        public int SearchXvs {
            get { return _SearchXvs; }
            set { _SearchXvs = value; }
        }

        public int SearchvsX {
            get { return _SearchvsX; }
            set { _SearchvsX = value; }
        }

        public DateTime SearchMatchStart {
            get { return _SearchMatchStart; }
            set { _SearchMatchStart = value; }
        }

    }
}