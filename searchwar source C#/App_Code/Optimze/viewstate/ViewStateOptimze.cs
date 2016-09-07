using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

/// <summary>
/// Summary description for ViewStateOptimze
/// </summary>
namespace SearchWar.ViewStateOptimze {

    public class ViewStateOptimze {
        public ViewStateOptimze() {
            //
            // TODO: Add constructor logic here
            //
        }


        public void WriteViewStateInFile(string userId,
            string viewState) {

            StringWriter writer = new StringWriter();
            writer.Write(viewState);
            // save the string to disk
            StreamWriter sw = File.CreateText(HttpContext.Current.Request.MapPath("~/userViewState/" + userId + ".vs"));
            sw.Write(writer.ToString());
            sw.Close();
            writer.Close();
            

        }

        public string LoadViewStateFromFile(string userId) {

            string file = HttpContext.Current.Request.MapPath("~/userViewState/" + userId + ".vs");

            // determine the file to access
            if (!File.Exists(file))
            {
                return null;
            }
            else {
                // open the file
                StreamReader sr = File.OpenText(file);
                string viewStateString = sr.ReadToEnd();
                sr.Close();
                // deserialize the string

                return viewStateString;
                
            }

        }
    }
}