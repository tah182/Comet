function Comet() {
    this.nameCtrl = null;
    this.LAST_PAGE_MOVE = new Date().getTime();
    this.LAST_REQUEST_CHECK_TIME = new Date().getTime();
    this.PAGE_REFRESH_TIME = 3600000;
    this.REFRESH_REQUESTS_TIME = 60000;

    updateRefreshTime = function() {
        this.LAST_PAGE_MOVE = new Date().getTime();
    };

    this.updateRequestCheckTime = function() {
        this.LAST_REQUEST_CHECK_TIME = new Date().getTime();
    };
}

function ErrorInsert(pageName, stepName, errorCode, errorDetails) {
    this.PageName       = pageName;
    this.StepName       = stepName;
    this.ErrorCode      = null;
    this.ErrorDetails   = null;

    if (errorCode !== null && typeof errorCode === "string")
        ErrorCode = errorCode;
    if (errorDetails !== null && typeof errorDetails === "string")
        ErrorDetails = errorDetails;

    this.logError = function (location) {
        runAjax(ajax("ErrorInsert", null, this, "Grid").InsertError);
        console.log("Unimplemented: Log error: " + this.ErrorDetails);
        $(location).html(ErrorInsert.ErrorDetails);
    }
}

var Global = {
    PAGE_NAME: "Global"
}

Comet.prototype = {
    setNameCtrl: function(ctrl) {
        this.nameCtrl = ctrl;
    },
    TODAY: function() {
        var today = new Date();
        today.setDate(today.getDate());
        return today;
    },
    NOW: function() {
        return new Date().getTime();
    },
    setRefresh: function() {
        var that = this;
        var method = function() { that.setRefresh(); };
        if (this.NOW() - this.LAST_PAGE_MOVE >= 21600000)
            window.location.reload(true);
        else
            setTimeout(method, this.PAGE_REFRESH_TIME);
    },
    refreshData: function() {
        runAjax(ajaxCall(
            ajaxDetails().HttpType.POST,
            "/Rest/RefreshAppData",
            null,
            ajaxDetails().Synchronous.TRUE,
            ajaxDetails().ContentType.JSON,
            function() {
                loadStart();
            },
            function(result) {
                alert(result);
                this.updateRefreshTime();
                location.reload();
            },
            function (request, status, error) {
                
            }));
    },
    getNewRequests: function() {
        var that = this;

        var method = function() { that.getNewRequests(); };
        if (this.NOW() - this.LAST_REQUEST_CHECK_TIME >= this.REFRESH_REQUESTS_TIME) {
            runAjax(ajaxCall(
                    ajaxDetails().HttpType.POST,
                    "/Rest/Requests",
                    null,
                    ajaxDetails().Synchronous.TRUE,
                    ajaxDetails().ContentType.DEFAULT,
                    ajaxDetails().nullBeforeSend,
                    function(result) {
                        if (result.length > 0) {
                            if ($("#consoleUnread").length === 0) {
                                var notification = document.createElement("div");
                                $(notification)
                                    .attr("id", "consoleUnread")
                                    .addClass("notification");
                                CorrectNotification();
                                $("#menuDiv").append(notification);
                            }
                            $("#consoleUnread").html(result.length);
                        } else {
                            if ($("#consoleUnread") !== null && typeof $("#consoleUnread") !== "undefined")
                                $("#consoleUnread").remove();
                        }
                    },
                    function(request, status, error) {
                        //alert(error);
                    })
            );
            this.updateRequestCheckTime();
        }
        setTimeout(method, this.REFRESH_REQUESTS_TIME);
    },
    CorrectNotification: function() {
        $("#consoleUnread").css({
            "left": $("#consoleAnchor").offset().left + $("#consoleAnchor").width() - 15,
            "top": $("#consoleAnchor").offset().top - 8,
        });
    }
};

var userTimer = null
function showUserDetails(element) {
    var $this = $(element);
    if ($(element).data("events") === null || typeof ($(element).data("events") === "undefined")) {
        $(element).bind({
            mouseleave: function() {
                clearTimeout(userTimer);
                $("#" + $this.prop("id") + "User").css("visibility", "hidden");
            }
        });
    }
    userTimer = setTimeout(function() {
        $("#" + $this.prop("id") + "User").css("visibility", "visible");
    }, 500);
}



/**
 * Page Refresh Embed
 */
$(document).ready(function() {
    var comet = new Comet();
    comet.setRefresh();
    comet.LAST_REQUEST_CHECK_TIME = comet.LAST_REQUEST_CHECK_TIME - comet.REFRESH_REQUESTS_TIME;
    comet.getNewRequests();
    $(window).resize(comet.CorrectNotification);

    var ver = getInternetExplorerVersion();
    if(ver > -1 && ver < 10.0)
        alert("Your are using an outdated version of IE. Comet recommends that you use IE10, Chrome, or Firefox.");

    //$('ul#menu').jqsimplemenu();

    $("div#body").scroll(function () {
        var scroll = $("div#body").scrollTop();
        if (scroll > 0) 
            $("header").addClass("scroll");
        else 
            $("header").removeClass("scroll");
    });
    
    if ($("#sawNewVersion").html().toLowerCase() === "false") {
        $.ajax({
            type: "GET",
            url: "/Rest/ShowNav",
            async: true,
            success: function(data){
                $("#showNav").html(data);
            },
            error: function(request, status, error) {
                var e = request.responseText + " --- " + error;
                var insert = new ErrorInsert(Global.PAGE_NAME, "showNav", "g-r-sn-01", e);
                insert.logError();
            }   // end error
        });
        $("#showNav").css("display", "block");
    }
    
    $("a#refresh").click(function() {
        comet.refreshData();
    });

    // attach a datepicker to date inputs 
    $("input[type='date']").each(function() {
        if (this.type !== "date") {
            $.datepicker.formatDate("MM/dd/yyyy");
            $(this).datepicker({
                buttonImage: "calendar.gif",
                buttonText: "Pick a date",
                beforeShowDay: $.datepicker.noWeekends
            });
        }
    });
});

function closeNav(neverShowAgain) {
    if (neverShowAgain) {
        $.ajax({
            type: "GET",
            url: "/Rest/MarkNewVersionAcknowledgement",
            async: true,
            error: function (request, status, error) {
                var Insert = new ErrorInsert();
                Insert.PageName = "Index";
                Insert.StepName = "ajaxCloseNav";
                Insert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState == 4) {
                    Insert.ErrorCode = "c-g-j-cn-01";
                }   // end if readyState 4
            }   // end error
        });
    }
    $("#showNav").fadeOut("slow");
}

/**
 * Returns Capitalized names without all Upper
 */ 
function toTitleCase(str) {
    return str.replace(/\w\S*/g, function(txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
}

var dimensions = [];
function minMaxParent(id) {
    if ($(id).hasClass("minmaxOpen")) {
        $(id).parent().children().each(function() {
            if ($(this).attr("id") !== $(id).attr("id"))
                $(this).fadeOut(200);
        });

        $(id).parent().parent().children().each(function () {
            $(this).animate(
            { "height": 0 }, 300);
        });

        // check if Dimensions has been stored already
        var foundID = false;
        $.each(dimensions, function(index, value) {
            if ($(id).attr("id") === value.id)
                foundID = true;
        });
        if (!foundID) {
            dimensions.push({
                "id": $(id).attr("id"),
                "width": $(id).parent().css("width"),
                "height": $(id).parent().css("height"),
                "padding": $(id).parent().css("padding")
            });
        }
        $(id).parent().animate({
            "width": "15px",
            "height": "15px",
            "padding": "0px",
            "border-radius": "7px"
        }, 500);

        $(id).removeClass("minmaxOpen").addClass("minmaxClose");
    } else {                        // if opening back up
        $.each(dimensions, function(index, value) {
            if ($(id).attr("id") === value.id) {
                $(id).parent().animate({
                    "width": value.width,
                    "height": value.height,
                    "padding": value.padding
                }, 400, function() {
                    $(this).css({
                        "height": "auto",
                        "border-radius": "0" //value.borderRadius
                    });
                });
            }
        });

        $(id).parent().find(".content").each(function() {
            if(this.hasChildNodes())
                $(this).parent().fadeIn(900);
        });

        $(id).parent().parent().children().each(function () {
            $(this).animate(
            { "height": "auto" }, 300);
        });
        $(id).removeClass("minmaxClose").addClass("minmaxOpen");
    }
}

// Loading Functions
function loadEnd() {
    $("#loadImage").stop().animate({
        opacity: 0
    }, 1000);
    $(".loading").stop().animate({
        opacity: 0
    }, 800);
    setTimeout(function() {
        $(".loading").css("z-index", "-10");
        $("#loadImage").css("z-index", "-10");
    }, 1800);
}
function loadStart() {
    $(".loading").css("z-index", "99");
    $("#loadImage").css("z-index", "120");
    setTimeout(function() {
        $(".loading").stop().animate({
            opacity: 0.8,
            "background-color": "#000"
        }, 200);
    }, 0);
    setTimeout(function() {
        $("#loadImage").stop().animate({
            opacity: 1
        }, 200);
    }, 0);
}

function getInternetExplorerVersion() {
    // Returns the version of Internet Explorer or a -1
    // (indicating the use of another browser).

    var rv = -1; // Return value assumes failure.
    if (navigator.appName === "Microsoft Internet Explorer") {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) !== null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}

function supportsHtml5Storage() {
    try {
        return 'localStorage' in window && window.localStorage !== null;
    } catch (e) {
        return false;
    }
}

function createStorageExpiration() {
    var expiration = new Date();
    expiration.setDate(expiration.getDate() + 1);
    return expiration.setHours(8, 0, 0, 0);
}

function trimFuncName(func) {
    var name = func.substr('function '.length);
    name = name.substr(0, name.indexOf('('));
    return name;
}

function ajaxDetails() {
    return {
        ContentType: {
            DEFAULT: "application/x-www-form-urlencoded; charset=UTF-8",
            NULL: null,
            JSON: "application/json; charset=utf-8",
            TEXT: "text/plain",
            HTML: "html"
        },
        HttpType: {
            GET: "GET",
            POST: "POST"
        },
        Synchronous: {
            TRUE: true,
            FALSE: false
        }
    };
}

//----------------------------------------- Refactored Ajax Call methods --------------------------------------
function ajaxCall(
            httpType,
            queryString,
            data,
            synchronous,
            contentType,
            beforeSendDelegate,
            handleSuccessDelegate,
            handleFailureDelegate) {

    if (hasValidParams === false)
        return null;

    var _queryString = queryString;
    var _data = data;
    var _httpType = httpType;
    var _contentType = contentType;
    var _beforeSendDelagate = beforeSendDelegate;
    var _handleSuccessDelegate = handleSuccessDelegate;
    var _handleFailureDelegate = handleFailureDelegate;

    var hasValidParams = function() {
        if (typeof queryString !== "string")
            return false;

        if (typeof httpType !== "string")
            return false;

        if (typeof contentType !== "string" || contentType === null)
            return false;

        if (typeof data === "object" && data !== null)
            if (Array.isArray(data) !== true)
                return false;

        if ((typeof handleSuccessDelegate === "object" && handleSuccessDelegate !== null) === false)
            return false;

        if ((typeof handleFailureDelegate === "object" && handleFailureDelegate !== null) === false)
            return false;

        if ((typeof synchronous !== "boolean"))
            return false;

        return true;
    };

    return {
        QueryString: _queryString,
        Data: _data,
        HttpType: _httpType,
        ContentType: _contentType,
        Synchronous: synchronous,
        BeforeSendDelegate: function() { return(_beforeSendDelagate === null || typeof _beforeSendDelagate === "undefined") ? null : _beforeSendDelagate(); },
        SuccessDelegate: function(data) { return _handleSuccessDelegate(data); },
        FailureDelegate: function(request, status, error) { return _handleFailureDelegate(request, status, error); }
    };
}


function runAjax(ajaxCallInstance) {
    if (typeof ajaxCallInstance === "object" && ajaxCallInstance !== null) {
        if (ajaxCallInstance.ContentType !== null && ajaxCallInstance.Data !== null)
            $.ajax({
                type: ajaxCallInstance.HttpType,
                contentType: ajaxCallInstance.ContentType,
                data: ajaxCallInstance.Data,
                url: ajaxCallInstance.QueryString,
                async: ajaxCallInstance.Synchronous,
                //xhrFields:      { widthCredentials: true },
                beforeSend: function() {
                    ajaxCallInstance.BeforeSendDelegate();
                },
                success: function(data) {
                    ajaxCallInstance.SuccessDelegate(data);
                },
                error: function(request, status, error) {
                    ajaxCallInstance.FailureDelegate(request, status, error);
                }
            });
        else
            $.ajax({
                type: ajaxCallInstance.HttpType,
                url: ajaxCallInstance.QueryString,
                async: ajaxCallInstance.Synchronous,
                beforeSend: function() {
                    ajaxCallInstance.BeforeSendDelegate();
                },
                success: function(data) {
                    ajaxCallInstance.SuccessDelegate(data);
                },
                error: function(request, status, error) {
                    ajaxCallInstance.FailureDelegate(request, status, error);
                }
            });
    }
}

function Ajax(funcName, updateLocation, data, references) {
    "use strict";
    var nullData, nullContentType, nullBeforeSend;
    nullData = nullContentType = nullBeforeSend = null;
    data = (data !== null && (typeof data === "object" || typeof data === "string")) ? data : nullData;

    if (updateLocation !== null && typeof updateLocation === "string")
        errorPanel = updateLocation;
    return {
        NewRequests: new ajaxCall(ajaxDetails().HttpType.GET,
            "Rest/Requests",
            data,
            ajaxDetails().Synchronous.TRUE,
            ajaxDetails().ContentType.JSON,
            nullBeforeSend,
            function(result) {
                $("#" + updateLocation).html(result.length);
            },
            function(request, status, error) {
            }),
        InsertError: new ajaxCall(ajaxDetails().HttpType.POST,
            "Rest/JSError",
            data,
            ajaxDetails().Synchronous.FALSE,
            ajaxDetails().ContentType.JSON,
            nullBeforeSend,
            function() {

            },
            function(request, status, error) {
            })
    };
}