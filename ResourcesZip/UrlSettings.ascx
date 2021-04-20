<%@ Control Language="C#" AutoEventWireup="True" Inherits="Christoc.Modules.dnnsimplearticle.UrlSettings"
    CodeBehind="UrlSettings.ascx.cs" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
test
	<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("ArticleUrlProviderSettings")%></a></h2>
	<fieldset>
        <div class="dnnFormItem dnnClear">
            <dnn:label ID="lblHideArticlePagePath" runat="server" ResourceKey="HideArticlePagePath"/>
            <asp:CheckBox ID="chkHideArticlePagePath" runat="server" />
        </div>

        <div class="dnnFormItem dnnClear">
            <dnn:Label ID="lblDateFormat" runat="server" ResourceKey="DateFormat" /> 
             <asp:TextBox ID="txtDateFormat" runat="server" />
        </div>

        <div class="dnnFormItem dnnClear">
            <dnn:label id="lblArticlePage" runat="server" controlname="cboArticlePage" ResourceKey="ArticlePage" />
            <dnn:DnnPageDropDownList ID="cboArticlePage" runat="server" />
        </div>

    </fieldset>