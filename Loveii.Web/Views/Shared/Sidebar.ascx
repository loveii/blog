<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Loveii.ViewModels.HomeViewModel>" %>
<div class="loveii-sidebar">
<%--    <h2 class="loveii-sidebar-title">关于我们</h2>
    <div>
        介绍<a href="http://blog.aliyun.com">more »</a>
        <div><a href="http://blog.aliyun.com/feed"><img src="/css/img/loveii-ico-rss.png" alt=""/></a></div>
    </div>--%>
    <h2 class="loveii-sidebar-title">文章分类</h2>
        <ul> 
          <%foreach (var item in Model.TermList)
            { %>
            <li><a  href="/cate/<%=item.slug%>"><%=item.name%></a></li>
          <%} %> 
        </ul>
    <!--<h2 class="loveii-sidebar-title">热门活动</h2>
    <div><a href=""><img src="/img/loveii-img01.png" alt=""/></a></div>-->
    <div id="meta-2" class="widget widget_meta">
        <h2>功能</h2>			
        <ul>
			<li><a href="/register">注册</a></li>			
            <li><a href="/Admin/Login">登录</a></li> 				
        </ul>
    </div>		
    <div id="recent-posts-2" class="widget widget_recent_entries">		
        <h2>近期文章</h2>		
        <ul>
           <%foreach (var item in Model.NewsPostList)
            { %>
            <li><a href="/<%=item.id%>.html"><%=item.title%></a></li>
          <%} %>  
        </ul>
	</div>
    <div id="recent-posts-3" class="widget widget_recent_entries">		
        <h2>友情连接</h2>		
        <ul>
           <%foreach (var item in Model.LinkList)
            { %>
            <li><a href="<%=item.url%>" target="_blank" ><%=item.name%></a></li>
          <%} %>  
        </ul>
	</div>
</div>