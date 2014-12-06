<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Loveii.ViewModels.AdminViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Index</title>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <script type="text/javascript" charset="utf-8" src="/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/ueditor/ueditor.all.min.js"> </script>
    <script type="text/javascript" charset="utf-8" src="/ueditor/lang/zh-cn/zh-cn.js"></script>
</head>
<body>
    <div>
        <p><%=Html.DropDownListFor(model => Model.termId, Model.TermSelectList)%></p>
        <%foreach (var item in Model.PostList.Item)
          {%>
          <ul>
            <li><a href="/Admin/Modify/<%=item.id %>" >编辑</a><span>[<%=item.id %></span>]<a href="/<%=item.id %>.html" target="_blank" ><%=item.title %></a></li>
            </ul>
        <% } %>
    </div>
    <script src="/js/jquery.js" type="text/javascript"></script>
    <div class="loveii-paging">
        <%=Html.Pagination(Model.PostList, "/Admin/Index/?cateId=" + Model.cateId)%>
    </div>
</body>
<script src="/js/jquery.js" type="text/javascript"></script>
<script type="text/javascript">
    $("#termId").change(function () {
        var usr = "/Admin/Index/?termId=" + $("#termId").val() + ""; 
        window.location.href = usr;
    });
</script>
</html>
