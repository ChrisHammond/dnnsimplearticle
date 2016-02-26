﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticleView.ascx.cs" Inherits="Christoc.Modules.dnnsimplearticle.Controls.ArticleView" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>

<h1><asp:Panel ID="plArticleTitle" runat="server" CssClass="Head" /></h1>
<asp:Panel runat="server" ID="pnlAuthorInfo"></asp:Panel>
<asp:Panel ID="plArticleBody" runat="server" CssClass="Normal ArticleBody" />


<div class="ArticleTags" id="ArticleTags" runat="server">
    <asp:label id="lblTags" runat="server" resourcekey="lblTags" />
    <dnn:tags id="tagsControl" runat="server" />
</div>

<div class="ArticleAdmin" runat="server" id="ArticleAdmin">
    <asp:LinkButton id="lnkEdit" runat="server" resourcekey="EditArticle" 
        onclick="lnkEdit_Click" />
    <asp:LinkButton ID="lnkDelete" runat="server" ResourceKey="DeleteArticle" 
        onclick="lnkDelete_Click" />
</div>