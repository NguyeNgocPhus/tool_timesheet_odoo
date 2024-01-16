$(function () {

    for (let i = 1; i <= 12; i++) {
        $('.months').append($('<option>', {
            value: i,
            text: `Tháng ${i}`
        }));
    };

    $('.months').on('change', function () {

        $('.submit_timesheet').html(` 👻 Tạo timesheet cho tháng ${this.value}👻 `);

    });

    $('.info_timesheet').hide();
    $('.tab_loaded').hide();
    let type = 0;
    let unit_amount = 8.5;
    let employee_id;
    let employeeName;
    let project_id;
    let projectName;
    let task_id;
    let month = 1;
    let rangeDay = "";
    // create timesheet
    $('.submit_timesheet').click(function () {
        $('.tab_loaded').show();
        $('.create_success').html("");

        let description = $('.description').val();
        let sessionId = $('.SessionId').val();
        let rangeDay = $('.dayofmonth').val();

        let data = {
            type: type,
            sessionId: sessionId,
            employeeId: employee_id,
            employeeName: employeeName,
            projectId: project_id,
            projectName: projectName,
            taskId: task_id,
            description: description,
            dateTimesheet: type === 0 ? month : rangeDay
        };
        console.log("data: ", data);
        $.ajax({
            type: 'POST',
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: `/home/create`,
            success: function (data, status, xhr) {
                $('.tab_loaded').hide();
                alert(`${data.successRecord} bản ghi được thêm thành công và ${data.errorRecord} bản ghi lỗi \nTruy cập "https://erp.thgdx.vn" để kiểm tra dữ liệu \nCảm ơn rất nhiều vì đã sử dụng phần mềm\n🌺🌺🌻🌻🌹🌹🌼🌼`)

            }
        });

    });

    // get user by session_id
    $('.getUser').click(function () {

        let sessionId = $('.SessionId').val();
        console.log("sessionId", sessionId);
        if (sessionId === null || sessionId === undefined || sessionId === "") {
            alert("vui lòng nhập thông tin session id 💩 💩");
            return;
        }
        // configuration websocket
        WebSocketHandle(sessionId);


        $.ajax({
            type: 'GET',
            url: `/home/user?sessionId=${sessionId}`,
            success: function (data, status, xhr) {
                //console.log("du lieu nam o day");
                console.log('data: ', data);
                $('.info_timesheet').show();
                $('.range_day').hide();

                $('.dayofmonth').datepicker({
                    startView: 0,
                    minViewMode: 0,
                    maxViewMode: 2,
                    multidate: true,
                    multidateSeparator: "-",
                    autoClose: true,
                    beforeShowDay: highlightRange,
                }).on("changeDate", function (event) {
                    var dates = event.dates,
                        elem = $('.dayofmonth');
                    if (elem.data("selecteddates") == dates.join(",")) return;
                    if (dates.length > 2) dates = dates.splice(dates.length - 1);
                    dates.sort(function (a, b) { return new Date(a).getTime() - new Date(b).getTime() });
                    elem.data("selecteddates", dates.join(",")).datepicker('setDates', dates);
                });

                function highlightRange(date) {
                    var selectedDates = $('.dayofmonth').datepicker('getDates');
                    if (selectedDates.length === 2 && date >= selectedDates[0] && date <= selectedDates[1]) {
                        return 'highlighted';
                    }
                    return '';
                }

                $('.users').select2();
                $.each(data, function (i, item) {
                    if (i === 0) {
                        employeeName = item.name;
                        employee_id = item.id;
                    }
                    $('.users').append($('<option>', {
                        value: item.id,
                        text: item.name
                    }));
                });
            }
        });

        $.ajax({
            type: 'GET',
            url: `/home/projects?sessionId=${sessionId}`,
            success: function (data, status, xhr) {
                //console.log("du lieu nam o day");
                console.log('data: ', data);

                $('.select_project').show();
                $('.projects').select2();
                $.each(data, function (i, item) {
                    if (i === 0) {
                        projectName = item.name;
                        project_id = item.id;
                        // call api get task
                        $.ajax({
                            type: 'GET',
                            url: `/home/tasks?sessionId=${sessionId}&projectId=${project_id}`,
                            success: function (data, status, xhr) {
                                //console.log("du lieu nam o day");
                                console.log('data: ', data);
                                $('.select_task').show();
                                $('.tasks').select2();
                                $('.tasks').html("");
                                $.each(data, function (i, item) {
                                    if (i === 0) {
                                        task_id = item.id;
                                    }
                                    $('.tasks').append($('<option>', {
                                        value: item.id,
                                        text: item.name
                                    }));
                                });
                            }
                        });
                    }
                    $('.projects').append($('<option>', {
                        value: item.id,
                        text: item.name
                    }));
                });
            }
        });
    });

    // get project by session_id
    $('.projects').on('change', function () {
        project_id = this.value;
        projectName = $('.projects').find(":selected").text();
        let sessionId = $('.SessionId').val();
        $.ajax({
            type: 'GET',
            url: `/home/tasks?sessionId=${sessionId}&projectId=${project_id}`,
            success: function (data, status, xhr) {
                //console.log("du lieu nam o day");
                console.log('data: ', data);
                $('.select_task').show();
                $('.tasks').select2();
                $('.tasks').html("");
                $.each(data, function (i, item) {
                    if (i === 0) {
                        task_id = item.id;
                    }
                    $('.tasks').append($('<option>', {
                        value: item.id,
                        text: item.name
                    }));
                });
            }
        });
    });
    $('.users').on('change', function () {
        employee_id = this.value;
        employeeName = $('.users').find(":selected").text();
        console.log("employee_id", employeeName)
    });
    $('.tasks').on('change', function () {
        task_id = this.value;
    });
    $('.months').on('change', function () {
        month = this.value;
    });

    $('.btn_full_day_of_month').click(function () {
        type = 0;
        $('.full_day_of_month').show();
        $('.range_day').hide();
    });
    $('.btn_range_day').click(function () {
        type = 1;
        console.log("btn_range_day");
        $('.full_day_of_month').hide();
        $('.range_day').show();

    });


});
function WebSocketHandle(session_id) {
    // enable websocket
    let scheme = document.location.protocol === "https:" ? "wss" : "ws";
    let port = document.location.port ? (":" + document.location.port) : "";

    let webSocketUrl = scheme + "://" + document.location.hostname + port + "/ws";
    console.log("webSocketUrl", webSocketUrl);
    let socket = new WebSocket(webSocketUrl);

    socket.onopen = function (event) {
        console.log("onopen");
        socket.send(JSON.stringify({ "command": "subscribe", "identifier": session_id }))
    };
    socket.onclose = function (event) {
        console.log("onclose");
    };
    socket.onerror = function (event) {
        console.log("onerror ");
    };
    socket.onmessage = function (event) {
        let message = JSON.parse(event.data);
        if (message.command == "loaded") {
            console.log("onmessage", message);
            $('.loaded').css("width", `${message.data}%`);
            $('.percent').html(`loading ${message.data}%`)
        }

    };
}