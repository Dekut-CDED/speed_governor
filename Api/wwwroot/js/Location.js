var dataTable;

$(document).ready(function () {
    LoadList();
});

function LoadList() {
    dataTable = $('#Dt_Load').DataTable({
        "ajax": {
            "url": "/api/@id",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "Time", "width": "10%" },
            { "data": "speed", "width": "5%" },
            { "data": "latitude", "width": "10%" },
            { "data": "long", "width": "10%" },
            { "data": "gpsCource", "width": "10%" },
            { "data": "speedSignalStatus", "width": "10%" },
            { "data": "engineON", "width": "5%" },
            {
                "data": { id: "id", latitude: "latitude", logitude: "long" },
                "render": function (data) {
                    return `
                         <div class="text-center"> 
                           <a class="btn btn-primary text-white" style="cussor:pointer, width:100px;" onClick=LockUnlock('${data}')>
                             <i class="fas fa-view"</i> View Location
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


// TODO route to the another dotnet page from js