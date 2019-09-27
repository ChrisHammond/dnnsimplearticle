<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlexibleList.ascx.cs"
    Inherits="Christoc.Modules.dnnsimplearticle.Controls.FlexibleList" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<div class="row" runat="server" id="FlexibleListWrapper">
    <asp:PlaceHolder ID="phFlexibleList" runat="server" />
</div>
<asp:Panel ID="pnlPaging" runat="server">
    <asp:HyperLink ID="lnkPrevious" runat="server" resourcekey="lnkPrevious" Visible="false"
        CssClass="lnkPrevious"></asp:HyperLink>
    <asp:HyperLink ID="lnkNext" runat="server" resourcekey="lnkNext" Visible="false"
        CssClass="lnkNext"></asp:HyperLink>
</asp:Panel>
