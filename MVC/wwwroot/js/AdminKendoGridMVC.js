$(document).ready(function(){
    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "/MVCViewController/AdminGetEmpData",
                dataType: "json"
            },
            update: {
                url: "/MVCViewController/AdminUpdateEmpData",
                type: "POST",
                dataType: "json"
            },
            destroy: {
                url: "/MVCViewController/AdminDeleteEmp",
                type: "POST",
                dataType: "json"
            }
        },
        pageSize: 10,
        schema: {
            model : {
                id: "c_empid",
                fields: {
                    c_empid: {type:"number", editable: false, nullable: false},
                    c_empname: {type:"string",validation:{required:true}},
                    c_empgender: {type:"string",validation:{required:true}},
                    c_dob: {type:"string",validation:{required:true}},
                    c_shift: {type:"string",validation:{required:true}},
                    c_department: {type:"string",validation:{required:true}},
                    c_empimage: {type:"string",validation:{required:true}},
                }
            }
        }
    });

    $("#grid").kendoGrid({
        dataSource: dataSource,
        columns: [
        {field: "c_empid", title: "ID"},
        {field: "c_empname", title: "Employee Name"},
        {field: "c_empgender", title: "Gender"},
        {field: "c_dob", title: "DOB"},
        {field: "c_shift", title: "Shift"},
        {field: "c_department", title: "Department"},
        {field: "c_empimage", title: "Image"},
        {command: ["edit","destroy"], title: "Action", width: "200px"}
        ],
        editable: "popup",
        pageable: true,
        sortable: true,
        filterable: true,
    });

    dataSource.bind("requestEnd", function(e){
        if(e.type === "create" || e.type === "update" || e.type === "destroy"){
            dataSource.read();
        }
    });

    $("#grid").on("click", ".k-grid-cancel-changes", function(){
        dataSource.cancelChanges();
    })
});