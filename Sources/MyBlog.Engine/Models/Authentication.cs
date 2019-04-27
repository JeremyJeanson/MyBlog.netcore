using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Engine.Models
{
    public sealed class AuthenticationProviders
    {
        public String ReturnUrl { get; set; }
        public AuthenticationProvider[] Providers { get; set; }
    }

    public sealed class AuthenticationProvider
    {
        public String Style { get; set; }
        public String Icon { get; set; }
        public String Name { get; set; }
        public String Provider { get; set; }
    }

    public sealed class AuthenticationChoice
    {
        public String ReturnUrl { get; set; }
        public String Provider { get; set; }
    }

    public sealed class AuthenticationCallback
    {
        public String ReturnUrl { get; set; }
        public String RemoteError { get; set; }
    }
}
