<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Loveii.ViewModels.HomeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"><%=Model.Post.title %></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HeaderContent" runat="server">
  <%Html.RenderPartial("~/Views/Shared/Header.ascx", Model);%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarContent" runat="server">
  <%Html.RenderPartial("~/Views/Shared/Sidebar.ascx",Model);%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="loveii-page"> 
    <div class="loveii-article">
        <div class="loveii-article-header">
          <div class="fn-clear"><span class="tag">产品更新</span> 
            <h2 class="title"><a href=""><%=Model.Post.title %></a></h2></div>
            <div class="info"><a href=""><a href="#"  rel="author"><%=Model.Post.User.userName %></a></a> 发表于<%=Model.Post.User.createTime.ToString() %> | 阅读(<%=Model.Post.click %>) <a href="#respond">评论 </a>(<%=Model.Post.commentCount %>)</div>
        </div>
        <div class="loveii-article-con">
            <%=Model.Post.content %>
        </div>
    <%if (Model.Post.nextId > 0)
      {%>
    <div class="loveii-article-next">
          <div>下一文章</div>
          <a href="/<%=Model.Post.nextId %>.html" rel="next"><%=Model.Post.nextTitle %></a>        
    </div>
    <%} %>
</div>
<%if (Model.Post.commentStatus == "open")
  {%>
<div class="loveii-comment">
    <div class="loveii-comment-title">
        <span class="comment-total"><em class="fc-red"><%=Model.Post.commentCount %></em>条评论</span>
    </div> 
    <ul class="loveii-comment-list">           
    <%foreach (var item in Model.CommentList)
      { %> 
        <li>
            <a class="img"><img alt="" src="/css/img/loveii-noface.gif" /></a> 
            <div class="user"><%=item.author%></div>
            <div class="con"><p><%=item.content%></p></div>
            <div class="info"><span class="date"><%=item.createTime.ToString()%></span> </div>
        </li>
     <% } %>     
    </ul> 
    <div id="respond" class="loveii-comment-submit">
    <a href="" class="img"></a>  
    <textarea id="comment" name="comment" cols="30" rows="10" class="input"></textarea> 
    <div class="fn-clear"><input  type="button" class="submit fn-right" onclick="comment();" value="发布" /></div>
    </div> 
    <input type="hidden" id="postId" name="postId" value="<%=Model.Post.id %>"/> 
</div>
<%} %> 
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="JSContent" runat="server">
<script type="text/javascript">
    function comment() {
        var postId = $("#postId").val(); 
        var comment = $("#comment").val();
        $.ajax({
            url: "/Comment/Add",
            type: "post",
            data: {
                postId:postId, 
                content: comment
            },
            success: function (data) {
                if (data.Successed) {
                    window.location.reload();
                }
                else {
                    alert(data.Message);
                }
            }
        });
    }  
</script>
</asp:Content>