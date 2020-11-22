function getSelectedTaskId() {
    return $("#selectTask").val();
}

function getSelectedCustomerId() {
    return $("#selectCustomer").val();
}
function getSelectedHour() {
    return $("#selectHour").val();
}

function getTaskInfo() {
    
    $.get('/Home/TaskStatus',
        {
            taskId: getSelectedTaskId(),
            customerId: getSelectedCustomerId()
        },
        function (statusJson) {
            displayStatusData(statusJson);
        }
    );
}

function startTask() {
    var idVal = $("#selectTask").val();
    $.get('/Home/TaskStart',
        {
            taskId: getSelectedTaskId(),
            customerId: getSelectedCustomerId()
        },
        function (statusJson) {
            displayStatusData(statusJson);
        }
    );
}


function terminateTask() {
    $.get('/Home/TaskTerminate',
        {
            taskId: getSelectedTaskId(),
            customerId: getSelectedCustomerId()
        },
        function (statusJson) {
            displayStatusData(statusJson);
        }
    );
}


function startSchedulerTask() {
    $.get('/Home/TaskScheduler',
        {
            taskId: getSelectedTaskId(),
            customerId: getSelectedCustomerId(),
            hour: getSelectedHour()
        },
        function (statusJson) {
            displayStatusData(statusJson);
        }
    );
}


function displayStatusData(statusJson) {
    $("#tableInfoBody").empty();
    $.each(statusJson, function (key, data) {
        if (data) {
            $('#tableInfoId').append('<tr><td>' + key + '</td><td>' + data + '</td></tr>');
        }
    });
}