<%@ Control Language="C#" Inherits="Christoc.Modules.dnnsimplearticle.Edit" AutoEventWireup="True"
    CodeBehind="Edit.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnndep" Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" %>


<div class="dnnForm dnnSimpleArticleSettings dnnClear" id="dnnSimpleArticleSettings">
    <div class="dnnFormExpandContent"><a href=""><%=LocalizeString("ExpandAll")%></a></div>

    <h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead">
        <a href="" class="dnnSectionExpanded">
            <%=LocalizeString("BasicSettings")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label ID="lblTitle" ControlName="txtTitle" runat="server" />
            <asp:TextBox ID="txtTitle" runat="server" Columns="50" /><asp:RequiredFieldValidator
                ID="rfvTitle" runat="server" ControlToValidate="txtTitle" CssClass="NormalRed" />
        </div>
    </fieldset>
    <h2 id="H1" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("Description")%></a></h2>
    <fieldset>

        <div class="dnnFormItem">
             <dnn:label ID="lblPermaLink" ControlName="txtPermaLink" runat="server" />
            <asp:TextBox ID="txtPermaLink" runat="server" Columns="50" /><asp:RequiredFieldValidator
                ID="rfvPermaLink" runat="server" ControlToValidate="txtPermaLink" CssClass="NormalRed" />
        </div>

        <div class="dnnFormItem">

            <dnn:TextEditor ID="txtDescription" runat="server" Width="100%" Height="300px" />
            <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                CssClass="NormalRed" />
        </div>


        <div class="dnnFormItem">
            <asp:UpdatePanel ID="pnlImage" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dnn:label ID="lblImage" runat="server" ControlName="urlImage" />
                    <dnn:URL ID="urlImage" runat="server" Width="325"
                        ShowFiles="true"
                        ShowUrls="false"
                        ShowTabs="false"
                        ShowLog="false"
                        ShowTrack="false"
                        Required="False"
                        ShowNewWindow="False" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>


    </fieldset>

    <h2 id="H2" class="dnnFormSectionHead">
        <a href="" class="dnnSectionExpanded">
            <%=LocalizeString("ArticleBody")%></a></h2>
    <fieldset>

        <div class="dnnFormItem">
            <dnn:TextEditor ID="txtBody" runat="server" Width="100%" Height="400px" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblTerms" runat="server" ControlName="tsTerms" />
            <dnndep:TermsSelector ID="tsTerms" runat="server" Height="250" Width="600" AllowCustomText="true" />
        </div>


    </fieldset>

    <div class="dnnFormItem">
        <asp:LinkButton ID="lbSave" runat="server" resourcekey="lbSave" OnClick="LbSaveClick" CssClass="dnnPrimaryAction" />
        <asp:LinkButton ID="lbCancel" runat="server" resourcekey="lbCancel" OnClick="LbCancelClick"
            CausesValidation="false" CssClass="dnnSecondaryAction" />
    </div>

</div>

<script language="javascript" type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($, Sys) {
        function dnnSimpleArticleSettings() {
            $('#dnnSimpleArticleSettings').dnnPanels();
            $('#dnnSimpleArticleSettings .dnnFormExpandContent a').dnnExpandAll({ expandText: '<%=Localization.GetString("ExpandAll", LocalResourceFile)%>', collapseText: '<%=Localization.GetString("CollapseAll", LocalResourceFile)%>', targetArea: '#dnnSimpleArticleSettings' });
        }

        $(document).ready(function () {
            dnnSimpleArticleSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                dnnSimpleArticleSettings();
            });
        });

    }(jQuery, window.Sys));
</script>
