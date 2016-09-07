using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Searchwar_netModel;

/// <summary>
/// Summary description for GamesSystem
/// </summary>
namespace SearchWar.SearchEngine.Games {
    public class GamesSystem {
        public GamesSystem() {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Get all games
        /// </summary>
        /// <returns>List of SW_SearchWarGame objects</returns>
        public List<SW_SearchWarGame> GetGames() {

            Searchwar_netEntities db = new Searchwar_netEntities();

            var r = (from G in db.SW_SearchWarGame
                     select G).ToList<SW_SearchWarGame>();

                return r;

        }

        /// <summary>
        /// Get game
        /// </summary>
        /// <returns>SW_SearchWarGame object</returns>
        public SW_SearchWarGame GetGame(string gameName) {


            gameName = HttpContext.Current.Server.HtmlDecode(gameName);

            Searchwar_netEntities db = new Searchwar_netEntities();

                var r = (from G in db.SW_SearchWarGame
                         where G.SearchWarGameName == gameName
                         select G).SingleOrDefault<SW_SearchWarGame>();




                return r;

        }
    }
}