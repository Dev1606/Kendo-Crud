$(document).ready(function(){
    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "https://localhost:7074/kendoGrid/AdminGetEmpData",
                dataType: "json"
            },
            update: {
                url: "https://localhost:7074/kendoGrid/AdminUpdateEmpData",
                type: "POST",
                dataType: "json"
            },
            destroy: {
                url: "https://localhost:7074/kendoGrid/AdminDeleteEmpConfirm",
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
        {field: "c_empgender", title: "Gender",editor: function (container, options) {
            $('<input id="engine1" type="radio" name="' + options.field + '" value="Male" selectable="true" />').appendTo(container);
            $('<label for="engine1">Male</label>').appendTo(container);
            $('<input id="engine2" type="radio" name="' + options.field + '" value="Female"/>').appendTo(container);
            $('<label for="engine2">Female</label>').appendTo(container);
        }},
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