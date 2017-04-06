$(document).ready(function () {

    DisplayExistingConditions();

    $("a.add-condition-group").click(function () {
        AddConditionGroup();
    });

    $(".k-grid-toolbar a.k-grid-delete").livequery("click", function () {
        var grid = $(this).closest(".condition-group-grid");
        if (grid != undefined) {
            var conditionGroupId = $(grid).attr("data-conditionGroupId");
            var gridId = $(grid).attr("id");

            if (conditionGroupId != undefined && gridId != undefined) {

                DeleteConditionGroup(conditionGroupId, gridId);
            }
        }
    });

});

$("#editor").livequery(function () {
    AddViewModel();
});

$(window).unload(function () {  
    var entityId = $.getHiddenValFromDom("#RibbonGeneralInformation_Id");

    if (entityId != undefined && entityId != 0) {
        var deleteUnusedConditionGroupsUrl = $.getHiddenValFromDom("#delete-unused-condition-groups-url");
        var conditionId = $.getHiddenValFromDom("#condition-id");
        var parameters = { "conditionId": conditionId };

        $.ajax({
            cache: false, async: false, type: "POST", data: $.toJSON(parameters),
            contentType: "application/json; charset=utf-8", url: deleteUnusedConditionGroupsUrl, success: function () {
                return;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Deleting condition failed.");
            }
        });
    }
});

function DisplayExistingConditions() {
    var entityId = $.getHiddenValFromDom("#RibbonGeneralInformation_Id");

    if (!entityId || entityId == 0) {
        return;
    }

    var conditionGroupIds = $.parseJSON($.getHiddenValFromDom("#condition-groups"));
    if (conditionGroupIds != null) {
        for (var i = 0; i < conditionGroupIds.length; i++) {
            AddConditionGroup(conditionGroupIds[i]);
        }
    }
}

function AddConditionGroup(conditionGroupId) {

    if (conditionGroupId && conditionGroupId != 0) {
        AddConditionGroupGridHtml(conditionGroupId);
    } else {
        conditionGroupId = CreateConditionGroupAndAddConditionGroupGridHtml();
        if (conditionGroupId == "") {
            return;
        }
    }

    addKendoGridForConditionGroup(conditionGroupId);
}

function addKendoGridForConditionGroup(conditionGroupId) {
    var conditionGroupGridId = "condition-group-grid-" + conditionGroupId;

    var readConditionGroupUrl = $.getHiddenValFromDom("#read-condition-group-url");
    var updateConditionGroupUrl = $.getHiddenValFromDom("#update-condition-statement-url");
    var destroyConditioGroupUrl = $.getHiddenValFromDom("#destroy-condition-statement-url");
    var createConditionUrl = $.getHiddenValFromDom("#create-condition-statement-url");

    var popupValues = {};

    var conditionStatementId = 0;

    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: readConditionGroupUrl,
                dataType: "json",
                contentType: "application/json"
            },
            update: {
                url: updateConditionGroupUrl,
                type: "POST",
                dataType: "json",
                contentType: "application/json"
            },
            destroy: {
                url: destroyConditioGroupUrl,
                dataType: "json",
                contentType: "application/json"
            },
            create: {
                url: createConditionUrl,
                type: "POST",
                dataType: "json",
                contentType: "application/json"
            },
            parameterMap: function (options, operation) {
                if (operation === "update" || operation === "create") {
                    return kendo.stringify(popupValues);
                }
                if (operation === "read") {
                    return { conditionGroupId: kendo.stringify(conditionGroupId) };
                }
                if (operation === "destroy") {
                    return { conditionStatementId: kendo.stringify(conditionStatementId) };
                }
            }
        },
        batch: true,
        pageSize: 30,
        schema: {
            model: {
                id: "Id",
                fields: {
                    Type: { type: "text" },
                    OperatorType: { type: "text" },
                    Text: { type: "text" },
                    Value: { type: "text" }
                }
            }
        }
    });

    $("#" + conditionGroupGridId).kendoGrid({
        dataSource: dataSource,
        sortable: true,
        editable: {
            mode: "popup",
            template: kendo.template($("#popup_editor").html())
        },
        edit: function (e) {
            var model = e.model;

            if (model.Type != undefined && model.OperatorType != undefined && model.Value != undefined) {
                $("#condition-type").attr("data-editValue", model.Type);
                $("#condition-operator").attr("data-editValue", model.OperatorType);
                $("#condition-value").attr("data-editValue", model.Value);
                $("#condition-num-value").attr("data-editValue", model.Value);
            }
        },
        save: function (e) {
            popupValues.ConditionGroupId = conditionGroupId;
            popupValues.Type = $("#condition-type").val();
            popupValues.OperatorType = $("#condition-operator").val();

            var valueDropdownDisplay = $("#condition-value").parent().css("display");
            if (valueDropdownDisplay && valueDropdownDisplay == "none") {
                popupValues.Value = $("#condition-num-value").val();
            } else {
                popupValues.Value = $("#condition-value").val();
            }

            // if this is true, then we are editing a row and not creating a new one
            if (e.model.Type != undefined) {
                var data = dataSource.data();
                for (var i = 0; i < data.length; i++) {
                    if (data[i].Id == e.model.Id) {
                        popupValues.Id = e.model.Id;
                        data[i].dirty = true;
                    }
                }
            }
        },
        remove: function (e) {
            if (e.model.Id != undefined && e.model.Id != "") {
                var data = dataSource.data();
                for (var i = 0; i < data.length; i++) {
                    if (data[i].Id == e.model.Id) {
                        conditionStatementId = e.model.Id;
                        data[i].dirty = true;
                    }
                }
            }
        },
        toolbar: ["create", "destroy"],
        columns: [
            {
                field: "Id",
                title: "Id",
                width: 100,
                hidden: true
            },
            {
                field: "Type",
                title: "Type",
                width: 100
            },
            {
                field: "OperatorType",
                title: "OperatorType",
                width: 100

            },
            {
                field: "Text",
                title: "Text",
                width: 250
            },
             {
                 field: "Value",
                 title: "Value",
                 width: 200,
                 hidden: true
             },
             { command: ["edit", "destroy"], title: "&nbsp;", width: "150px" }
        ]
    });
}

function DeleteConditionGroup(conditionGroupId, gridId) {
    var deleteConditionGroupUrl = $.getHiddenValFromDom("#delete-condition-group-url");

    var parameters = { "conditionGroupId": conditionGroupId };

    $.ajax({
        cache: false, type: "POST", data: $.toJSON(parameters),
        contentType: "application/json; charset=utf-8", url: deleteConditionGroupUrl, success: function () {

            $("#" + gridId).data("kendoGrid").destroy();
            $("#" + gridId).parent().remove();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Deleting condition group failed.");
        }
    });

    return;
}

function AddViewModel() {
    var viewModel = kendo.observable({
        typeSource: GetConditionTypes(),
        selectedType: null,
        selectedOperator: null,
        selectedValue: null
    });

    viewModel.selectedType = SetSelectedType(viewModel.typeSource);
    viewModel.selectedOperator = function () {
        var operator = $("#condition-operator").attr("data-editValue");
        if (operator != undefined && operator != "") {
            return operator;
        }

        return null;
    };

    viewModel.selectedValue = function () {
        var valueDropdownDisplay = $("#condition-value").parent().css("display");
        var value = $("#condition-value").attr("data-editValue");

        if (valueDropdownDisplay && valueDropdownDisplay == "none") {
            value = $("#condition-num-value").attr("data-editValue");
            if (value != undefined && value != "") {
                return value;
            }
        } else {
            if (value != undefined && value != "") {
                return value;
            }
        }
        return null;
    };

    kendo.bind($("#editor"), viewModel);
}

function SetSelectedType(typeSource) {
    var type = $("#condition-type").attr("data-editValue");

    if (type != undefined && type != "") {
        for (var i = 0; i < typeSource.length; i++) {
            if (typeSource[i].Type == type) {
                return typeSource[i];
            }
        }
    }
    return null;
}

function GetConditionTypes() {
    var getConditionTypesUrl = $.getHiddenValFromDom("#get-condition-type-url");

    var conditionTypes;
    $.ajax({
        cache: false, async: false, type: "GET",
        contentType: "application/json; charset=utf-8", url: getConditionTypesUrl, success: function (data) {

            conditionTypes = data;

        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Retrieving condition types failed.");
        }
    });

    return conditionTypes;
}

function CreateConditionGroupAndAddConditionGroupGridHtml() {

    var createConditionGroupUrl = $.getHiddenValFromDom("#create-condition-group-url");
    var conditionId = $.getHiddenValFromDom("#condition-id");

    if (createConditionGroupUrl == "" || conditionId == "") {
        return 0;
    }

    var parameters = { "conditionId": conditionId };

    var conditionGroupId = "";

    $.ajax({
        cache: false, async: false, type: "POST", data: $.toJSON(parameters),
        contentType: "application/json; charset=utf-8", url: createConditionGroupUrl, success: function (data) {
            conditionGroupId = data;
            AddConditionGroupGridHtml(conditionGroupId);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Creating new condition group failed.");
        }
    });

    return conditionGroupId;
}

function AddConditionGroupGridHtml(conditionGroupId) {
    var conditionGroupGridClass = "condition-group-grid";
    var conditionGroupGridId = "condition-group-grid-" + conditionGroupId;

    var conditionGroupGridHtml = "<div><div class=\"" + conditionGroupGridClass + "\" id=\"" + conditionGroupGridId + "\" data-conditionGroupId=\"" + conditionGroupId + "\"></div><div class=\"group-dependancy-text\"><p>--OR--</p></div></div>";
    $("#condition-groups").append(conditionGroupGridHtml);
}