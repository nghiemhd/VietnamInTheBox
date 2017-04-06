/// <reference path="../../../presentation/nop.web/scripts/jquery-1.7.1.js" />

var ribbonType = "";
var editRibbonPictureButtonElement = "";
var pageRibbonTableSelector = "table.page-ribbon";

$(document).ready(function () {

    addRibbonPicturesGrid();

    $(pageRibbonTableSelector).each(function () {
        var ribbonImageLabel = $(this).find(".ribbon-image label");

        if (ribbonImageLabel != undefined && ribbonImageLabel.length > 0) {
            $(this).find(".remove-ribbon-picture").css("display", "none");
        }
    });

    // When clicked on this button a popup with all of the ribbon pictures will be shown
    $("tr.ribbon-picture-buttons .edit-ribbon-picture").click(function () {
        var editPictureUrl = getEditPictureUrl(this);

        if (editPictureUrl == "") {
            return;
        }

        editRibbonPictureButtonElement = this;

        $(editRibbonPictureButtonElement).parents(".page-ribbon-settings").each(function (index, value) {
            var rType = $(value).attr("data-ribbontype");

            if (rType != undefined && rType != "") {
                ribbonType = rType;

            }
        });

        addAndDisplayRibbonPicturesGrid();
    });

    // When clicked on this button the ribbon picture will be removed
    $("tr.ribbon-picture-buttons .remove-ribbon-picture").on("click", function () {
        var ribbonPicture = $(this).closest("table.page-ribbon").find("td.ribbon-image");

        if (ribbonPicture == undefined) {
            return;
        }

        var image = $(ribbonPicture).find("img");

        if (image == undefined) {
            return;
        }

        setRibbonType(this);

        removePictureId();
        removeRibbonPictureFromDom(ribbonPicture);

        // Hide the button
        $(this).css("display", "none");
    });

    $(".manage-ribbon-pictures").click(function () {
        window.location.href = getAddDeleteProductRibbonPictureUrl(this);
    });
});

function setRibbonType(ribbonImageButton) {
    $(ribbonImageButton).parents(".page-ribbon-settings").each(function (index, value) {
        var rType = $(value).attr("data-ribbontype");

        if (rType != undefined && rType != "") {
            ribbonType = rType;

        }
    });
}

function removeRibbonPictureFromDom(ribbonPicture) {
    var noPictureLabelText = getNoPlictureLableText();

    if (noPictureLabelText == "") {
        return;
    }

    var noPictureLabelHtml = "<label>" + noPictureLabelText + "</label>";

    $(ribbonPicture).html(noPictureLabelHtml);
}

function removePictureId() {
    if (ribbonType == "category" && $("#CategoryPageRibbon_PictureId") != undefined) {
        $("#CategoryPageRibbon_PictureId").attr("value", 0);
    }
    if (ribbonType == "product" && $("#ProductPageRibbon_PictureId") != undefined) {
        $("#ProductPageRibbon_PictureId").attr("value", 0);
    }
}

function addAndDisplayRibbonPicturesGrid() {
    var kendoWindow = $(".ribbonPicturesWindow").data("kendoWindow");

    if (kendoWindow == undefined) {
        populateWindowPictureGrid();

        var kwindow = $(".ribbonPicturesWindow");
        kwindow.kendoWindow({
            width: 650,
            modal: true,
            animation: false
        });

        kendoWindow = $(".ribbonPicturesWindow").data("kendoWindow");
    }

    kendoWindow.center();

    $(".ribbonPicturesWindow").closest(".k-window").css("top", "70px");

    kendoWindow.open();
}


function addRibbonPicturesGrid() {
    var categoryPageProductsPanelSelector = "body";

    var ribbonPictureGridHtml = "<div class=\"ribbonPicturesWindow\"><div id=\"ribbon-pictures-grid\"></div></div>";

    $(categoryPageProductsPanelSelector).prepend(ribbonPictureGridHtml);
}

function populateWindowPictureGrid() {
    var setRibbonPictureUrl = getSetRibbonPictureUrl();

    if (setRibbonPictureUrl == "") {
        return;
    }

    $("#ribbon-pictures-grid").kendoGrid({
        dataSource: new kendo.data.DataSource({
            transport: {
                read: {
                    url: setRibbonPictureUrl,
                    dataType: "json",
                    type: "POST"
                }
            },
            schema: {
                data: "Data", // records are returned in the "Data" field of the response
                total: "Total" // total number of records is in the "Total" field of the response
            },
            pageSize: 10,
            serverPaging: true
        }),
        selectable: "single",
        pageable: true,
        change: function () {

            var dataItem = this.dataItem(this.select());
            var id = dataItem.Id;
            if (!id) {
                return;
            }
            var picture = dataItem.PictureUrl;
            addPictureIdToTheMarkup(id, picture);

        },
        columns: [{
            field: "Id",
            filterable: false
        },
            {
                field: "PictureUrl",
                template: "<img src='${ PictureUrl }' alt='${ Id }' />",
                width: 250
            },
            {
                field: "PictureHeight",
                filterable: true
            },
            {
                field: "PictureWidth",
                filterable: true
            }

        ]
    });

    $("#ribbon-pictures-grid").data("kendoGrid").hideColumn("Id");
}

function addPictureIdToTheMarkup(pictureId, picture) {
    if (ribbonType == "category" && $("#CategoryPageRibbon_PictureId") != undefined) {
        setPictureId("#CategoryPageRibbon_PictureId", pictureId, picture);
    }
    if (ribbonType == "product" && $("#ProductPageRibbon_PictureId") != undefined) {
        setPictureId("#ProductPageRibbon_PictureId", pictureId, picture);
    }
}

function setPictureId(pictureIdElement, pictureId, picture) {
    $(pictureIdElement).attr("value", pictureId);

    $(".ribbonPicturesWindow").data("kendoWindow").close();

    addPictureToDom(picture, pictureId);
    showDeleteRibbonPictureButton();
}

function showDeleteRibbonPictureButton() {
    var deleteRibbonPictureButton = $(editRibbonPictureButtonElement).closest("table.page-ribbon").find(".remove-ribbon-picture");

    if (deleteRibbonPictureButton != undefined) {
        $(deleteRibbonPictureButton).css("display", "inline-block");
    }
}

function addPictureToDom(picture, pictureId) {
    var editRibbonPictureButton = $(editRibbonPictureButtonElement);

    if (editRibbonPictureButton == undefined) {
        return;
    }

    var ribbonImageElement = editRibbonPictureButton.closest("table.page-ribbon").find(".ribbon-image");

    if (ribbonImageElement == undefined) {
        return;
    }

    var pictureHtml = "<img src=" + picture + " alt=" + pictureId + " />";

    ribbonImageElement.html(pictureHtml);
}

function getRibbonPictureId(clickedElement) {
    var id = $(clickedElement).attr("data-ribbonPictureId");

    if (id == undefined || id == "") {
        return 0;
    }

    return parseInt(id) || 0;
}

function getSetRibbonPictureUrl() {
    var url = $("#ribbon-data").attr("data-setRibbonPictureUrl");

    if (url == undefined) {
        return "";
    }

    return url;
}

function getNoPlictureLableText() {
    var text = $("#ribbon-data").attr("data-noPictureLabelText");

    if (text == undefined) {
        return "";
    }

    return text;
}

function getAddDeleteProductRibbonPictureUrl(button) {
    var url = $(button).attr("data-addDeleteProductRibbonPictureUrl");

    if (url == undefined) {
        return "";
    }

    return url;
}

function getEditPictureUrl(editRibbonPictureInputSelector) {
    var editPictureUrl = $(editRibbonPictureInputSelector).attr("data-editPictureUrl");

    if (editPictureUrl == undefined || editPictureUrl == "")
        return "";

    return editPictureUrl;
}