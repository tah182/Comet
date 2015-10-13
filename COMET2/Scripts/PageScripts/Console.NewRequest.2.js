Number.prototype.mod = function (n) {
    return ((this % n) + n) % n;
};

Date.prototype.addBusDays = function (dd) {
    var wks = Math.floor(dd / 5);
    var dys = dd.mod(5);
    var dy = this.getDay();
    if (dy === 6 & dys > -1) {
        if (dys === 0) {
            dys -= 2;
            dy += 2;
        }
        dys++;
        dy -= 6;
    }
    if (dy === 0 && dys < 1) {
        if (dys === 0) {
            dys += 2;
            dy -= 2;
        }
        dys--;
        dy += 6;
    }
    if (dy + dys > 5) dys += 2;
    if (dy + dys < 1) dys -= 2;
    this.setDate(this.getDate() + wks * 7 + dys);
};

function Request(submittedBy) {
    this.SubmittedBy = submittedBy;
    this.RequestBy;
    this.SupportAreaID;
    this.TypeID;
    this.RequestCategory = 1;
    this.RequestedDueDate;
    this.RequestSummary;
    this.RequestDescription;
    this.ValueDriverID;
    this.Value;
    this.ValueReason;
};
        
Request.prototype = {
    changeLabelColor: function (element, highlight) {
        if (highlight)
            $("label[for=" + $(element).attr("id") + "]").removeClass("invalid");
        else
            $("label[for=" + $(element).attr("id") + "]").addClass("invalid");
    },
    validateDropDown: function (element) {
        var valid = $(element).val() >= 0;
        this.changeLabelColor(element, valid);

        return valid;
    },

    validateText: function (element, max) {
        var valid = $(element).val().length <= max && $(element).val().length > 0;
        this.changeLabelColor(element, valid);
        return valid;
    },

    setRequestor: function (element) {
        var num = parseFloat($(element).html());
        if (!isNaN(num) && isFinite(num)) {
            this.RequestBy = num;
            return true;
        }
        return false;
    },
    setRequestArea: function (element) {
        var valid = this.validateDropDown(element);
        if (valid)
            this.SupportAreaID = parseFloat($(element).val());
        return valid;
    },
    setRequestType: function (element) {
        var valid = this.validateDropDown(element);
        if (valid)
            this.TypeID = parseFloat($(element).val());
        return valid;
    },

    setRequestCategory: function (element) {
        if (element !== null && typeof element !== "undefined") {
            var valid = this.validateDropDown(element);
            if (valid)
                this.RequestCategory = parseFloat($(element).val());
            return valid;
        } 
        requestCategory = 1;
        return valid;
    },

    setRequestSummary: function (element) {
        var valid = this.validateText(element, 100);
        if (valid)
            this.RequestSummary = $(element).val();

        return valid;
    },

    setRequestDescription: function (element) {
        var valid = this.validateText(element, 1000);
        if (valid)
            this.RequestDescription = $(element).val();

        return valid;
    },

    setValueDriver: function (element, changed) {
        var valid = this.validateDropDown(element);
        var opt = parseFloat($(element).val());
        if (changed) {
            if (opt == 1) {
                var temp = $("#value");
                $("#value").parent().empty().append("$").append(temp);
                $("label[for=value]").text("Value*");
            }
            else {
                var temp = $("#value");
                $("#value").parent().empty().html(temp);
                var select = $("<select>").addClass("inline").prop("id", "valueHours");
                var valuesText = [
                    { "ID": 1, "Text": "Hours - One Time" },
                    { "ID": 2, "Text": "Hrs/Day" },
                    { "ID": 3, "Text": "Hrs/Week" },
                    { "ID": 4, "Text": "Hrs/Month" },
                    { "ID": 5, "Text": "Hrs/Year" }
                ];
                fillDropDown(select, valuesText);
                $("#value").parent().append(select);
                $("label[for=value]").text("Value");
            }
        }

        if (valid)
            this.ValueDriverID = opt;
        return valid;
    },

    setValue: function (element) {
        if ($(element).val() === "" || $(element).val() === "undefined") {
            if (this.ValueDriverID === 1) {
                this.changeLabelColor(element, false);
                return false;
            }
            this.changeLabelColor(element, true);
            this.Value = 0;
        }
        if (this.ValueDriverID !== 1) {
            var val = parseFloat($(element).val());
            var valType = parseInt($("#valueHours").val());
            switch (valType) {
                case 2: // Hours/day
                    this.Value = val * 5 * 52;
                    break;
                case 3: // Hours/wk
                    this.Value = val * 52;
                    break;
                case 4: // Hours/mth
                    this.Value = val * 12;
                    break;
                case 5: // Hours/year
                    this.Value = val;
                case 1: // Hours - One Time
                    this.Value = val;
            }
        } else
            this.Value = parseFloat($(element).val());

        return true;
    },

    setValueDescription: function (element) {
        var valid = this.validateText(element, 500);
        if (valid)
            this.ValueReason = $(element).val();

        return valid;
    },

    setDesiredDueDate: function (element) {
        var today = new Date();
        var date = $(element).val().split("-");
        var checkDate;
        if (date.length > 1)
            checkDate = new Date(date[0], date[1] - 1, date[2]);
        else {
            date = $(element).val().split("/");
            checkDate = new Date(date[2], date[0] - 1, date[1]);
        }
        today.setHours(0, 0, 0, 0);
        var valid = checkDate >= today;
        if (valid)
            this.RequestedDueDate = $(element).val() + " 00:00:00";
                
        this.changeLabelColor(element, valid);
        return valid;
    },

    submitForm: function () {
        var form = document.createElement("form");
        form.method = "POST";
        form.action = "SubmitRequest";

        $(form).append(createInput("number", "SubmittedBy", this.SubmittedBy));
        $(form).append(createInput("number", "RequestBy", this.RequestBy));
        $(form).append(createInput("number", "SupportAreaID", this.SupportAreaID));
        $(form).append(createInput("number", "TypeID", this.TypeID));
        $(form).append(createInput("number", "RequestCategory", this.RequestCategory));
        $(form).append(createInput("text", "RequestedDueDate", this.RequestedDueDate));
        $(form).append(createInput("text", "RequestSummary", this.RequestSummary));
        $(form).append(createInput("text", "RequestDescription", this.RequestDescription));
        $(form).append(createInput("number", "ValueDriverID", this.ValueDriverID));
        $(form).append(createInput("number", "Value", this.Value));
        $(form).append(createInput("text", "ValueReason", this.ValueReason));
        $(form).append($("<input>").attr("type", 'submit').val('Submit'));
                
        $("#errorDetails").css("display", "none").append(form);
        form.submit();
        function createInput(type, name, value) {
            return $("<input>").attr("type", type).attr("name", name).val(value);
        };
    }
}

function continueLoad() {
    var date = new Date();
    date.addBusDays(5);
    if (document.getElementById("dueDate").type !== "date")
        $("#dueDate").val((date.getMonth() < 10 ? "0" : "") + (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear());
    else
        $("#dueDate").val(date.toISOString().substring(0, 10));

    runAjax(indexAjax("ready", null, null, requestType).RequestAreas);
    runAjax(indexAjax("ready", null, { "activeOnly": true }, requestType).RequestTypes);
    runAjax(indexAjax("ready", null, null, valueDriver).ValueDrivers);
    if (isAdmin)
        runAjax(indexAjax("ready", null, { 'input': "" }, requestor).RequestorSearch);
    runAjax(indexAjax("ready", null, { "activeOnly": true }, requestCategory).RequestCategories);

    $("#requestor").bind("input", function () {
        $("label[for='requestor']").addClass("invalid");
        runAjax(indexAjax("ready", null, { 'input': $(this).val() }, $(this)).RequestorSearch);
    });
    $("#requestor").focus(function () { $(this).val(""); });
    
    $("#valueDriver").change(function (e) {
        request.setValueDriver($(this), true);
    });
    summaryCharLeft();
    loadEnd();
};

function submit() {
    function idName (id, name, message) {
        var id = id;
        var name = name;
        var message = message;
        return {
            ID: id,
            Name: name,
            Message: message
        };
    };
    var requestor =             idName("requestor", "Requestor", "invalid value");
    var requestArea =           idName("requestArea", "Request Area", "needs value");
    var requestType =           idName("requestType", "Request Type", "needs value");
    var requestCategory =       idName("requestCategory", "Request Category", "needs value");
    var requestSummary =        idName("requestSummary", "Request Summary", "must be between 1 and 100 characters");
    var requestDescription =    idName("requestDescription", "Description", "must be between 1 and 500 characters");
    var valueDriver =           idName("valueDriver", "Value Driver", "needs value");
    var valueDescription =      idName("valueDescription", "Value Description", "must be between 1 and 500 characters");
    var dueDate =               idName("dueDate", "Desired Due Date", "must be in the future");
    var value =                 idName("value", "Value", "is required when selecting financial as a Value Driver");

    var canSubmit = "";
    request.setRequestor($("#hiddenRequestor"));
    canSubmit += !$("label[for='requestor']").hasClass("invalid") ?                 "" : createClickDiv(requestor);
    canSubmit += request.setRequestArea($("#" + requestArea.ID)) ?                  "" : createClickDiv(requestArea);
    canSubmit += request.setRequestType($("#" + requestType.ID)) ?                  "" : createClickDiv(requestType);
    if (isAdmin) 
        canSubmit += request.setRequestCategory($("#" + requestCategory.ID)) ? "" : createClickDiv(requestCategory);
    else
        request.setRequestCategory(null);
    canSubmit += request.setRequestSummary($("#" + requestSummary.ID)) ?            "" : createClickDiv(requestSummary);
    canSubmit += request.setValueDriver($("#" + valueDriver.ID), false) ?           "" : createClickDiv(valueDriver);
    canSubmit += request.setValue($("#" + value.ID)) ?                              "" : createClickDiv(value);
    canSubmit += request.setRequestDescription($("#" + requestDescription.ID)) ?    "" : createClickDiv(requestDescription);
    canSubmit += request.setValueDescription($("#" + valueDescription.ID)) ?        "" : createClickDiv(valueDescription);
    canSubmit += request.setDesiredDueDate($("#" + dueDate.ID)) ?                   "" : createClickDiv(dueDate);
            
    if (canSubmit.length > 0) {
        var mesg = "Please correct the following fields and submit again. <ul>" + canSubmit + "</ul>";
        $("#errorDetails").html(mesg).removeClass("valid").addClass("invalid");
        //alert(mesg);
    } else {
        $("#submit").prop("disabled", true);
        request.submitForm();
    }

    function createClickDiv(requestArea) {
        return "<li><div onclick=\"setFocus('" + requestArea.ID + "')\" class='inline link'>" + requestArea.Name + "</div> - " + requestArea.Message + ".</li>";
    }
}
function setFocus(elementName) {
    $("#" + elementName).focus();
}

function fillDropDown(element, options) {
    var opt = document.createElement("option");
    opt.value = -1;
    opt.innerHTML = "-- Please Select --";
    $(element).append(opt);
    var exp = "<b>Explanation: </b><ul>";

    for (var i = 0; i < options.length; i++) {
        var opts = document.createElement("option");
        opts.value = parseInt(options[i].ID);
        opts.innerHTML = options[i].Text;
                
        $(element).append(opts);
        exp += "<li><i>" + options[i].Text + "</i> - " + options[i].Comment + "</li>";
    }
    var detail = $("<span class='tooltip' style='font-weight: normal'>");
    $(detail).html(exp + "</ul>");
    if ($("label[for='" + $(element).prop("id") + "']").parent().hasClass("tooltip")) 
        $("label[for='" + $(element).prop("id") + "']").parent().append(detail);
}

function summaryCharLeft() {
    var max = 100;
    var remainString = " characters remaining";

    $("#summaryRemaining").html(max + remainString);
    $("#requestSummary").keyup(function () {
        if (max < $(this).val().length)
            $("#summaryRemaining").html("too many characters").css("color", "red");
        else
            $("#summaryRemaining").html((max - $(this).val().length) + remainString).css("color", "black");
    });
}

function indexAjax(funcName, updateLocation, data, references) {
    "use strict";
    var mapControllerPath = "/NewRequest/";
    var nullData, nullContentType, nullBeforeSend;
    nullData = nullContentType = nullBeforeSend = null;
    data = (data !== null && (typeof data === "object" || typeof data === "string")) ? data : null;
                                    
    if (updateLocation !== null && typeof updateLocation === "string")
        errorPanel = updateLocation;

    function getExpiration() {
        var date = new Date();
        date.setDate(date.getDate() + 1)
        date.setHours(8, 0, 0, 0);
        return date;
    }

    function saveToLocalStorage(key, data) {
        if (typeof key !== "string" || typeof data !== "object")
            return null;

        localStorage.setItem(key, { "data": data, "expiration": getExpiration() });
    }

    return {
        RequestAreas: new ajaxCall(ajaxDetails().HttpType.GET,
                mapControllerPath + "RequestAreas",
                nullData,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                ajaxDetails().nullBeforeSend,
                function (result) {
                    fillDropDown("#requestArea", result);
                },
                function () {
                }),
        RequestTypes: new ajaxCall(ajaxDetails().HttpType.GET,
                mapControllerPath + "RequestTypes",
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                ajaxDetails().nullBeforeSend,
                function (result) {
                    fillDropDown("#requestType", result);
                },
                function (request, status, error) {
                }),
        RequestCategories: new ajaxCall(ajaxDetails().HttpType.GET,
                mapControllerPath + "RequestCategories",
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                ajaxDetails().nullBeforeSend,
                function (result) {
                    fillDropDown("#requestCategory", result);
                },
                function (request, status, error) {
                }),
        ValueDrivers: new ajaxCall(ajaxDetails().HttpType.GET,
                mapControllerPath + "ValueDrivers",
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                ajaxDetails().nullBeforeSend,
                function (result) {
                    fillDropDown("#valueDriver", result);
                },
                function (request, status, error) {
                }),
        RequestorSearch: new ajaxCall(ajaxDetails().HttpType.GET,
                mapControllerPath + "Employees",
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                ajaxDetails().nullBeforeSend,
                function (result) {
                    var results = [];
                    for (var i = 0; i < result.length; i++) {
                        results.push({
                            "label": result[i].Name,
                            "value": result[i].Name,
                            "id": result[i].EmployeeID
                        });
                    }

                    $(references).autocomplete({
                        source: results,
                        select: function (event, ui) {
                            $("#hiddenRequestor").html(ui.item.id);
                            $("label[for='requestor']").removeClass("invalid");
                            runAjax(indexAjax("ready", null, { 'employeeID' : ui.item.id }, requestor).EmployeeManager);
                        },
                        focus: function (event, ui) {
                            $(this).val(ui.item.label);
                        },
                        open: function () {
                            $(".ui-autocomplete").css({
                                "max-width": "320px",
                                "background-color": "#FFFFFF",
                                "font-size": "0.7em",
                                "max-height": "350px",
                                "overflow-y": "scroll",
                                "overflow-x": "hidden"
                            });
                            $(".ui-autocomplete li").css({
                                outline: "1px"
                            })
                        }
                    }).data("ui-autocomplete")._renderItem = function (ul, item) {
                        return $("<li>")
                            .data("ui-autocomplete-item", item)
                            .append($("<a>").html(item.label))
                            .appendTo(ul);
                    };
                },
                function (request, status, error) {
                            
                }),
        EmployeeManager: new ajaxCall(ajaxDetails().HttpType.GET,
                mapControllerPath + "EmployeeGroupManager",
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.DEFAULT,
                ajaxDetails().nullBeforeSend,
                function (result) {
                    $("#groupName").html(result.GroupName);
                    $("#groupManager").html(result.Manager);
                },
                function (request, status, error) {
                }),
        SubmitForm: new ajaxCall(ajaxDetails().HttpType.POST,
                "SubmitRequest",
                data,
                ajaxDetails().Synchronous.TRUE,
                ajaxDetails().ContentType.JSON,
                function () {
                    loadStart();
                },
                function (result) {
                    location.reload(true);
                },
                function (request, status, error) {
                })
    }
}