using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loveii.Models;
using System.Web.Mvc;
using Loveii.Services; 

namespace Loveii.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        public AdminViewModel()
        {
            this.Post = new Post(); 
            this.TermSelectList = new List<SelectListItem>();
            this.PostList = new PageResult<Post>();
        }

        public int termId { get; set; }

        public int cateId { get; set; }

        public Post Post { get; set; }

        public List<Term> TermList { get; set; }

        public List<SelectListItem> TermSelectList { get; set; }

        public PageResult<Post> PostList { get; set; }
    }
}
