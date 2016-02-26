/* coming soon, this isn't currently used and doesn't currently work 
    stay tuned for Version 2.5

*/

function Article(a, selectedArticle) {
    this.id = a.ArticleId;
    this.Title = a.Title;
    this.Description = a.Description;
    this.Body = a.Body;
    this.url = a.url;
}

var viewModel = {
    articles: ko.observableArray([])
};

$(function () {
    /* Initialize AJAX so we can simplify individual calls */
    var sf = $.ServicesFramework(414); //production id needs a module id here afaik
    //var sf = $.ServicesFramework(381); //development id
    sf.getAntiForgeryProperty();
    var mappedFromServer;
    var data = {};

    data.portalId = 0; // development/production id
    data.sortAsc = true;
    $.ajax({
        type: "POST",
        cache: false,
        //don't forget about siteurls.config replacement used below
        url: '/desktopmodules/DnnSimpleArticle/API/DnnSimpleArticle.ashx/GetAllArticles',
        data: data
    }).done(function (data) {

        viewModel.articles = ko.utils.arrayMap(data.Articles, function (article) {
            return new Article(article);
        });
        ko.applyBindings(viewModel);
    }).fail(function () {
        alert('Sorry failed to load articles');
    });
});

/*

sample HTML

<!-- ko foreach: articles -->
        <div>
            <h1 data-bind="html:Title">
            </h1>
            <div data-bind="html:Body">
            </div>
        </div>
        <!-- /ko -->


*/