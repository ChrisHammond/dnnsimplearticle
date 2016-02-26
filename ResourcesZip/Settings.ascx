<%@ Control Language="C#" AutoEventWireup="false" Inherits="Christoc.Modules.dnnsimplearticle.Settings"
    CodeBehind="Settings.ascx.cs" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead">
    <a href="" class="dnnSectionExpanded">
        <%=LocalizeString("BasicSettings")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label ID="lblPageSize" runat="server" ControlName="txtPageSize" />
        <asp:TextBox ID="txtPageSize" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblShowCategories" runat="server" ControlName="chkShowCategories">
        </dnn:label>
        <asp:CheckBox ID="chkShowCategories" runat="server" />
    </div>
</fieldset>
<h2 id="H1" class="dnnFormSectionHead">
    <a href="" class="dnnSectionExpanded">
        <%=LocalizeString("AdvancedSettings")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label ID="lblFullArticleRss" runat="server" ControlName="chkFullArticleRss" />
        <asp:CheckBox ID="chkFullArticleRss" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblCleanRss" runat="server" ControlName="chkCleanRss"></dnn:label>
        <asp:CheckBox ID="chkCleanRss" runat="server" />
    </div>
</fieldset>
<h2 id="H2" class="dnnFormSectionHead">
    <a href="" class="dnnSectionExpanded">
        <%=LocalizeString("AdminTools")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label ID="lblDelete" runat="server" ControlName="lnkDeleteAll" />
        <asp:LinkButton ID="lnkDeleteAll" runat="server" resourcekey="lnkDeleteAll" OnClick="lnkDeleteAll_Click" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblRemoveSearchIndex" runat="server" ControlName="lnkRemoveSearchIndex" />
        <asp:LinkButton ID="lnkRemoveSearchIndex" runat="server" resourcekey="lnkRemoveSearchIndex"
            OnClick="lnkRemoveSearchIndex_Click" />
    </div>
</fieldset>
