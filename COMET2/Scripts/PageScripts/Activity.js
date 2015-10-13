google.load("visualization", "1.0", { 'packages': ['corechart'] });
//google.setOnLoadCallback(getUsageByWeeksJson);

$(document).ready(function () {
    getUsageByWeeksJson();
 
    loadEnd();
});



function openMenu(option) {
    
}



// ***************** Creates Graph ************************ //
// Calls: AJAX(getUsageByWeeks)
// Returns: 
// Sets: 
function CreateUsageGraph(usageArray) {
    var colorsArray = ["#acacac"
            , "#34C6CD"
            , "#FFB473"
            , "#2A4380"
            , "#FFD173"
            , "#0199AB"
            , "#FF7600"
            , "#123EAB"
            , "#FFAB00"
            , "#1A7074"
            , "#A64D00"
            , "#466FD5"
            , "#BE008A"
            , "#B4F200"
            , "#DF38B1"
            , "#EF002A"
            ];
    var data = new google.visualization.DataTable();
    
    data.addColumn("string", "Week Start");
    var frequency = 2 * Math.PI / usageArray[0].length;
    // create the headings columns and colors array dynamically
    for (var i = 1; i < usageArray[0].length; i++) {
        var red = Math.sin(i * frequency + 0) * 127 + 128;
        var green = Math.sin(i * frequency + 2) * 127 + 128;
        var blue = Math.sin(i * frequency + 4) * 127 + 128;
        data.addColumn("number", usageArray[0][i]);
        colorsArray.push(RGB2Color(red, green, blue));
    }

    // add the data
    //data.addColumn({ type: "number", role: "annotation" });
    for (var i = 1; i < usageArray.length; i++) 
        data.addRows([usageArray[i]]);

    var options = {
        backgroundColor: "#efeeef",
        title: "Distinct Logins a day by group by week",
        titlePosition: "out",
        colors: colorsArray,
        lineWidth: 3,
        width: "100%",
        height: "100%",
        curveType: "function",
        explorer: {},
        hAxis: {
            textPosition: "none",
            slantedText: true,
            slantedTextAngle: 60
            //gridlines: {
            //    color: "#111",
            //    count: 5//data.getNumberOfRows()
            //}
        },
        legend: {
            position: 'top',
            textStyle: { fontSize: 13 }
        },
        chartArea: {
            left: 60,
            top: 70,
            width: "100%",
            height: "90%"
        },
        animation: {
            duration: 1000,
            easing: "out"
        },
        //selectionMode: "multiple",
        vAxis: {
            title: "Distinct Users"
        }
    };
    
    var chart = new google.visualization.LineChart(document.getElementById("usageByTime"));
    //google.visualization.events.addListener(chart, "select", function () {
    //    var arr = new Array();
    //    $(".grid").find("td").each(function () {
    //        arr.push($(this).attr("id"));
    //        $(this).removeClass("graphRowHover");
    //    });
    //    alert(arr);
    //    var selectedItem = chart.getSelection()[0];
    //    //alert("#cell" + selectedItem.column + "," + (selectedItem.row + 1));
    //    $("#cell" + selectedItem.column + "," + (selectedItem.row + 1)).addClass("graphRowHover");
    //});
    chart.draw(data, options);

    // create Data Table
    var table = document.createElement("table");
    $(table).addClass("grid");
    for (var i = 0; i < usageArray[1].length; i++) {
        var row = document.createElement("tr");
        for (var j = 0; j < usageArray.length; j++) {
            var cell = document.createElement("td");
            if (i == 0)
                cell = document.createElement("th");

                

            // start Formatting
            $(cell).addClass("graphCellHover");
            $(cell).hover(function () {
                $(this).closest("tr").addClass("graphRowHover");
            }, function () {
                $(this).closest("tr").removeClass("graphRowHover");
            });
            // end formatting

            cell.appendChild(document.createTextNode(usageArray[j][i]));
            // if heading, allow filter
            if (j === 0) {
                var userList = UsersByGroup(usageArray[j][i]);
                $(cell).css("width", "100px");
                
                if (i !== 0) $(cell).addClass("showing").addClass("rowHeader").attr("id", "t" + i).attr("title", "Hide/Show on graph \n\n" + 
                        UsersByGroup(usageArray[j][i])
                    );
                (function (d, opt, n) {                     // the onclick function
                    $(cell).click(function () {
                        view = new google.visualization.DataView(d);
                        if ($(this).hasClass("showing")) {
                            $(this).closest("tr").find("td").each(function () {
                                $(this).removeClass("showing").removeClass("graphCellHover").addClass("grapCellhHoverNotShow");
                            });
                        } else {
                            $(this).closest("tr").find("td").each(function () {
                                $(this).removeClass("grapCellhHoverNotShow").addClass("showing").addClass("graphCellHover");
                            });
                        }

                        var showColumnsArray = new Array();
                        // toggle
                        $(".rowHeader").each(function () {
                            if (!$(this).hasClass("showing"))
                                showColumnsArray.push(parseInt($(this).attr("id").substr(1, $(this).attr("id").length - 1)));
                            //view.hideRows([parseInt($(this).attr("id").substr(1, $(this).attr("id").length - 1))]);
                        });

                        view.hideColumns(showColumnsArray);
                        chart.draw(view, opt);
                    });
                })(data, options, i);
            } else {
                $(cell).addClass("cellNumberTight");
                if (i > 0) {
                    $(cell).attr("id", "cell" + i + "," + j);
                    $(cell).mouseover(function () {
                        var selectItem = chart.series;
                        var split = $(this).attr("id").substr(4, $(this).attr("id").length - 4).split(",");
                    });
                }
            }
            row.appendChild(cell);
        }
        table.appendChild(row);
    }
    $("#graphDetails").html(table);
        
    // show the graph in full-screen mode
    $("#showFullScreen").on("click", function (e) {
        $("#usageByTime").removeClass("graph").css({
                "position": "fixed",
                "left": 0,
                "top": 0,
                "z-index": 5,
                "width": "100%",
                "height": "100%",
                "margin": 0
        });

        $(this).css("z-index", 1);
        
        // create the Div for closing the full-screen
        var closeDiv = document.createElement("div");
        $(closeDiv).addClass("float-right-absolute").click(function () { 
            $("#usageByTime").css({
                "position": "",
                "left": "",
                "top": "",
                "z-index": "",
                "width": "",
                "height": "",
                "margin": ""
            }).addClass("graph");
            $("#closeButtonDiv").remove();
            chart.draw(data, options);
            $("#showFullScreen").css("z-index", 10);
        }).css({
            "min-width": "20px",
            "min-height": "20px",
            "background-image": "url(../Images/x-circle.png)",
            "background-size": "cover",
            "cursor": "pointer"
        }).attr("id", "closeButtonDiv");

        closeDiv.appendChild(document.createTextNode("  "));
        document.body.appendChild(closeDiv);
        chart.draw(data, options);
    });
}





// ***************** returns object of ActivityByWeeks ************************ //
// Calls: AJAX(getUsageByWeeks)
// Returns: 
// Sets: 
function getUsageByWeeksJson() {
    var usageWeeks = new Array();
    var usageData = new Array();
    var usageArray = new Array();
    setTimeout(function () {
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: "GetUsageByWeeks",
            async: false,
            success: function (data) {
                /*
	                column 1 = Week Start Date
	                column 2-n  = Department Name
                */
                var weekList = new Array();
                var deptArray = new Array();
                var usageArray = new Array();

                deptArray.push("Week Start");
                // create an Array of weeks to check off of
                for (var i = 0; i < data.length; i++) {
                    if (jQuery.inArray(data[i].WeekStart, weekList) === -1)
                        weekList.push(data[i].WeekStart);
                    if (jQuery.inArray(data[i].Group, deptArray) === -1)
                        deptArray.push(data[i].Group);
                }
                
                var newDeptArray = deptArray.slice(0);
                usageArray.push(newDeptArray);
                deptArray.shift();
                for (var i = 0; i < weekList.length; i++) {
                    var tempArray = new Array();
                    tempArray.push(weekList[i]);
                    for (var j = 0; j < deptArray.length; j++) 
                        tempArray.push(data[i * deptArray.length + j].DistinctLogins);
                    usageArray.push(tempArray);
                }
                
                CreateUsageGraph(usageArray);
            },
            error: function (request, status, error) {
                var Insert = new ErrorInsert();
                Insert.PageName = "Index";
                Insert.StepName = "ajaxGetUsageByWeeks";
                Insert.ErrorDetails = request.responseText + " -- " + error;
                if (request.readyState == 4) {
                    Insert.ErrorCode = "c-g-j-ageubw-01";
                }   // end if readyState 4
            }   // end error
        });
    }, 5);
}




// ***************** returns string of Users ************************ //
// Calls: AJAX(UsersInGroup)
// Returns: 
// Sets: 
function UsersByGroup(group) {
    var users = ""

    $.ajax({
        type: "POST",
        data: { "userGroup": group },
        url: "GetUsersInGroup",
        async: false,
        success: function (data) {
            for (var i = 0; i < data.length; i++)
                users += data[i] + "\n";
        },
        error: function (request, status, error) {
            var Insert = new ErrorInsert();
            Insert.PageName = "Index";
            Insert.StepName = "ajaxGetUsageByWeeks";
            Insert.ErrorDetails = request.responseText + " -- " + error;
            if (request.readyState == 4) {
                Insert.ErrorCode = "c-g-j-ageubw-01";
            }   // end if readyState 4
        }   // end error
    });

    return users;
}






// ***************** creates dynamic color scheme for legend ************************ //
// Calls: 
// Returns: 
// Sets: 
function RGB2Color(r, g, b) {
    return '#' + byte2Hex(r) + byte2Hex(g) + byte2Hex(b);
}
function byte2Hex(n) {
    var nybHexString = "0123456789ABCDEF";
    return String(nybHexString.substr((n >> 4) & 0x0F, 1)) + nybHexString.substr(n & 0x0F, 1);
}






// ***************** creates expand and collapse on pivot ************************ //
// Calls: 
// Returns: 
// Sets: 
function expandCollapse(group, focus, vendor) {
    setTimeout(loadStart(), 500);
    setTimeout(function () {
        if (focus.src.match("expand")) {
            focus.src = "../../Images/collapse.gif";
            $("#" + group + " > tbody > tr").each(function () {
                if ($(this).attr('id') === vendor)
                    $(this).show();
            }); // end foreach
        } else {
            focus.src = "../../Images/expand.gif";
            $("#" + group + " > tbody > tr").each(function () {
                if ($(this).attr('id') === vendor)
                    $(this).hide();
            }); // end foreach
        }   // end if
    }, 1000);
    setTimeout(loadEnd(), 10000);
}