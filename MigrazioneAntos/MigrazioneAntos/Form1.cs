using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigrazioneAntos
{
    public partial class Form1 : Form
    {
        private struct ZDLoginParameters
        {
            public string Url;
            public string Username;
            public string Password;
            public string Token;

            public string ZDUrl;
            public string AntosUrl;

            public ZDLoginParameters(string url, string user, string pwd, string token, string zdUrl, string antosUrl)
            {
                Url = url;
                Username = user;
                Password = pwd;
                Token = token;
                ZDUrl = zdUrl;
                AntosUrl = antosUrl;
            }
        }

        private struct ZDBridge
        {
            public ZDLoginParameters Source;
            public ZDLoginParameters Destination;
        }

        private List<ZDBridge> _bridges = new List<ZDBridge>();

        public Form1()
        {
            InitializeComponent();

            SetCredentials();
        }

        private void SetCredentials()
        {
            /*
            ZDBridge bravo = new ZDBridge();
            bravo.Source = new ZDLoginParameters(
                "bravomanufacturing",
                MigrazioneAntos.Properties.Settings.Default.ZDUser,
                MigrazioneAntos.Properties.Settings.Default.ZDPassword,
                "maOG2pzEKraAG6E9oRrMgCdTP3XqMpjhkwK4J3Yv",
                "bravomanufacturing.zendesk.com",
                "support.bravomanufacturing.it"
                );
            bravo.Destination = new ZDLoginParameters(
                "bravosupport",
                MigrazioneAntos.Properties.Settings.Default.ZDUser,
                MigrazioneAntos.Properties.Settings.Default.ZDPassword,
                "eCCDxn1MW7OEDEz3ny4hitfqQEZpSwARfuthZp2o",
                "bravosupport.zendesk.com",
                "support.bravomanufacturing.it"
                );


            _bridges.Add(bravo);
            /**/

            /**/
            ZDBridge perfetto = new ZDBridge();
            perfetto.Source = new ZDLoginParameters(
                "perfetto",
                MigrazioneAntos.Properties.Settings.Default.ZDUser,
                MigrazioneAntos.Properties.Settings.Default.ZDPassword,
                "OAqWa3Jum974GyGpa64w6R4t89tDfApdJgaAdNvc",
                "perfetto.zendesk.com",
                "support.myperfetto.it"
                );
            perfetto.Destination = new ZDLoginParameters(
                "perfettosupport",
                MigrazioneAntos.Properties.Settings.Default.ZDUser,
                MigrazioneAntos.Properties.Settings.Default.ZDPassword,
                "eCCDxn1MW7OEDEz3ny4hitfqQEZpSwARfuthZp2o",
                "perfettosupport.zendesk.com",
                "support.myperfetto.it"
                );

            _bridges.Add(perfetto);

            /**/

            /*
            ZDBridge antos = new ZDBridge();
            antos.Source = new ZDLoginParameters(
                "antossrl",
                MigrazioneAntos.Properties.Settings.Default.ZDUser,
                MigrazioneAntos.Properties.Settings.Default.ZDPassword
                );

            _bridges.Add(antos);
            /**/

            /*
            ZDBridge uaucrm = new ZDBridge();
            uaucrm.Source = new ZDLoginParameters(
                "uaucrm",
                MigrazioneAntos.Properties.Settings.Default.ZDUser,
                "AntosMR",
                "IHlzV6EsNSW2EK4piLc2clZS9fATLd75MIslXx8Y",
                "uaucrm.zendesk.com",
                "support.uaucrm.it"
                );
            uaucrm.Destination = new ZDLoginParameters(
               "uausupport",
               MigrazioneAntos.Properties.Settings.Default.ZDUser,
               MigrazioneAntos.Properties.Settings.Default.ZDPassword,
               "eCCDxn1MW7OEDEz3ny4hitfqQEZpSwARfuthZp2o",
               "uausupport.zendesk.com",
               "support.uaucrm.it"
               );

            _bridges.Add(uaucrm);

            /* */
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            Log("Sync started");

            foreach (ZDBridge bridge in _bridges)
            {
                Log(String.Empty);
                Log("Starting bridge: " + bridge.Source.Url);
                ListArticles(timestamp, bridge);
            }

            Log("Sync terminated");

        }



        private async void ListArticles(string timestamp, ZDBridge bridge)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            try
            {
                string localPath = Path.Combine(MigrazioneAntos.Properties.Settings.Default.LocalBackupPath, timestamp, bridge.Source.Url);

                CheckDirectories(localPath);

                WebClient client = new WebClient();

                ZendeskApi_v2.ZendeskApi sourceEndPoint = new ZendeskApi_v2.ZendeskApi(
                    bridge.Source.Url,
                    bridge.Source.Username,
                    //bridge.Source.Password
                    bridge.Source.Token,
                    "en-us"
                    );

                ZendeskApi_v2.ZendeskApi destinationEndPoint = new ZendeskApi_v2.ZendeskApi(
                    bridge.Destination.Url,
                    bridge.Destination.Username,
                    //bridge.Destination.Password,
                    bridge.Destination.Token,
                    "en-us"
                    );

                /*
                client.DownloadFile(sourceEndPoint.ZendeskUrl + "help_center/articles.json?per_page=100", Path.Combine(localPath, "articles", "articles.json"));
                Log("[" + bridge.Source.Url + "] articles.json downloaded");

                if (bridge.Source.Url == "perfetto" || bridge.Source.Url == "bravo")
                {
                    client.DownloadFile(sourceEndPoint.ZendeskUrl + "help_center/articles.json?per_page=100&page=2", Path.Combine(localPath, "articles", "articles2.json"));
                    Log("[" + bridge.Source.Url + "] articles2.json downloaded");
                }

                if (bridge.Source.Url == "perfetto")
                {
                    client.DownloadFile(sourceEndPoint.ZendeskUrl + "help_center/articles.json?per_page=100&page=3", Path.Combine(localPath, "articles", "articles3.json"));
                    Log("[" + bridge.Source.Url + "] articles3.json downloaded");
                }

                if (bridge.Source.Url == "perfetto")
                {
                    client.DownloadFile(sourceEndPoint.ZendeskUrl + "help_center/articles.json?per_page=100&page=4", Path.Combine(localPath, "articles", "articles4.json"));
                    Log("[" + bridge.Source.Url + "] articles4.json downloaded");
                }

                client.DownloadFile(sourceEndPoint.ZendeskUrl + "help_center/sections.json?per_page=100", Path.Combine(localPath, "articles", "sections.json"));
                Log("[" + bridge.Source.Url + "] sections.json downloaded");

                client.DownloadFile(sourceEndPoint.ZendeskUrl + "help_center/community/topics.json?per_page=100", Path.Combine(localPath, "topics", "topics.json"));
                Log("[" + bridge.Source.Url + "] topics.json downloaded");

                client.DownloadFile(sourceEndPoint.ZendeskUrl + "help_center/community/posts.json?per_page=100", Path.Combine(localPath, "posts", "posts.json"));
                Log("[" + bridge.Source.Url + "] posts.json downloaded");
                */
                #region process categories
                Dictionary<long, long> mapCategories = new Dictionary<long, long>();

                var sourceCategories = sourceEndPoint.HelpCenter.Categories.GetCategories().Categories;
                var destinationCategories = destinationEndPoint.HelpCenter.Categories.GetCategories().Categories;
                Log(sourceCategories.Count + " categories found");

                foreach (var sourceCategory in sourceCategories)
                {
                    bool found = false;

                    foreach (var destinationCategory in destinationCategories)
                    {
                        if (String.Compare(sourceCategory.Name, destinationCategory.Name) == 0)
                        {
                            mapCategories.Add((long)sourceCategory.Id, (long)destinationCategory.Id);
                            Log("Destination category found: " + destinationCategory.Name + " ID: " + destinationCategory.Id);

                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        ZendeskApi_v2.Models.HelpCenter.Categories.Category c = new ZendeskApi_v2.Models.HelpCenter.Categories.Category();
                        c.Description = sourceCategory.Description;
                        c.Locale = sourceCategory.Locale;
                        c.Name = sourceCategory.Name;
                        c.Position = sourceCategory.Position;
                        c.Translations = sourceCategory.Translations;

                        var catResponse = destinationEndPoint.HelpCenter.Categories.CreateCategory(c);
                        mapCategories.Add((long)sourceCategory.Id, (long)catResponse.Category.Id);
                        Log("Destination category created: " + catResponse.Category.Name + " ID: " + catResponse.Category.Id);
                    }
                }

                #endregion

                #region process sections
                Dictionary<long, long> mapSections = new Dictionary<long, long>();

                var sourceSections = sourceEndPoint.HelpCenter.Sections.GetSections().Sections;
                var destinationSections = destinationEndPoint.HelpCenter.Sections.GetSections().Sections;
                Log(sourceSections.Count + " sections found");

                foreach (var sourceSection in sourceSections)
                {
                    bool found = false;

                    foreach (var destinationSection in destinationSections)
                    {
                        if (String.Compare(sourceSection.Name, destinationSection.Name) == 0)
                        {
                            mapSections.Add((long)sourceSection.Id, (long)destinationSection.Id);
                            Log("Destination section found: " + destinationSection.Name + " ID: " + destinationSection.Id);

                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        ZendeskApi_v2.Models.Sections.Section s = new ZendeskApi_v2.Models.Sections.Section();
                        s.CategoryId = mapCategories[(long)sourceSection.CategoryId];
                        s.Description = sourceSection.Description;
                        s.Name = sourceSection.Name;
                        s.Position = sourceSection.Position;
                        s.Locale = sourceSection.Locale;
                        s.Translations = sourceSection.Translations;


                        //var souResponse = destinationEndPoint.HelpCenter.Sections.RunRequest("help_center/categories/"+ s.CategoryId + "/sections", "POST", s);
                        var souResponse = destinationEndPoint.HelpCenter.Sections.CreateSection(s);
                        mapSections.Add((long)sourceSection.Id, (long)souResponse.Section.Id);
                        Log("Destination section created: " + souResponse.Section.Name);
                    }
                }

                #endregion

                #region process articles
                IList<ZendeskApi_v2.Models.Articles.Article> articles = sourceEndPoint.HelpCenter.Articles.GetArticles(
                    ZendeskApi_v2.Requests.HelpCenter.ArticleSideLoadOptionsEnum.Translations,
                    null,
                    100
                    ).Articles;

                while (articles.Count % 100f == 0)
                {


                    var addons = sourceEndPoint.HelpCenter.Articles.GetArticles(
                    ZendeskApi_v2.Requests.HelpCenter.ArticleSideLoadOptionsEnum.Translations,
                    null,
                    100,
                    Convert.ToInt32(Math.Round(articles.Count / 100f, 0)) + 1
                    ).Articles;

                    if (addons.Count > 0)
                    {
                        foreach (var item in addons)
                            articles.Add(item);
                        //articles = (IList<ZendeskApi_v2.Models.Articles.Article>)(articles.Concat(addons));
                    }
                    else
                        break;
                }

                Log("[" + bridge.Source.Url + "] " + articles.Count + " articles found");

                int idx = 0;
                foreach (var article in articles)
                {
                    idx++;

                    /*
                    if (idx != 17)
                        continue;
                        */

                    Log("[" + bridge.Source.Url + "][Article " + idx + " of " + articles.Count + "] Processing " + article.Id);
                    Log("[" + bridge.Source.Url + "][Article " + idx + " of " + articles.Count + "] " + article.Title);

                    string articleAttachmentsPath = Path.Combine(localPath, "articles", article.Id.ToString());

                    if (!Directory.Exists(articleAttachmentsPath))
                        Directory.CreateDirectory(articleAttachmentsPath);

                    var json = JsonConvert.SerializeObject(article);
                    File.WriteAllText(Path.Combine(localPath, "article-" + article.Id.ToString() + "-dump.json"), json);

                    ZendeskApi_v2.Models.Articles.Article a = new ZendeskApi_v2.Models.Articles.Article();

                    a.Body = article.Body;
                    a.Locale = article.Locale;
                    a.Position = article.Position;
                    a.SectionId = mapSections[(long)article.SectionId];
                    a.Title = article.Title;
                    //a.Translations = article.Translations;

                    //buffer
                    string newBody = article.Body;

                    var artResponse = destinationEndPoint.HelpCenter.Articles.CreateArticle((long)a.SectionId, a);
                    Log("Article synced - NEW ID: " + artResponse.Arcticle.Id);

                    IList<ZendeskApi_v2.Models.Shared.Attachment> attachments = sourceEndPoint.Attachments.GetAttachmentsFromArticle(article.Id).Attachments;

                    Log("[" + bridge.Source.Url + "][Article " + idx + " of " + articles.Count + "] " + attachments.Count + " attachments found");


                    List<ZendeskApi_v2.Models.HelpCenter.Translations.Translation> articleTranslations = (List<ZendeskApi_v2.Models.HelpCenter.Translations.Translation>)article.Translations;

                    if (attachments.Count > 0)
                    {
                        foreach (ZendeskApi_v2.Models.Shared.Attachment attachment in attachments)
                        {

                            client.DownloadFile(attachment.ContentUrl, Path.Combine(articleAttachmentsPath, attachment.FileName));
                            Log("[" + bridge.Source.Url + "][Article " + idx + " of " + articles.Count + "] " + Path.Combine(articleAttachmentsPath, attachment.FileName) + " downloaded");

                            if (attachment.ContentType.StartsWith("image"))
                            {
                                string uploadRes = AsyncHelpers.RunSync<string>(() => UploadAttachment(
                                   destinationEndPoint.ZendeskUrl + "help_center/articles/" + artResponse.Arcticle.Id + "/attachments.json",
                                   Path.Combine(articleAttachmentsPath, attachment.FileName),
                                   bridge.Destination.Username,
                                   bridge.Destination.Token
                                   ));

                                if (!String.IsNullOrEmpty(uploadRes))
                                {
                                    ArticleAttachmentResponse obj = JsonConvert.DeserializeObject<ArticleAttachmentResponse>(uploadRes);

                                    Log(uploadRes);

                                    newBody = SubstituteAttachment(
                                        newBody,
                                        (attachment.ContentUrl).Replace("http://", "https://"),
                                        obj.article_attachment.content_url,
                                        bridge.Source.ZDUrl,
                                        bridge.Source.AntosUrl
                                        );

                                    if (articleTranslations != null && articleTranslations.Count > 0)
                                    {
                                        for (int i = 0; i < articleTranslations.Count; i++)
                                        {
                                            articleTranslations[i].Body = SubstituteAttachment(
                                                articleTranslations[i].Body,
                                                (attachment.ContentUrl).Replace("http://", "https://"),
                                                obj.article_attachment.content_url,
                                                bridge.Source.ZDUrl,
                                                bridge.Source.AntosUrl
                                                );
                                        }
                                    }

                                    Log("[" + bridge.Source.Url + "][Article " + idx + " of " + articles.Count + "] " + Path.Combine(articleAttachmentsPath, attachment.FileName) + " uploaded");
                                }
                                else
                                {
                                    Log("[" + bridge.Source.Url + "][Article " + idx + " of " + articles.Count + "] " + Path.Combine(articleAttachmentsPath, attachment.FileName) + " ************ SKIPPED FOR ERROR ************ ");
                                }

                            }
                            else
                            {
                                Log("[" + bridge.Source.Url + "][Article " + idx + " of " + articles.Count + "] " + Path.Combine(articleAttachmentsPath, attachment.FileName) + " ************ SKIPPED ************ ");
                            }

                        }

                        var insertedArticle = artResponse.Arcticle;
                        insertedArticle.Body = newBody;
                        var artUpdateResponse = destinationEndPoint.HelpCenter.Articles.UpdateArticle(insertedArticle);
                        Log("Article update with new attachments paths");

                        if (articleTranslations != null)
                        {
                            foreach (var translation in articleTranslations)
                            {
                                if (translation.Locale == "it")
                                    continue;

                                ZendeskApi_v2.Models.HelpCenter.Translations.Translation t = new ZendeskApi_v2.Models.HelpCenter.Translations.Translation();

                                t.Title = translation.Title;
                                t.Body = translation.Body;
                                t.Locale = translation.Locale;
                                t.SourceId = (long)artResponse.Arcticle.Id;
                                t.SourceType = translation.SourceType;

                                var transResponse = AsyncHelpers.RunSync<string>(() => CreateTranslation(
                                    destinationEndPoint.ZendeskUrl + "help_center/articles/" + artResponse.Arcticle.Id + "/translations.json",
                                    t,
                                    bridge.Destination.Username,
                                    bridge.Destination.Token
                                   )
                                );

                                Log(transResponse);
                            }
                        }
                    }

                    //TODO - remove
                    //break;                   

                    Log("[" + bridge.Source.Url + "][Article " + idx + " of " + articles.Count + "] Processed " + article.Id);
                }
                #endregion

                #region process topics
                /*
                var topics = sourceEndPoint.Topics.GetTopics().Topics;

                foreach (var topic in topics)
                {
                    //TODO - send to destination
                }
                /**/
                #endregion

                #region process posts

                #endregion

            }
            catch (System.Net.WebException wex)
            {
                Log("[ERROR] " + wex.Message);
            }
            catch (Exception ex)
            {
                Log("[ERROR] " + ex.Message);
            }
        }

        private string SubstituteAttachment(string body, string originalUrl, string newUrl, string zdUrl, string redirectUrl)
        {
            body = body.Replace(
                originalUrl,
                newUrl
                );

            string redirected = originalUrl.Replace(zdUrl, redirectUrl);

            body = body.Replace(
                redirected,
                newUrl
                );

            string localized_it = redirected.Replace("/hc/", "/hc/it/");

            body = body.Replace(
                localized_it,
                newUrl
                );

            string localized_en = redirected.Replace("/hc/", "/hc/en/");

            body = body.Replace(
                localized_en,
                newUrl
                );

            string localized_en2 = redirected.Replace("/hc/", "/hc/en-us/");

            body = body.Replace(
                localized_en2,
                newUrl
                );

            if (originalUrl.Contains("https://"))
                return SubstituteAttachment(body, originalUrl.Replace("https://", "http://"), newUrl, zdUrl, redirectUrl);
            else
                return body;
        }

        private void CheckDirectories(string localPath)
        {
            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);

            if (!Directory.Exists(Path.Combine(localPath, "articles")))
                Directory.CreateDirectory(Path.Combine(localPath, "articles"));

            if (!Directory.Exists(Path.Combine(localPath, "topics")))
                Directory.CreateDirectory(Path.Combine(localPath, "topics"));

            if (!Directory.Exists(Path.Combine(localPath, "posts")))
                Directory.CreateDirectory(Path.Combine(localPath, "posts"));
        }

        private void Log(string msg)
        {
            this.logTextBox.AppendText("[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "] " + msg + Environment.NewLine);
            this.logTextBox.ScrollToCaret();

            Application.DoEvents();
        }

        public static byte[] imageToByteArray(System.Drawing.Image imageIn, string extension)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (String.Compare(extension, ".png", true) == 0)
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                if ((String.Compare(extension, ".jpg", true) == 0) || (String.Compare(extension, "jpeg", true) == 0))
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                if (String.Compare(extension, ".gif", true) == 0)
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);


                return ms.ToArray();
            }
        }

        static async Task<string> UploadAttachment(string endpoint, string filePath, string user, string token)
        {
            FileInfo imageInfo = new FileInfo(filePath);

            Image img;

            try { img = Image.FromFile(filePath); } catch (Exception ex) { Console.WriteLine("ERROR DFK: " + filePath + " --- " + ex.Message); return String.Empty; }

            var imageContent = new ByteArrayContent(imageToByteArray(img, imageInfo.Extension));

            var hClient = new System.Net.Http.HttpClient();
            hClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(user + "/token:" + token)));

            var requestContent = new MultipartFormDataContent();

            if (String.Compare(imageInfo.Extension, ".png", true) == 0)
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");

            if ((String.Compare(imageInfo.Extension, ".jpg", true) == 0) || (String.Compare(imageInfo.Extension, "jpeg", true) == 0))
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            if (String.Compare(imageInfo.Extension, ".gif", true) == 0)
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/gif");

            requestContent.Add(imageContent, "file", filePath);
            requestContent.Add(new StringContent("true"), "inline");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Content = requestContent;
            System.Net.Http.HttpResponseMessage response = await hClient.SendAsync(request).ConfigureAwait(false);

            // Get the response content.
            System.Net.Http.HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.
                return ((await reader.ReadToEndAsync()));
            }
        }

        static async Task<string> CreateTranslation(string endpoint, ZendeskApi_v2.Models.HelpCenter.Translations.Translation trans, string user, string token)
        {
            var hClient = new System.Net.Http.HttpClient();
            hClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(user + "/token:" + token)));

            Dictionary<string, ZendeskApi_v2.Models.HelpCenter.Translations.Translation> dict = new Dictionary<string, ZendeskApi_v2.Models.HelpCenter.Translations.Translation>();
            dict.Add("translation", trans);

            //string paypload = "{ \"translation\" : " + JsonConvert.SerializeObject(trans) + "}";
            //var requestContent = new StringContent(paypload);

            string payload = JsonConvert.SerializeObject(dict);
            var requestContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Content = requestContent;
            System.Net.Http.HttpResponseMessage response = await hClient.SendAsync(request).ConfigureAwait(false);

            // Get the response content.
            System.Net.Http.HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.
                return ((await reader.ReadToEndAsync()));
            }
        }

        private async void WipeAllArticles(ZDBridge bridge)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            try
            {
                ZendeskApi_v2.ZendeskApi destinationEndPoint = new ZendeskApi_v2.ZendeskApi(
                    bridge.Destination.Url,
                    bridge.Destination.Username,
                    //bridge.Destination.Password,
                    bridge.Destination.Token,
                    "it"
                    );

                #region process articles
                IList<ZendeskApi_v2.Models.Articles.Article> articles = destinationEndPoint.HelpCenter.Articles.GetArticles(
                    ZendeskApi_v2.Requests.HelpCenter.ArticleSideLoadOptionsEnum.None,
                    null,
                    100
                    ).Articles;

                while (articles.Count % 100f == 0)
                {
                    var addons = destinationEndPoint.HelpCenter.Articles.GetArticles(
                    ZendeskApi_v2.Requests.HelpCenter.ArticleSideLoadOptionsEnum.None,
                    null,
                    100,
                    Convert.ToInt32(Math.Round(articles.Count / 100f, 0)) + 1
                    ).Articles;

                    if (addons.Count > 0)
                    {
                        foreach (var item in addons)
                            articles.Add(item);
                    }
                    else
                        break;
                }

                Log("[" + bridge.Source.Url + "] " + articles.Count + " articles found");

                int idx = 0;
                foreach (var article in articles)
                {
                    idx++;

                    /*
                    if (idx != 17)
                        continue;
                        */

                    Log("[" + bridge.Source.Url + "][Article " + idx + " of " + articles.Count + "] Processing " + article.Id);
                    Log("[" + bridge.Source.Url + "][Article " + idx + " of " + articles.Count + "] " + article.Title);


                    var artResponse = destinationEndPoint.HelpCenter.Articles.DeleteArticle((long)article.Id);
                    Log("[" + bridge.Source.Url + "][Article " + idx + " of " + articles.Count + "] Article " + article.Id + " deleted");

                }
                #endregion
            }
            catch (System.Net.WebException wex)
            {
                Log("[ERROR] " + wex.Message);
            }
            catch (Exception ex)
            {
                Log("[ERROR] " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var listOfBridges = _bridges.Select(item => item.Destination.Url);
            var descOfBridges = String.Join("+", listOfBridges);

            DialogResult res = MessageBox.Show("Are you REALLY sure you want to wipe all the articles from " + descOfBridges + "???");

            if (res == DialogResult.OK)
            {
                Log("Wiping started");

                foreach (ZDBridge bridge in _bridges)
                {
                    Log(String.Empty);
                    Log("Starting bridge: " + bridge.Destination.Url);
                    WipeAllArticles(bridge);
                }

                Log("Wiping terminated");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Log("Sync started");
            Log(String.Empty);

            foreach (var bridge in _bridges)
                SyncCommunity(bridge);

            Log(String.Empty);
            Log("Sync terminated");

        }

        private async void SyncCommunity(ZDBridge bridge)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            try
            {
                WebClient client = new WebClient();

                ZendeskApi_v2.ZendeskApi sourceEndPoint = new ZendeskApi_v2.ZendeskApi(
                    bridge.Source.Url,
                    bridge.Source.Username,
                    //bridge.Source.Password
                    bridge.Source.Token,
                    "en-us"
                    );

                ZendeskApi_v2.ZendeskApi destinationEndPoint = new ZendeskApi_v2.ZendeskApi(
                    bridge.Destination.Url,
                    bridge.Destination.Username,
                    //bridge.Destination.Password,
                    bridge.Destination.Token,
                    "en-us"
                    );

                Dictionary<long, long> mapUsers = new Dictionary<long, long>();
                Dictionary<long, long> mapForums = new Dictionary<long, long>();
                Dictionary<long, long> mapTopics = new Dictionary<long, long>();
                Dictionary<long, long> mapPosts = new Dictionary<long, long>();

                #region process users

                long defaultID = 0;

                var sourceUsers = sourceEndPoint.Users.GetAllUsers(100, 1).Users;
                while (sourceUsers.Count % 100f == 0)
                {
                    var addons = sourceEndPoint.Users.GetAllUsers(
                    100,
                    Convert.ToInt32(Math.Round(sourceUsers.Count / 100f, 0)) + 1
                    ).Users;

                    if (addons.Count > 0)
                    {
                        foreach (var item in addons)
                            sourceUsers.Add(item);
                    }
                    else
                        break;
                }
                Log(sourceUsers.Count + " users found on source");

                var destinationUsers = destinationEndPoint.Users.GetAllUsers(100, 1).Users;
                while (destinationUsers.Count % 100f == 0)
                {
                    var addons = destinationEndPoint.Users.GetAllUsers(
                    100,
                    Convert.ToInt32(Math.Round(destinationUsers.Count / 100f, 0)) + 1
                    ).Users;

                    if (addons.Count > 0)
                    {
                        foreach (var item in addons)
                            destinationUsers.Add(item);
                    }
                    else
                        break;
                }
                Log(destinationUsers.Count + " users found on destination");

                foreach (var sourceUser in sourceUsers)
                    if (String.IsNullOrEmpty(sourceUser.Email)) Log("[ERROR] Source empty email: " + JsonConvert.SerializeObject(sourceUser));

                foreach (var destUser in destinationUsers)
                    if (String.IsNullOrEmpty(destUser.Email)) Log("[ERROR] Destionation empty email: " + JsonConvert.SerializeObject(destUser));

                //Log(JsonConvert.SerializeObject(destinationUsers));

                foreach (var sourceUser in sourceUsers)
                {
                    bool found = false;

                    if (String.IsNullOrEmpty(sourceUser.Email))
                        continue;

                    foreach (var destinationUser in destinationUsers)
                    {
                        if (String.IsNullOrEmpty(destinationUser.Email))
                            continue;

                        if (String.Compare(sourceUser.Email.Trim(), destinationUser.Email.Trim()) == 0)
                        {
                            mapUsers.Add((long)sourceUser.Id, (long)destinationUser.Id);
                            Log("User found: " + destinationUser.Email + " ID: " + destinationUser.Id);

                            if(String.Compare(destinationUser.Email, "mauro.recanatesi@antos.it")==0)
                            {
                                defaultID = (long)destinationUser.Id;
                                Log("DEFAULT User found: " + destinationUser.Email + " ID: " + destinationUser.Id);
                            }

                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        ZendeskApi_v2.Models.Users.User u = new ZendeskApi_v2.Models.Users.User();
                        u.Active = sourceUser.Active;
                        u.Alias = sourceUser.Alias;
                        u.Details = sourceUser.Details;
                        u.Email = sourceUser.Email;
                        u.ExternalId = sourceUser.ExternalId;
                        u.Moderator = sourceUser.Moderator;
                        u.Name = sourceUser.Name;
                        u.Notes = sourceUser.Notes;
                        u.Phone = sourceUser.Phone;
                        u.Role = sourceUser.Role;
                        u.Signature = sourceUser.Signature;
                        u.Suspended = sourceUser.Suspended;
                        u.TimeZone = sourceUser.TimeZone;
                        u.Verified = sourceUser.Verified;

                        if (u.Suspended)
                            u.Moderator = false;

                        var userResponse = destinationEndPoint.Users.CreateUser(u);
                        mapUsers.Add((long)sourceUser.Id, (long)userResponse.User.Id);
                        Log("User created: " + userResponse.User.Email + " ID: " + userResponse.User.Id);                        
                    }


                }

                #endregion

                #region process forums 
                /*

                var sourceForums = sourceEndPoint.Forums.GetForums().Forums;                
                Log(sourceForums.Count + " forums found on source");

                var destinationForums = destinationEndPoint.Forums.GetForums().Forums;               
                Log(destinationForums.Count + " forums found on destination");

                foreach (var sourceForum in sourceForums)
                {
                    bool found = false;

                    foreach (var destinationForum in destinationForums)
                    {
                        if (String.Compare(sourceForum.Name.Trim(), destinationForum.Name.Trim()) == 0)
                        {
                            mapForums.Add((long)sourceForum.Id, (long)destinationForum.Id);
                            Log("Forum found: " + destinationForum.Name + " ID: " + destinationForum.Id);

                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        ZendeskApi_v2.Models.Forums.Forum f = new ZendeskApi_v2.Models.Forums.Forum();
                        f.Access = sourceForum.Access;
                        f.Description = sourceForum.Description;
                        f.ForumType = sourceForum.ForumType;
                        f.Locked = sourceForum.Locked;
                        f.Name = sourceForum.Name;
                        f.Position = sourceForum.Position;                        

                        var topResponse = destinationEndPoint.Forums.CreateForum(f);
                        mapForums.Add((long)sourceForum.Id, (long)topResponse.Forum.Id);
                        Log("Topic created: " + topResponse.Forum.Name + " ID: " + topResponse.Forum.Id);
                    }
                }
                /**/
                #endregion

                #region process topics

                /*
                var sourceTopics = sourceEndPoint.Topics.GetTopics().Topics;
                
                Log(sourceTopics.Count + " topics found on source");

                var destinationTopics = destinationEndPoint.Topics.GetTopics().Topics;
              
                Log(destinationTopics.Count + " topics found on destination");

                foreach (var sourceTopic in sourceTopics)
                {
                    //Log("TOPIC :: " + JsonConvert.SerializeObject(sourceTopic));

                    bool found = false;
                    ZendeskApi_v2.Models.Topics.Topic foundTopic;

                    foreach (var destinationTopic in destinationTopics)
                    {
                        if (String.Compare(sourceTopic.Title.Trim(), destinationTopic.Title.Trim()) == 0)
                        {
                            mapTopics.Add((long)sourceTopic.Id, (long)destinationTopic.Id);
                            Log("Topic found: " + destinationTopic.Title + " ID: " + destinationTopic.Id);

                            foundTopic = destinationTopic;

                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        ZendeskApi_v2.Models.Topics.Topic t = new ZendeskApi_v2.Models.Topics.Topic();
                        t.Title = sourceTopic.Title;
                        t.Body = sourceTopic.Body;
                        t.CommentsCount = sourceTopic.CommentsCount;
                        t.Highlighted = sourceTopic.Highlighted;
                        t.Locked = sourceTopic.Locked;
                        t.Pinned = sourceTopic.Pinned;
                        t.Position = sourceTopic.Position;
                        t.Tags = sourceTopic.Tags;
                        t.TopicType = sourceTopic.TopicType;
                        t.SubmitterId = mapUsers[(long)sourceTopic.SubmitterId];
                        t.ForumId = mapForums[(long)sourceTopic.ForumId];
                                                
                        var topResponse = destinationEndPoint.Topics.CreateTopic(t);
                        mapTopics.Add((long)sourceTopic.Id, (long)topResponse.Topic.Id);
                        Log("Topic created: " + topResponse.Topic.Title + " ID: " + topResponse.Topic.Id);

                        foundTopic = topResponse.Topic;
                    }


                    



                }
                /**/
                #endregion


                //return;

                #region process posts
                
                string sourcePostsRes = AsyncHelpers.RunSync<string>(() => GenericGET(sourceEndPoint.ZendeskUrl + "community/posts.json?per_page=100", bridge.Source.Username, bridge.Source.Token));
                Log(sourceEndPoint.ZendeskUrl + "community/posts.json");
                //Log("POSTS RES :: " + sourcePostsRes);
                PostsResponse sourcePosts = JsonConvert.DeserializeObject<PostsResponse>(sourcePostsRes);

                Log("Posts found: " + sourcePosts.posts.Count);

                foreach (var sourcePost in sourcePosts.posts)
                {
                    string sourcePostsCommentsRes = AsyncHelpers.RunSync<string>(() => GenericGET(sourceEndPoint.ZendeskUrl + "community/posts/" + sourcePost.id + "/comments.json", bridge.Source.Username, bridge.Source.Token));
                    PostCommentsResponse sourcePostsComments = JsonConvert.DeserializeObject<PostCommentsResponse>(sourcePostsCommentsRes);

                    Log("Posts Comments for post " + sourcePost.title + " found: " + sourcePostsComments.comments.Count);
                    

                    Post newpost = new Post();
                    if (mapUsers.Keys.Contains(sourcePost.author_id))
                        newpost.author_id = (mapUsers[(long)sourcePost.author_id]);
                    else
                        newpost.author_id = defaultID;

                    newpost.closed = sourcePost.closed;
                    newpost.comment_count= sourcePost.comment_count;
                    newpost.created_at= sourcePost.created_at;
                    newpost.details = sourcePost.details;
                    newpost.featured= sourcePost.featured;
                    newpost.follower_count= sourcePost.follower_count;
                    newpost.pinned= sourcePost.pinned;
                    newpost.status= sourcePost.status;
                    newpost.title= sourcePost.title;
                    //newpost.topic_id = 200567937;//bravo-idee e funzionalità
                    newpost.topic_id = 200586498;
                    newpost.updated_at= sourcePost.updated_at;
                    
                    Dictionary<string, Post> dict = new Dictionary<string, Post>();
                    dict.Add("post", newpost);                 

                    string payload = JsonConvert.SerializeObject(dict);

                    //Log(payload);

                    string destPostsRes = AsyncHelpers.RunSync<string>(() => GenericPOST(destinationEndPoint.ZendeskUrl + "community/posts.json",
                            payload,
                            bridge.Destination.Username,
                            bridge.Destination.Token));

                    PostSingleResponse destPost = JsonConvert.DeserializeObject<PostSingleResponse>(destPostsRes);

                    foreach (var sourcePostComment in sourcePostsComments.comments)
                    {
                        PostComment newcomment = new PostComment();
                        if (mapUsers.Keys.Contains(sourcePostComment.author_id))
                            newcomment.author_id = (mapUsers[sourcePostComment.author_id]);
                        else
                            newcomment.author_id = defaultID;

                        newcomment.updated_at = sourcePostComment.updated_at;
                        newcomment.body = sourcePostComment.body;
                        newcomment.official = sourcePostComment.official;
                        newcomment.post_id = destPost.post.id;
                        newcomment.created_at = sourcePostComment.created_at;
                        newcomment.vote_count = sourcePostComment.vote_count;
                        newcomment.vote_sum = sourcePostComment.vote_sum;
                        

                        Dictionary<string, PostComment> dict2 = new Dictionary<string, PostComment>();
                        dict2.Add("comment", newcomment);

                        string payload2 = JsonConvert.SerializeObject(dict2);


                        string destPostCommentRes = AsyncHelpers.RunSync<string>(() => GenericPOST(destinationEndPoint.ZendeskUrl + "community/posts/"+destPost.post.id +"/comments.json",
                            payload2,
                            bridge.Destination.Username,
                            bridge.Destination.Token));

                        Log(destPostsRes);
                    }

                }

                #endregion

            }
            catch (System.Net.WebException wex)
            {
                Log("[ERROR] " + wex.Message);
            }
            catch (Exception ex)
            {

                Log("[ERROR] " + ex.Message);
            }
        }

        private async Task<string> GenericGET(string endpoint, string user, string token)
        {
            var hClient = new System.Net.Http.HttpClient();
            hClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(user + "/token:" + token)));

            //string paypload = "{ \"translation\" : " + JsonConvert.SerializeObject(trans) + "}";
            //var requestContent = new StringContent(paypload);

            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            System.Net.Http.HttpResponseMessage response = await hClient.SendAsync(request).ConfigureAwait(false);

            // Get the response content.
            System.Net.Http.HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.
                return ((await reader.ReadToEndAsync()));

            }
        }

        private async Task<string> GenericPOST(string endpoint, string payload,string user, string token)
        {
            var hClient = new System.Net.Http.HttpClient();
            hClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(user + "/token:" + token)));

            //string paypload = "{ \"translation\" : " + JsonConvert.SerializeObject(trans) + "}";
            var requestContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Content = requestContent;
            System.Net.Http.HttpResponseMessage response = await hClient.SendAsync(request).ConfigureAwait(false);

            // Get the response content.
            System.Net.Http.HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.
                return ((await reader.ReadToEndAsync()));

            }
        }
    }
}
