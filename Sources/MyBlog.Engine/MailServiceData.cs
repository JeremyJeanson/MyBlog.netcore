using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Engine
{
    internal sealed class UserValidationMailData
    {
        public String Name { get; set; }
        public String Email { get; set; }
        public Guid Token { get; set; }
    }

    internal sealed class NotifyUserForCommentData
    {
        public String PostTitle { get; set; }
        public String PostUri { get; set; }
        public UserAddresse[] Users { get; set; }
    }

    internal sealed class UserAddresse
    {
        public String Name { get; set; }
        public String Email { get; set; }
    }
}
