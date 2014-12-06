<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Loveii.ViewModels.AdminViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8"/>
    <script type="text/javascript" charset="utf-8" src="/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/ueditor/ueditor.all.min.js"> </script>
    <script type="text/javascript" charset="utf-8" src="/ueditor/lang/zh-cn/zh-cn.js"></script>
    <style type="text/css">
        #title,#excerpt{width: 300px;} 
    </style>
</head>
<body>
<div>  
    <p>分类ID：<%=Html.DropDownListFor(model => Model.termId, Model.TermSelectList)%></p>  
    <p>标　题：<input type="text" id="title" name="title"  value="" /></p>
    <p>简　介：<input type="text" id="excerpt" name="excerpt"  value="" /></p>
    <p><script id="editor" type="text/plain" style="width:1024px;height:500px;"></script></p>
    <p><input type="button" name="submit"  onclick="submit()" value="提交" /></p>
</div>
<script src="/js/jquery.js" type="text/javascript"></script>
<script type="text/javascript">
    var ue = UE.getEditor('editor'); 
    function submit() { 
        var termId = $("#termId").val();
        var title = $("#title").val();
        var excerpt = $("#excerpt").val();
        var content = UE.getEditor('editor').getContent();

        if (!termId) {
            alert("请选择分类！");
            return false;
        }
        else if (termId < 1) {
            alert("分类ID错误！");
            return false;
        }

        if (!title) {
            alert("标题不能为空！");
            return false;
        }
        else if (title.length > 32) {
            alert("长度不能大于32个字符！");
            return false;
        }

        if (!excerpt) {
            alert("简介不能为空！");
            return false;
        }
        else if (title.length > 255) {
            alert("长度不能大于255个字符！");
            return false;
        }

        if (!UE.getEditor('editor').hasContents()) {
            alert("文章不能为空！");
            return false;
        }

        $.ajax({
            url: "/Admin/Add",
            type: "post",
            data: {
                termId: termId,
                title: title,
                excerpt: excerpt,
                content: content
            },
            success: function (data) {
                if (data.Successed) {
                    alert(data.Message); 
                } 
            }
        });
    } 
</script>
</body>
</html>
