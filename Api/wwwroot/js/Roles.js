var dataTable;

$(document).ready(function () {
    LoadList();
});

function LoadList() {
    dataTable = $('#Dt_Load').DataTable({
        "ajax": {
            "url": "/api/school",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "40%" },
            { "data": "motto", "width": "30%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                         <div class="text-center"> 
                           <a href="/Admin/school/upsert?id=${data}" class="btn btn-success text-white" style="cussor:pointer, width:100px;">
                             <i class="fas fa-edit"></i> Edit</a>
                         
                           <a class="btn btn-danger text-white" style="cursor:pointer, width:100px;" onClick=Delete('/api/school/'+'${data}')>
                             <i class="fas fa-trash-alt"></i> Delete</a>
                         </div>`;
                },
                "width": "30%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    })
}


function Delete(url) {
    swal({
        title: "Are You sure You Want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willdelet) => {
        if (willdelet) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message)
                    }

                }
            })
        }
    })
}