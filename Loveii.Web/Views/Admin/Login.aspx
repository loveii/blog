<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Loveii.AdminBaseViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <title>Login</title>
</head>
<body>
    <div>
        <form action="/Admin/Login" method="post">
        <p><span>用户名：</span><input type="text" id="userName" name="userName" /></p>
        <p><span>密　码：</span><input type="password" id="password" name="password" /></p>
        <p><input type="submit" value="登录" /></p>
        <p><span><%=Model.GetItem("Message")%></span></p>
        </form>
    </div>
</body>
</html>
