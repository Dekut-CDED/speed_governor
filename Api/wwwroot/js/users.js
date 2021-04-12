var dataTable;

$(document).ready(function () {
    LoadList();
});

function LoadList() {
    dataTable = $('#Dt_Load').DataTable({
        "ajax": {
            "url": "/api/User/all",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "fullName", "width": "25%" },
            { "data": "username", "width": "15%" },
            { "data": "email", "width": "20%" },
            { "data": "phoneNumber", "width": "20%" },
            {
                "data": { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {

                        return `
                         <div class="text-center"> 
                           <a class="btn btn-danger text-white" style="cussor:pointer, width:100px;" onClick=LockUnlock('${data.id}')>
                             <i class="fas fa-lock-open"</i> Unlock
                    </a></div>`;
                    } else {
                        return `
                         <div class="text-center"> 
                           <a class="btn btn-success text-white" style="cussor:pointer, width:100px;" onClick=LockUnlock('${data.id}')>
                             <i class="fas fa-lock"</i> LockUser
                         </div>`;
                    }
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