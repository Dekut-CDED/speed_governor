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
            { "data": "plateNumber", "width": "10%" },
            { "data": "phone", "width": "15%" },
            { "data": "fuellevel", "width": "20%" },
            { "data": "vibrations", "width": "20%" },
            {
                "data": { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    return `
                         <div class="text-center"> 
                           <a class="btn btn-primary text-white" style="cussor:pointer, width:100px;" onClick=LockUnlock('${data.id}')>
                             <i class="fas fa-view"</i> View SpeedGovernor
                    </a></div>`;
                }
                ,
                "width": "100%"
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