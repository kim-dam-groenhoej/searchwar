using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Xml;
using System.Xml.Linq;
using Searchwar_netModel;
using System.Web.Security;

/// <summary>
/// Summary description for XmlChatHandler
/// </summary>
namespace SearchWar.ChatSystem {
    public class XmlChatHandler : IHttpHandler, IRequiresSessionState {
        public XmlChatHandler() {
            //
            // TODO: Add constructor logic here
            //
        }



        public bool IsReusable {
            get {
                return true;
            }
        }


        private enum ChatCmds
        {
            SendMessage = 1
        }


        public void ProcessRequest(HttpContext context) {
            // Set defaults values
            string data = null;
            string currentUserIp = "62.107.21.129";
            HttpRequest currentR = context.Request;
            HttpServerUtility currentS = context.Server;
            int currentLangId;
            bool isValid = true;
            string errorMessage = "succes";
            ANO_User getUser = ANOProfile.GetCookieValues(currentUserIp, context);



            string userAction = context.Request.QueryString["a"];
            if (string.IsNullOrEmpty(userAction)) {
                isValid = false;
                errorMessage = "You forgot action (querystring a)";
            }


            // Create documet and root element called "c" for "chat"
            XDocument createXml = new XDocument(
                new XDeclaration("1.0", "iso-8859-1", "true"),
                new XElement("chat"));

            // Get root element "c"
            XElement getRootElement = createXml.Descendants("chat").Single();

            if (userAction == ChatCmds.SendMessage.ToString())
            {

                string userText = context.Request["t"];
                if (string.IsNullOrEmpty(userText)) {
                    isValid = false;
                    errorMessage = "You forgot the message (post/querystring t)";
                }

                string chatIdQuery = context.Request["c"];
                Guid? chatId = null;
                if (string.IsNullOrEmpty(chatIdQuery)) {
                    isValid = false;
                    errorMessage = "You forgot the chat window id (post/querystring c";
                }

                // Convert chatid querystring to Guid
                try
                {
                    chatId = new Guid(chatIdQuery);
                }
                catch
                {
                    isValid = false;
                    errorMessage = "Problem with converting chat window id (querystring c)";
                }

                if (isValid == true && chatId.HasValue)
                {
                    string getUsername = null;
                    if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated) {
                        getUsername = Membership.GetUser(getUser.UserID).UserName;
                    }

                    if (string.IsNullOrEmpty(getUsername)) {

                        Searchwar_netEntities db = new Searchwar_netEntities();
                        SW_SearchWar matchSearch = db.SW_SearchWar.SingleOrDefault(w => w.SearchWarId == getUser.SearchMatchID);

                        if (matchSearch != null)
                        {
                            getUsername = matchSearch.SearchWarClanName;
                        }

                    }

                    if (!string.IsNullOrEmpty(getUsername))
                    {
                        ChatSystem mngChat = new ChatSystem(context, currentUserIp);
                        mngChat.CreateMsg(userText, chatId, getUser.UserID, getUsername);
                    }
                    else
                    {
                        isValid = false;
                        errorMessage = "You muct be logged in or searching for a match";
                    }
                }

            }

            // write status for result
            getRootElement.Add(new XElement("status", new XAttribute("bool", isValid.ToString()), errorMessage));

            // Write/save data
            StringWriter sw = new StringWriter();
            XmlWriter xw = XmlWriter.Create(sw);
            createXml.Save(xw);

            xw.Close();

            data = sw.ToString();

            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(XmlWhiteSpaceModule.RemoveWhitespace(data));

        }
    }

}