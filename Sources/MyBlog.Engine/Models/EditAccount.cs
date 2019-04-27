using MyBlog.Engine.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Engine.Models
{
    public sealed class EditAccount
    {
        public UserProfile User { get; set; }

        public Nullable<Boolean> Success { get; set; }
    }
}
