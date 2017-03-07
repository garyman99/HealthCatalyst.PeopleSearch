var PeopleSearchModuleFactory = (function($) {

    var createModule = (function (moduleContainerId) {
        var root = location.protocol + '//' + location.host;
        var apiUrl = root + "/api/Person/";

        var searchEventBroker = function() {
            var handlers = [];

            var publishSearch = function (searchText, byInterest) {
                handlers.forEach(function(handler) {
                    handler.call(window, searchText, byInterest);
                });
            }
            
            return {
                listenForSearch: function(callback) {
                    handlers.push(callback);
                },
                search: publishSearch
            }
        }();

        var uiElements = function(uiSearchEventBroker) {

            // collect elements
            var moduleContainer = $("#" + moduleContainerId);
            if (moduleContainer === null || moduleContainer === undefined || moduleContainer.length === 0) {
                var errorMessage = "Unable to instantiate people search module. " +
                    " Module container could not be found for the following ID: " +
                    moduleContainerId;
                throw errorMessage;
            }

            var formElement = moduleContainer.find("form");
            var loadingElement = $("#ps-loading");
            var searchBox = $("#ps-searchText");
            var searchByInterest = $("#ps-searchByInterest");
            var searchButton = $("#ps-searchButton");
            var searchResultsContainer = $("#ps-searchResults");
            var searchResultTemplate = $("#ps-searchResultTemplate");

            var privateUpdateResults = function(results) {
                searchResultsContainer.empty();
                if (results !== null && results !== undefined && results.length !== 0) {
                    for (var i = 0; i < results.length; i++) {

                        var newResult = searchResultTemplate.clone();
                        newResult.removeClass("hide");
                        var profileImage = newResult.find(".ps-result-profileImage");
                        profileImage.attr("src", "data:" + results[i].Image);
                        profileImage.parent("a").attr("href", apiUrl + "/" + results[i].Id);
                        newResult.find(".ps-result-name").text(results[i].Name);
                        newResult.find(".ps-result-dob").text(results[i].DateOfBirth);

                        var interestsContainer = newResult.find(".ps-interests-container");
                        if (results[i].Interests !== null && results[i].Interests !== undefined) {
                            for (var j = 0; j < results[i].Interests.length; j++) {

                                var searchUrl = root +
                                    "/PersonSearch/Index/?searchText=" +
                                    results[i].Interests[j] +
                                    "&byInterest=true";
                                interestsContainer
                                    .append($("<li/>")
                                        .addClass("list-group-item ps-interest")
                                        .append($("<a/>")
                                            .attr("href", searchUrl)
                                            .text(results[i].Interests[j])));
                            }
                        }

                        searchResultsContainer.append(newResult);
                    }
                } else {
                    searchResultsContainer.append($("<div/>").addClass("container").text("No results to display"));
                }
                loadingElement.hide();
            }

            var privateHandleError = function (errorData) {
                loadingElement.hide();
                searchResultsContainer.empty();
                searchResultsContainer.text(errorData);
            }

            // set up click binding
            formElement.submit(
                function () {
                    loadingElement.show();
                    uiSearchEventBroker.search(searchBox.val(), searchByInterest.is(':checked'));
                    return false;
                });
            searchButton.on("click",
                function () {
                    loadingElement.show();
                    uiSearchEventBroker.search(searchBox.val(), searchByInterest.is(':checked'));
                });

            return {
                handleSearchResults: privateUpdateResults,
                handleError: privateHandleError,
        }
        }(searchEventBroker);

        var successHandler = function (data) {
            var dataObject = JSON.parse(data);
            uiElements.handleSearchResults(dataObject);
        }
        var errorHandler = function(data) {
            uiElements.handleError(data);
        }

        var apiService = (function () {
            var privateSearch = function (searchText, byInterest) {

                var searchData = {
                    searchString: searchText,
                    byInterest: byInterest
                }
                $.ajax({
                    url: apiUrl,
                    data: searchData,
                    dataType: "json",
                    type: "GET",
                    success: successHandler,
                    error: errorHandler
                });
            }

            return {
                search: privateSearch
            }
        })();

        searchEventBroker.listenForSearch(apiService.search);
    });

    return {
        create: createModule
    }
})(jQuery);


