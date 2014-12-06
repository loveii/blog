using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loveii.Repositories
{
    public class CreateRepository
    {
        public static readonly string Loveii_KEY = "loveii";

        public static PostRepository Post = new PostRepository();

        public static CommentRepository Comment = new CommentRepository();

        public static TermRepository Term = new TermRepository();

        public static UserRepository User = new UserRepository();

        public static LinkRepository Link = new LinkRepository();
    }
}