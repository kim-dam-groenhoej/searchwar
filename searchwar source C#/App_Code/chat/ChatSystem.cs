using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Searchwar_netModel;
using System.Web.Security;

/// <summary>
/// Summary description for ChatSystem
/// </summary>
namespace SearchWar.ChatSystem
{
    public class ChatSystem
    {
        private TimeZoneManager mngInfo;
        private string currentUserIp;
        public bool isValid = true;
        private HttpContext context;
        private ANO_User user = null;


        public ChatSystem(HttpContext context,
            string currentUserIP)
        {
            mngInfo = new TimeZoneManager(currentUserIP);
            this.context = context;
            this.currentUserIp = currentUserIP;

            user = ANOProfile.GetCookieValues(currentUserIP);

        }

        private struct GetChatMessagesColumnNoPosition
        {
            public Guid userid;
        }

        private static readonly Func<Searchwar_netEntities, GetChatMessagesColumnNoPosition, IQueryable<SW_ChatMessage>> GetFastChatMessages = System.Data.Objects.CompiledQuery.Compile<Searchwar_netEntities, GetChatMessagesColumnNoPosition, IQueryable<SW_ChatMessage>>
                                              ((db, p) => (from c in db.SW_chat.Where(c => c.SW_ChatMessages.Where(m => m.ChatPersonId == p.userid).FirstOrDefault() != null && c.ChatIsClosed == false)
                                                           join m in db.SW_ChatMessages on c.ChatId equals m.ChatId
                                                           select m));

        public ManagerResponseObj ChatMessages()
    {

        Searchwar_netEntities db = new Searchwar_netEntities();
        

            GetChatMessagesColumnNoPosition p = new GetChatMessagesColumnNoPosition()
            {
                userid = user.UserID
            };
            List<SW_ChatMessage> result = GetFastChatMessages(db, p).ToList<SW_ChatMessage>();


            XElement chatRootTag = new XElement("chat");

            chatRootTag.Add(new XElement("ci"));

            XElement chatitemsTag = chatRootTag.Descendants("ci").Single();

            if (result != null)
            {

                if (result.Count() > 0)
                {

                    List<SW_ChatMessage> chats = new List<SW_ChatMessage>();
                    foreach (SW_ChatMessage citem in result)
                    {
                        if (!chats.Any(c => c.ChatId == citem.ChatId))
                        {
                            chats.Add(citem);
                        }
                    }

                    foreach (SW_ChatMessage citem in chats)
                    {
                        XElement createChatTag = new XElement("i", new XAttribute("id", citem.ChatId));

                        foreach (SW_ChatMessage mitem in result.Where(c => c.ChatId == citem.ChatId))
                        {
                            createChatTag.Add(new XElement("m", new XAttribute("ui", mitem.ChatPersonId.ToString()), new XAttribute("d", mitem.ChatMsgDateAdded.ToString()),
                                new XElement("t", mitem.ChatMsgText)));
                        }

                        chatitemsTag.Add(createChatTag);

                    }

                    chats = null;

                }

            }

            ManagerResponseObj mro = new ManagerResponseObj();
            mro.DataObject = result;
            mro.Xml = chatRootTag;

            db.Dispose();

            return mro;
            
    }


        public void CreateMsg(string text,
            Guid? chatID,
            Guid userID,
            string username)
    {
        DateTime datetimenow = TimeZoneManager.DateTimeNow;
        using (Searchwar_netEntities db = new Searchwar_netEntities())
        {

            SW_chat chate = null;

            if (!chatID.HasValue)
            {
                chate = new SW_chat();
                chate.ChatId = Guid.NewGuid();
                chate.ChatDateAdded = datetimenow;
                chate.ChatDateEdit = datetimenow;
                chate.ChatIsClosed = false;

                db.SW_chat.AddObject(chate);
                db.SaveChanges();
            }
            else
            {
                chate = db.SW_chat.SingleOrDefault(c => c.ChatId == chatID);
                chate.ChatDateEdit = datetimenow;
                chate.ChatIsClosed = false;
            }

            if (chate != null)
            {

                SW_ChatPerson createPerson = db.SW_ChatPersons.SingleOrDefault(p => p.ChatPersonId == userID);

                if (createPerson == null)
                {

                    createPerson = new SW_ChatPerson();
                    createPerson.ChatPersonId = userID;
                    createPerson.ChatPersonName = Membership.GetUser(userID) != null ? Membership.GetUser(userID).UserName : username;
                    createPerson.ChatPersonDateAdded = datetimenow;
                    createPerson.ChatPersonDateEdit = datetimenow;

                    db.SW_ChatPersons.AddObject(createPerson);
                    db.SaveChanges();

                }

                SW_ChatMessage createText = new SW_ChatMessage();
                createText.ChatId = chatID.Value;
                createText.ChatMsgId = Guid.NewGuid();
                createText.ChatPersonId = createPerson.ChatPersonId;
                createText.ChatMsgText = text;
                createText.ChatMsgDateAdded = datetimenow;

                db.SW_ChatMessages.AddObject(createText);
                db.SaveChanges();

            }
        }
    }


        public void OpenChat(Guid chatID)
        {
            DateTime datetimenow = TimeZoneManager.DateTimeNow;
            Searchwar_netEntities db = new Searchwar_netEntities();

            SW_chat chatobj = db.SW_chat.SingleOrDefault(c => c.ChatId == chatID);

            if (chatobj != null)
            {
                chatobj.ChatIsClosed = false;
                chatobj.ChatDateEdit = datetimenow;
            }

            db.SaveChanges();

        }

        public void CloseChat(Guid chatID, int minutes)
        {
            DateTime datetimenow = TimeZoneManager.DateTimeNow;
            Searchwar_netEntities db = new Searchwar_netEntities();

            SW_chat chatobj = db.SW_chat.SingleOrDefault(c => c.ChatId == chatID);

            if (chatobj != null)
            {
                if (chatobj.ChatDateEdit < datetimenow.AddMinutes(-minutes))
                {
                    chatobj.ChatIsClosed = true;
                    chatobj.ChatDateEdit = datetimenow;
                }
            }

            db.SaveChanges();

        }

    }
}