using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loveii.Models; 

namespace Loveii.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            this.PostList = new PageResult<Post>();
             
            this.Post = new Post(); 
            this.cateId = 0;
            this.s = string.Empty;
            this.CommentList = new List<Comment>();
        }

        public int cateId { get; set; }

        public string cate { get; set; }

        public PageResult<Post> PostList { get; set; }
         
        public Post Post { get; set; }

        public string s { get; set; }

        public List<Comment> CommentList { get; set; }
    }
}
