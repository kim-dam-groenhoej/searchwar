using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Threading;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using SearchWar.ObjectHelper;
using System.Globalization;
using SearchWar.SearchEngine;
using SearchWar.SearchEngine.Validate;
using System.Threading;
using SearchWar.LangSystem;
using SearchWar.ChatSystem;

/// <summary>
/// Summary description for XmlResponseManager
/// </summary>
public class XmlResponseManager : IHttpHandler, IRequiresSessionState
{
	public XmlResponseManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

    public void ProcessRequest(HttpContext context)
    {

        context.Response.ClearHeaders();
        context.Response.ContentType = "text/xml";
        context.Response.ContentEncoding = Encoding.UTF8;
        context.Response.StatusCode = 202;
        context.Response.SubStatusCode = 202;
        context.Response.Status = "202 continue";
        context.Response.StatusDescription = "continue";

        //try
        //{

        string action = context.Request.QueryString["action"];
        string mngOption = context.Request.QueryString["option"];
        string outputData = "";
        SearchEngineMng engineMng = null;
        string currentUserIp = "62.107.21.129";

        if (action == "1")
        {
            //Guid newId = new Guid(context.Request.QueryString["searchmathid"]);
            //Guid userid = new Guid(context.Request.QueryString["userid"]);

            //ANOProfile.SaveCookies(currentUserIp, newId, userid);
            

            context.Session["chatmessages"] = null;
            context.Session["searchresult"] = null;
        }

        if (!string.IsNullOrEmpty(mngOption))
        {
            for (int i = 0; i < 2 * 60; i++)
            {
                if (context.Response.IsClientConnected)
                {
                    outputData = null;

                    XDocument createMngXml = new XDocument(new XDeclaration("1.0", "iso-8859-1", "true"),
                                        new XElement("manager"));

                    XElement root = createMngXml.Descendants("manager").Single();
                    XElement insertsearchX = null;
                    ManagerResponseObj chatobjresponse = null;
                    ManagerResponseObj searchobjresponse = null;
                

                    if (mngOption.ToLower().Contains("chat"))
                    {

                        ChatSystem cs = new ChatSystem(context, currentUserIp);
                        chatobjresponse = cs.ChatMessages();

                        if (chatobjresponse.DataObject != null)
                        {
                            List<Guid> chatMsgIds = new List<Guid>();
                            if (context.Session["chatmessages"] != null)
                            {
                                chatMsgIds = (List<Guid>)context.Session["chatmessages"];
                            }
                            foreach (Searchwar_netModel.SW_ChatMessage item in ((List<Searchwar_netModel.SW_ChatMessage>)chatobjresponse.DataObject))
                            {
                                chatMsgIds.Add(item.ChatMsgId);
                            }
                            context.Session["chatmessages"] = chatMsgIds;
                        }

                        root.Add(chatobjresponse.Xml);

                    }

                    if (mngOption.ToLower().Contains("search"))
                    {
                        engineMng = new SearchEngineMng(currentUserIp, context, action);

                        // insert search data to server
                        searchobjresponse = engineMng.SearchNow();
                        insertsearchX = searchobjresponse.Xml;

                        if (searchobjresponse.DataObject != null)
                        {
                            List<Guid> searchIds = new List<Guid>();
                            if (context.Session["searchresult"] != null)
                            {
                                searchIds = (List<Guid>)context.Session["searchresult"];
                            }
                            foreach (SearchObject item in ((List<SearchObject>)searchobjresponse.DataObject))
                            {
                                searchIds.Add(item.SearchWarID);
                            }
                            context.Session["searchresult"] = searchIds;
                        }


                        if (insertsearchX == null)
                        {
                            outputData = "";
                        }
                        else
                        {
                            if (searchobjresponse.DataObject != null && chatobjresponse.DataObject != null)
                            {
                                if (((List<SearchObject>)searchobjresponse.DataObject).Count() > 0 || ((List<Searchwar_netModel.SW_ChatMessage>)chatobjresponse.DataObject).Count() > 0 || engineMng.isValid == false)
                                {

                                    root.Add(insertsearchX);

                                    // Write/save data
                                    StringWriter sw = new StringWriter();
                                    XmlWriterSettings xws = new XmlWriterSettings();
                                    xws.CheckCharacters = false;
                                    xws.ConformanceLevel = ConformanceLevel.Auto;
                                    xws.Indent = false;
                                    xws.OmitXmlDeclaration = false;

                                    XmlWriter w = XmlWriter.Create(sw, xws);
                                    createMngXml.Save(w);

                                    w.Close();

                                    outputData = sw.ToString();

                                    sw.Close();
                                }
                            }
                        }
                    }


                    context.Response.Write(outputData);
                    context.Response.Write("    ");
                    context.Response.Flush();


                    if (engineMng.isValid == false || !string.IsNullOrEmpty(outputData))
                    {
                        engineMng.Close();

                        context.Response.End();
                        context.Response.Close();
                        return;
                    }

                    // wait (performance)
                    Thread.Sleep(5000);
                }
            }
        }

        engineMng.Close();

        //}
        //catch (Exception ex)
        //{

        //    context.Response.ClearContent();
        //    context.Response.Write("<?xml version='1.0'?><manager><error>" + ex.Source + ": " + ex.Message + "</error></manager>");
        //    context.Response.Write("    ");
        //    context.Response.Flush();
        //    // do nothing
        //}

        context.Response.End();
        context.Response.Close();
        return;
    }




}