using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using ChatterBotAPI;
using dotOmegle;

namespace CleverOmegleGUI.ChatterBotEx
{
    /// <summary>
    /// Implements Sensation Bot for ChatterBotAPI
    /// </summary>
    public class SensationBot : ChatterBot
    {
        /// <summary>
        /// Sensation Bot supports multiple 'profiles'.
        /// This enum holds a list of the values currently supported.
        /// </summary>
        public enum BotType
        {
            /// <summary>
            /// General chatter bot
            /// </summary>
            GeneralChat,
            /// <summary>
            /// Romantic male bot
            /// </summary>
            Romantic,
            /// <summary>
            /// Smacktalk bot (Super rude)
            /// </summary>
            SmackTalk,
            /// <summary>
            /// Adult female bot
            /// </summary>
            AdultM2F,
            /// <summary>
            /// Adult male bot
            /// </summary>
            AdultF2M,
            /// <summary>
            /// Adult female bot (Lesbian)
            /// </summary>
            AdultL,
            /// <summary>
            /// Adult male bot (Gay)
            /// </summary>
            AdultG
        }

        /// <summary>
        /// Internal array that corresponds to the values in the <see cref=">BotType"/> enum.
        /// </summary>
        protected readonly string[] dbType = new string[]
        {
            "general",
            "romanticbot",
            "smacktalk",
            "sexchat_m",
            "sexchat_f",
            "sexchat_l",
            "sexchat_g"
        };

        public BotType Type
        {
            get;
            set;
        }

        public int ChatLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SensationBot"/> class.
        /// </summary>
        /// <param name="type">The bot type.</param>
        /// <param name="chatLevel">The chat level.</param>
        public SensationBot(BotType type, int chatLevel = 0)
        {
            this.ChatLevel = chatLevel;
            this.Type = type;
        }

        public ChatterBotSession CreateSession()
        {
            return new SensationBotSession(dbType[(int)Type], ChatLevel);
        }
    }

    /// <summary>
    /// Implements a session with Sensation Bot.
    /// </summary>
    internal class SensationBotSession : ChatterBotSession
    {
        protected readonly NameValueCollection vars;
        protected readonly PostSubmitter request;

        protected readonly Uri rootUrl;
        protected readonly Uri initUrl;
        protected readonly Uri pollUrl;
        protected readonly Uri postUrl;

        protected int pollDelay = 4000;

        public SensationBotSession(string db, int chatlevel)
        {
            vars = new NameValueCollection();

            rootUrl = new Uri("http://www.sensationbot.com");
            initUrl = new Uri(rootUrl, "jschat.php");
            pollUrl = new Uri(rootUrl, "jspoll.php");
            postUrl = new Uri(rootUrl, "jswrap.php");

            request = new PostSubmitter();
            request.Type = PostSubmitter.PostTypeEnum.Get;
            request.CookieContainer = new CookieContainer();

            vars["db"] = db;
            vars["chatid"] = null;
            vars["pd"] = pollDelay.ToString();
            vars["kp"] = "0";
            vars["foc"] = "1";
            vars["cl"] = chatlevel.ToString();
        }

        private string getChatID()
        {
            NameValueCollection db = new NameValueCollection();

            db["db"] = vars["db"];

            request.Type = PostSubmitter.PostTypeEnum.Get;

            string response = request.Post(initUrl.ToString() + "?db=" + vars["db"]);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(response);

            string id = (from node in doc.DocumentNode.SelectNodes("//script")
                         let match = Regex.Match(node.InnerText, "chatid=(?<id>[\\w\\d]+)&")
                         where match.Success
                         let capture = match.Groups["id"].Value.ToString()
                         select capture).Single().ToString();

            return id;
        }

        public string poll()
        {
            //request.Type = PostSubmitter.PostTypeEnum.Post;
            return "";
        }

        public string Think(string text)
        {
            if (vars["chatid"] == null || vars["chatid"] == string.Empty)
                vars["chatid"] = getChatID();

            string url = buildUrl(postUrl.ToString());

            request.PostItems.Add("line", text);
            request.Type = PostSubmitter.PostTypeEnum.Post;

            string response = WebUtility.HtmlDecode(request.Post(url));

            request.PostItems.Remove("line");
            request.PostItems.Add("poll", "1");

            if (response.Length == 0)
            {
                vars["pd"] = "100";
                Thread.Sleep(100);

                for (; ; )
                {
                    response = request.Post(buildUrl(pollUrl.ToString()));
                    if (response.Length > 0)
                        break;
                    else
                    {
                        vars["pd"] = pollDelay.ToString();
                        Thread.Sleep(int.Parse(vars["pd"]));
                    }
                };
            }

            return response;
        }

        public ChatterBotThought Think(ChatterBotThought thought)
        {
            ChatterBotThought r = new ChatterBotThought();
            r.Text = Think(thought.Text);
            return r;
        }

        private string buildUrl(string baseUrl)
        {
            StringBuilder sb = new StringBuilder(baseUrl);

            for (int i = 0; i < vars.Count; i++)
                sb.Append(string.Format("{0}{1}={2}",
                    (i == 0 ? "?" : "&"),
                    HttpUtility.UrlEncode(vars.Keys[i]),
                    HttpUtility.UrlEncode(vars[i])));

            return sb.ToString();
        }
    }
}