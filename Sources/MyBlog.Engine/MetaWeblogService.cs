using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MyBlog.Engine
{
    /// <summary>
    /// Service to parse Metaweblog query and build the good reply
    /// Old Doc https://msdn.microsoft.com/ko-kr/library/aa905665.aspx
    /// </summary>
    public sealed class MetaWeblogService
    {
        #region Declarations

        private const String MethodGetUsersBlogs = "blogger.getusersblogs";
        private const String MethodGetCategories = "metaweblog.getcategories";
        private const String MethodGetPost = "metaweblog.getpost";
        private const String MethodGetRecentPosts = "metaweblog.getrecentposts";
        private const String MethodNewPost = "metaweblog.newpost";
        private const String MethodEditPost = "metaweblog.editpost";
        private const String MethodDeletePost = "blogger.deletepost";
        private const String MethodNewMediaObject = "metaweblog.newmediaobject";

        private const String NodeMethodName = "methodName";
        private const String NodeMethodResponse = "methodResponse";
        private const String NodeFault = "fault";
        private const String NodeParams = "params";
        private const String NodeParam = "param";
        private const String NodeValue = "value";
        private const String NodeStruct = "struct";
        private const String NodeMember = "member";
        private const String NodeName = "name";

        private const String NodeString = "string";
        private const String NodeInteger = "int";
        private const String NodeBoolean = "boolean";

        private const String StatusPublished = "publish";
        private const String StatusDraft = "draft";

        // Default Blog Id
        public const string BlogId = "MyBlog";

        // Default User Id
        public const string UserId = "UserId";

        private readonly DataService _dataService;
        private readonly IOptions<Settings> _options;
        private readonly FilesService _filesService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataService"></param>
        public MetaWeblogService(DataService dataService, IOptions<Settings> options, FilesService filesService)
        {
            _dataService = dataService;
            _options = options;
            _filesService = filesService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Process the request
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XDocument Process(XDocument doc)
        {
            // Get method allowed by access
            String method = GetMethodAllowed(doc);
            // Test access right
            if (String.IsNullOrWhiteSpace(method))
                return ProcessError();

            // Process methodes
            switch (method)
            {
                case MethodGetUsersBlogs: return ProcessGetUsersBlogs(doc);
                case MethodGetCategories: return ProcessGetCategories(doc);
                case MethodGetRecentPosts: return ProcessGetRecentPosts(doc);
                case MethodGetPost: return PorcessGetPost(doc);
                case MethodNewPost: return ProcessNewPost(doc);
                case MethodEditPost: return ProcessEditPost(doc);
                case MethodDeletePost: return ProcessDeletePost(doc);
                case MethodNewMediaObject: return ProcessNewsMediaObject(doc);
                default: return ProcessError();
            }
        }

        /// <summary>
        /// Return the method allowed if user have the right access level
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private String GetMethodAllowed(XDocument doc)
        {
            // Get the method called
            String method = doc.Descendants(NodeMethodName).FirstOrDefault()
               ?.Value.ToLower();

            // Get arguments
            String[] arguments = doc.Descendants(NodeValue)
                ?.Select(c => c.Value)
                .ToArray();

            // Dete method is the only one to use args from 2 to 3 for authentication
            if(method == MethodDeletePost)
            {
                // Test args number
                if (arguments.Length < 4)
                    return null;

                // Test access right
                if (_dataService.PublisherAccessAllowed(arguments[2], arguments[3]))
                    return method;
            }
            else
            {
                // Test args number
                if (arguments.Length < 3)
                    return null;

                // Test access right
                if (_dataService.PublisherAccessAllowed(arguments[1], arguments[2]))
                    return method;
            }

            // No access allowed
            return null;
        }

        /// <summary>
        /// Return an error
        /// </summary>
        /// <returns></returns>
        private XDocument ProcessError()
        {
            return new XDocument(
                new XElement(NodeMethodResponse,
                    new XElement(NodeFault,
                        StructValue(
                            Member("code", NodeInteger, "0"),
                            Member("message", NodeString, "Error")
                            ))));
        }

        /// <summary>
        /// Processs the GetUsersBlogs method
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private XDocument ProcessGetUsersBlogs(XDocument doc)
        {
            return Response(
                new XElement("array",
                    new XElement("data",
                        StructValue(
                            Member("url", NodeString, _options.Value.Url),
                            Member("blogid", NodeString, BlogId),
                            Member("blogName", NodeString, _options.Value.Title)
                            ))));
        }

        /// <summary>
        /// Process the GetCategories method
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private XDocument ProcessGetCategories(XDocument doc)
        {
            // Get categories
            var categories = _dataService.GetCategories();

            return Response(
                new XElement("array",
                    new XElement("data",
                        // List all categories as node
                        categories.Select(c =>
                            StructValue(
                                Member("description", NodeString, c.Name),
                                Member("title", NodeString, c.Name)
                                )))));
        }

        /// <summary>
        /// Process the GetRecentPosts method
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private XDocument ProcessGetRecentPosts(XDocument doc)
        {
            // Get the number of posts reqeusted (args 4)
            String arg = doc.Descendants(NodeValue).Skip(3).FirstOrDefault()?.Value;
            Int32 count;
            // Test if we can parse this arg as integer
            if (!Int32.TryParse(arg, out count))
                return ProcessError();

            // Set a low bound for count
            if (count < 10) count = 10;

            // Get posts
            Data.Models.Post[] posts = _dataService.GetPostsAndDrafts(0, count);

            // Send response
            return Response(
                new XElement("array",
                    new XElement("data",
                        posts.Select(c => new XElement(NodeValue, Post(c))))));
        }

        /// <summary>
        /// Process the GetPost method
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private XDocument PorcessGetPost(XDocument doc)
        {
            //Get the first arg with the id
            String arg = doc.Descendants(NodeValue).FirstOrDefault()?.Value;
            Int32 id;
            // Test if we can parse this arg as integer
            if (!Int32.TryParse(arg, out id))
                return ProcessError();

            // GetHashCode the post
            var post = _dataService.GetPost(id);

            if(post==null)
            {
                return ProcessError();
            }

            // Return the post
            return Response(Post(post));
        }

        /// <summary>
        /// Process the NewPost method
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private XDocument ProcessNewPost(XDocument doc)
        {
            // Get the post content
            XElement postObj = doc.Descendants(NodeParam).Skip(3).FirstOrDefault()?.Descendants(NodeStruct).FirstOrDefault();

            // Get the published param
            String published = doc.Descendants(NodeParam).Skip(4).FirstOrDefault()?.Value;

            // create a new post
            var post = GetPost(postObj, published);

            // Get or create categories
            var categories = GetCategoriesOfPost(postObj);

            // Add this new post to the database
            if (_dataService.AddPost(post, categories))
            {
                return Response(new XElement(NodeInteger, post.Id));
            }

            // Return an error
            return ProcessError();
        }

        /// <summary>
        /// Process the EditPost method
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private XDocument ProcessEditPost(XDocument doc)
        {
            // Get the id of the post to edit
            String idArg = doc.Descendants(NodeParam).FirstOrDefault()?.Value;

            // Get the post content
            XElement postObj = doc.Descendants(NodeParam).Skip(3).FirstOrDefault()?.Descendants(NodeStruct).FirstOrDefault();

            // Get the published param
            String published = doc.Descendants(NodeParam).Skip(4).FirstOrDefault()?.Value;

            // create a new post
            var post = GetPost(postObj, published);

            // Get or create categories
            var categories = GetCategoriesOfPost(postObj);

            // Test if we should add or update this post
            Int32 id;
            if(Int32.TryParse(idArg,out id) && id > 0)
            {
                // Set the id
                post.Id = id;
                // Update
                if (_dataService.EditPost(post, categories))
                {
                    return Response(new XElement(NodeBoolean, "1"));
                }
            }
            else
            {
                // Add this new post to the database
                if (_dataService.AddPost(post, categories))
                {
                    return Response(new XElement(NodeBoolean, "1"));
                }
            }

            // Return an error
            return ProcessError();
        }

        /// <summary>
        /// Process the DeletePost method
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private XDocument ProcessDeletePost(XDocument doc)
        {
            //Get the second arg with the id
            String arg = doc.Descendants(NodeValue).Skip(1).FirstOrDefault()?.Value;
            Int32 id;
            // Test if we can parse this arg as integer
            if (!Int32.TryParse(arg, out id))
                return ProcessError();

            // GetHashCode the post
            Boolean post = _dataService.DeletePost(id);

            // Return the post
            return Response(new XElement(NodeBoolean, post ? 1 : 0));
        }

        /// <summary>
        /// Process the NewMediaObject method
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private XDocument ProcessNewsMediaObject(XDocument doc)
        {
            //Get the 3 arg with the struct MediaObject
            XElement obj = doc.Descendants(NodeValue).Skip(3).FirstOrDefault();

            // Get the title of this object
            String name = GetMemberValue(obj, "name");
            // Get the binary of this object
            String bits64 = GetMemberValue(obj, "bits");
            // GetMemberValue the type of this object
            String type = GetMemberValue(obj, "type");

            // Save the object
            Byte[] bits = Convert.FromBase64String(bits64);
            // Try to upload this file
            Uri uri = _filesService.Upload(name, bits)
                .GetAwaiter()
                .GetResult();

            // Error
            if (uri == null)
                return ProcessError();

            // Else return informations
            return Response(
                new XElement(NodeStruct,
                    Member("id", NodeString, name),
                    Member("file", NodeString, name),
                    Member("type", NodeString, type),
                    Member("url", NodeString, uri.ToString())));
        }

        #endregion

        #region XML-RPLC helpers

        /// <summary>
        /// Return a well formated response
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static XDocument Response(XElement value)
        {
            return new XDocument(
                new XElement(NodeMethodResponse,
                    new XElement(NodeParams,
                        new XElement(NodeParam,
                            new XElement(NodeValue, value)))));
        }

        /// <summary>
        /// Return a structure value
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        private static XElement StructValue(params XElement[] members)
        {
            return new XElement(NodeValue,
                new XElement(NodeStruct, members));
        }

        /// <summary>
        /// Get a member for a struct
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static XElement Member(String name, String type, String value)
        {
            return new XElement(NodeMember,
                new XElement(NodeName, name),
                new XElement(NodeValue,
                    new XElement(type, value)));
        }

        /// <summary>
        /// Get a member struct
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private static XElement Member(String name, String[] values)
        {
            return new XElement(NodeMember,
                new XElement(NodeName, name),
                new XElement(NodeValue,
                    new XElement("array",
                        new XElement("data",
                            values.Select(value =>
                                new XElement("value",
                                    new XElement(NodeString, value)
                                )
                    )))));
        }

        /// <summary>
        /// Get a post struct
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        private static XElement Post(Data.Models.Post post)
        {
            return new XElement(NodeStruct,
                Member("postid", NodeInteger, post.Id.ToString()),
                Member("title", NodeString, post.Title),
                Member("description", NodeString, post.GetFullContentForOpenLiveWriter()),
                Member("link", NodeString, post.Url),
                Member("userid", NodeString, UserId),
                Member("date_created_gmt", "dateTime.iso8601", post.DateCreatedGmt.ToString("yyyyMMdd'T'HH':'mm':'ss", DateTimeFormatInfo.InvariantInfo)),
                Member("permaLink", NodeString, post.Url),
                Member("categories", post.Categories.Select(c => c.Category.Name).ToArray()),
                Member("post_status", NodeString, post.Published ? StatusPublished : StatusDraft)
                );
        }

        /// <summary>
        /// Get the value of a member
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        private static String GetMemberValue(XElement obj, String memberName)
        {
            return obj.Descendants(NodeMember)
                .FirstOrDefault(c => String.Equals( c.Descendants(NodeName).FirstOrDefault().Value, memberName, StringComparison.InvariantCultureIgnoreCase))
                ?.Descendants(NodeValue).FirstOrDefault().Value;
        }

        /// <summary>
        /// Get an array of values
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        private static String[] GetMemberValues(XElement obj, String memberName)
        {
            return obj.Descendants(NodeMember)
                .FirstOrDefault(c => String.Equals(c.Descendants(NodeName).FirstOrDefault().Value, memberName, StringComparison.InvariantCultureIgnoreCase))
                ?.Descendants(NodeValue).FirstOrDefault()
                .Descendants(NodeValue).Select(c=>c.Value).ToArray();
        }

        /// <summary>
        /// Get a post from an xml element
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static Data.Models.Post GetPost(XElement obj, String published)
        {
            // Create a new post to return
            var post = new Data.Models.Post
            {
                Published = published == "1",
                Title = GetMemberValue(obj, "title"),
                DateCreatedGmt = GetPostDate(obj)
            };

            // Set content
            post.SetContentFromHtml(GetMemberValue(obj, "description"));

            return post;
        }

        /// <summary>
        /// Get publication date of post
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static DateTime GetPostDate(XElement obj)
        {
            // Try to get the gmt date of this post
            String value = GetMemberValue(obj, "date_created_gmt");
            if (String.IsNullOrWhiteSpace(value))
            {
                // Try to get the date of this post
                value = GetMemberValue(obj, "date_created");
                // Try to pars this date
                if (!String.IsNullOrWhiteSpace(value) 
                    && DateTime8601.TryParseDateTime8601(value, out DateTime result))
                {
                    return result;
                }
            }
            else
            {
                // Try to pars this date
                if(DateTime8601.TryParseDateTime8601(value,out DateTime result))
                {
                    return result;
                }
            }
            // Return the current utc time as default
            return DateTime.UtcNow;
        }

        /// <summary>
        /// Get categories
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private Int32[] GetCategoriesOfPost(XElement obj)
        {
            // Get categpries labels
            var labels = GetMemberValues(obj, "categories");

            // Create categpries if not exists
            var categories = _dataService.GetCategoriesAndCreatIfNotExists(labels);

            // Return only ids
            return categories.Select(c=>c.Id).ToArray();
        }

        #endregion

        #region Datetime

        // Borrowed from: https://github.com/snielsson/XmlRpcLight/blob/master/XmlRpcLight/DataTypes/DateTime8601.cs               
        private static class DateTime8601
        {
            private static readonly Regex _dateTime8601Regex = new Regex(
                @"(((?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2}))|((?<year>\d{4})(?<month>\d{2})(?<day>\d{2})))"
                + @"T"
                + @"(((?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2}))|((?<hour>\d{2})(?<minute>\d{2})(?<second>\d{2})))"
                + @"(?<tz>$|Z|([+-]\d{2}:?(\d{2})?))");

            public static bool TryParseDateTime8601(string date, out DateTime result)
            {
                result = DateTime.MinValue;
                Match m = _dateTime8601Regex.Match(date);

                if (m == null)
                    return false;

                string normalized = m.Groups["year"].Value + m.Groups["month"].Value + m.Groups["day"].Value
                                    + "T"
                                    + m.Groups["hour"].Value + m.Groups["minute"].Value + m.Groups["second"].Value
                                    + m.Groups["tz"].Value;

                var formats = new[] {
                "yyyyMMdd'T'HHmmss",
                "yyyyMMdd'T'HHmmss'Z'",
                "yyyyMMdd'T'HHmmsszzz",
                "yyyyMMdd'T'HHmmsszz"
                };

                try
                {
                    result = DateTime.ParseExact(normalized, formats, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #endregion
    }
}
