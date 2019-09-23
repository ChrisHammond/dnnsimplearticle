<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlexibleList.ascx.cs"
    Inherits="Christoc.Modules.dnnsimplearticle.Controls.FlexibleList" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>

<asp:Repeater ID="rptArticleList" runat="server" OnItemDataBound="RptArticleListOnItemDataBound"
    OnItemCommand="RptArticleListOnItemCommand">
    <ItemTemplate>
        <asp:Panel CssClass="ArticleWrapper" runat="server">
            <asp:Panel runat="server" ID="pnlArticleTitle" CssClass="ArticleTitle">
                <h2><asp:HyperLink ID="lnkArticle" runat="server" CssClass="SubHead" NavigateUrl='<%# GetArticleLink(DataBinder.Eval(Container.DataItem,"ArticleId").ToString())%>'><%# System.Web.HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem,"Title").ToString()) %></asp:HyperLink></h2>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlArticleDescription" CssClass="Normal ArticleDescription">
                <%# HttpUtility.HtmlDecode(DataBinder.Eval(Container.DataItem, "Description").ToString())%>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlOtherControls" CssClass="Normal ArticleOther">
                <asp:Label ID="lblTags" runat="server" resourcekey="lblTags" />
                <dnn:Tags ID="tagsControl" runat="server" />
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlAdminControls" CssClass="Normal ArticleAdmin" Visible="false">
                <asp:LinkButton ID="lnkEdit" runat="server" ResourceKey="EditArticle.Text" CommandName="Edit"
                    Visible="false" Enabled="false" CssClass="dnnPrimaryAction" />
                <asp:LinkButton ID="lnkDelete" runat="server" ResourceKey="DeleteArticle.Text" CommandName="Delete"
                    Visible="false" Enabled="false" CssClass="dnnSecondaryAction" />
            </asp:Panel>
        </asp:Panel>
    </ItemTemplate>
</asp:Repeater>
<asp:Panel ID="pnlPaging" runat="server">
    <asp:HyperLink ID="lnkPrevious" runat="server" resourcekey="lnkPrevious" Visible="false"
        CssClass="lnkPrevious"></asp:HyperLink>
    <asp:HyperLink ID="lnkNext" runat="server" resourcekey="lnkNext" Visible="false"
        CssClass="lnkNext"></asp:HyperLink>
</asp:Panel>
