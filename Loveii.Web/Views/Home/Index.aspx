<%@ Page  Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Loveii.ViewModels.HomeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">begga blog</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HeaderContent" runat="server">
  <%Html.RenderPartial("~/Views/Shared/Header.ascx", Model);%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarContent" runat="server">
  <%Html.RenderPartial("~/Views/Shared/Sidebar.ascx",Model);%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="loveii-page"> 
    <%if (!string.IsNullOrEmpty(Model.s))
      {%>
    <div class="loveii-search-keyword">
    <span>关键字: ‘<%:Model.s%>’</span>的搜索结果
    </div>
    <%} %>
    <%foreach (var item in Model.PostList.Item)
      {%>
    <div class="loveii-article-list">
        <div class="loveii-article-header">
            <div class="fn-clear">
            <span class="tag"><%=item.Term.name %></span>
            <h2 class="title">
            <a href="/<%=item.id %>.html"><%=item.title %></a>
            </h2>
            </div>
            <div class="info">
            <a href="/author/<%=item.uid %>" rel="author"><%=item.User.niceName %></a>
            发表于<%=item.createTime.ToString("yyyy年MM月dd日") %> |  阅读(<%=item.click%> ) <a href=""><a href="/<%=item.id %>.html#respond" title="<%=item.title %>" >评论 (<%=item.commentCount %>)</a></a>
            </div>
        </div>
        <div class="loveii-article-con">
            <p><%=item.excerpt %><a href="/<%=item.uid %>.html" class="more-link">阅读全文&#8230;</a></p>
        </div>
    </div>
     <%  } %>
 
    <div class="loveii-paging">
        <%if (string.IsNullOrEmpty(Model.s))
          {%>
        <%=Html.Pagination(Model.PostList, "/cate/" + Model.cate, "")%>
      <%}
          else
          { %>
        <%=Html.Pagination(Model.PostList, "/cate/" + Model.cate, "?s="+Model.s)%>
    <%} %>
    </div>
</div>
</asp:Content>
