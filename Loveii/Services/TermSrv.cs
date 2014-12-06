using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loveii.Models;
using Loveii.Repositories;

namespace Loveii.Services
{
    public class TermSrv
    {
        public static List<Term> GetList()
        {
            return CreateRepository.Term.GetList();
        }

        public static TResult<Term> Get(int id)
        {
            return CreateRepository.Term.Get(id);
        }

        public static TResult<Term> Get(string slug)
        {
            return CreateRepository.Term.Get(slug);
        }

        public static Result Add(Term model)
        {
            return CreateRepository.Term.Add(model);
        }

        public static Result Modify(Term model)
        {
            return CreateRepository.Term.Modify(model);
        }
    }
}
