<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Loveii.ViewModels.HomeViewModel>" %>
<div class="loveii-header">
    <div class="loveii-inner-w">
    <form action="/cate/index" method="get">
        <div class="loveii-search">
            <input type="submit" class="btn" value="<%:Model.s %>"" />
            <input type="text" class="input" name="s" value="" placeholder="搜索博客"/>
        </div>
    </form>
      
  <a class="loveii-logo" title="返回首页" href="/"></a>
	<div class="loveii-nav-wraper">
        <ul class="loveii-nav"> 
          <%foreach (var item in Model.TermList)
            { %>
            <li><a  href="/cate/<%=item.slug%>"><%=item.name%></a></li>
          <%} %> 
        </ul>
	</div>
    </div>
</div>