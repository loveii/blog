using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loveii.Models;
using Loveii.Repositories;
using Loveii.Helpers;

namespace Loveii.Services
{
    public class UserSrv
    {
        public static TResult<User> Get(int id)
        {
            return CreateRepository.User.Get(id);
        }
         
        public static Result Add(User model)
        {
            Result rseult = CreateRepository.User.IsExist(model.userName);
            if (rseult.Successed)
                return new Result(false, "用户已存在");

            model.password = SecurityHelper.SHA1(model.password).ToLower();
            return CreateRepository.User.Add(model);
        }
    }
}
