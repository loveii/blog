using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loveii.Repositories;
using Loveii.Models;

namespace Loveii.Services
{
    public class LinkSrv
    { 
        public static List<Link> GetList()
        {
            return CreateRepository.Link.GetList();
        }
    }
}
