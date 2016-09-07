using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Searchwar_netModel;

/// <summary>
/// Summary description for GameModeSystem
/// </summary>
namespace SearchWar.SearchEngine.Games {
    public class GameModeSystem : GamesSystem {
        public GameModeSystem() {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Get GameType by GameName
        /// </summary>
        /// <param name="gameTypeName">Insert GameName</param>
        /// <returns>SW_SearchWarGameTypes object</returns>
        public SW_SearchWarGameType GetGameType(string gameTypeName) {


            gameTypeName = HttpContext.Current.Server.HtmlDecode(gameTypeName);

            Searchwar_netEntities db = new Searchwar_netEntities();

                var r = (from T in db.SW_SearchWarGameType
                         where T.SearchWarGameTypeName == gameTypeName
                         select T).SingleOrDefault<SW_SearchWarGameType>();




                return r;

        }

        /// <summary>
        /// Get all GameTypes by GameId
        /// </summary>
        /// <param name="GameId">Insert GameId</param>
        /// <returns>List of SW_SearchWarGameTypes objects</returns>
        public List<SW_SearchWarGameType> GetGameTypes(int GameId) {

            Searchwar_netEntities db = new Searchwar_netEntities();

                var r = (from T in db.SW_SearchWarGameType
                         where T.SearchWarGameId == GameId
                         orderby T.SearchWarGameTypeName
                         select T).ToList<SW_SearchWarGameType>();




                return r;

        }

        public List<dynamic> GetGameTypeIds()
        {

            Searchwar_netEntities db = new Searchwar_netEntities();

            var r = (from T in db.SW_SearchWarGameType
                     select new
                                {
                                    gameTypeId = T.SearchWarGameTypeId
                                }).ConvertListAnoToExpa();

            return r;

        }
    }
}