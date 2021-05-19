var dataTable;

$(document).ready(function () {
    LoadList();
});

function LoadList() {
    dataTable = $('#Dt_Load').DataTable({
        "ajax": {
            "url": "/api/SpeedGovernor/all",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "imei", "width": "20%" },
            { "data": "plateNumber", "width": "20%" },
            { "data": "phone", "width": "20%" },
            { "data": "owner", "width": "20%" },
            {
                "data": { imei: "imei", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    return `
                         <div class="text-center"> 
                           <a class="btn btn-primary text-white" style="cussor:pointer, width:100px;" href="/Admin/SpeedGovernors/Profile/Index/${data.imei}">
                             <i class="fas fa-view"</i> SpeedGovernor Details
                    </a></div>`;
                }
                ,
                "width": "20%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}


function LockUnlock(id) {
    $.ajax({
        type: 'POST',
        url: '/user',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            } else {
                toastr.error(data.message)
            }
        }
    });
}