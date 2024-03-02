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
                url: function(data){ return "https://localhost:7074/kendoGrid/AdminDeleteEmpConfirm/"+data.c_empid;},
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
        {
            field: "c_dob",
            title: "DOB",      
            editor: function (container, options) {
                // var dob = options.model.c_dob;
                // console.log("DOB",dob);
                $(container).kendoCalendar({
                    componentType: "modern",
                    format: "yyyy/MM/dd",
                    change: function () {
                        console.log("Change :: " + kendo.toString(this.value(), 'yyyy/MM/dd'));
                        options.model.set("c_dob",kendo.toString(this.value(), 'yyyy/MM/dd'));
                    },
                    navigate: function () {
                        console.log("Navigate");
                    }
                });
                // $(container).kendoCalendar("value", dob);
            },
            template: function (dataItem) {
                var dob = kendo.toString(kendo.parseDate(dataItem.c_dob), "yyyy-MM-dd");
                return dob;
            }
        },       
        {field: "c_shift", title: "Shift",editor:function(container,options){
            var shiftValues = options.model.c_shift.split(","); 
            $(container).kendoCheckBoxGroup({
                items:["Morning","Afternoon","Night"],
                layout:"horizontal",
                change: function () {
                    var selectedValues = $(container).kendoCheckBoxGroup("value")
                    console.log(selectedValues);
                    var arrayExpression = `${selectedValues.join(",")}`;
                    options.model.set("c_shift", arrayExpression);
                    console.log("c_shift", arrayExpression);
                }
            });
            $(container).kendoCheckBoxGroup("value", shiftValues);
        }
        },
        {field: "c_department", title: "Department",editor: function (container, options) {
            $('<input name="' + options.field + '" id="stateDropdown" checked="checked" optionLabel="Select" style="width: 100%;" />').appendTo(container).kendoDropDownList({
                dataSource: {
                    transport: {
                        read: "https://localhost:7074/kendoGrid/GetDepartment",
                        datatype: "json",
                    }
                },

            });
    }},
        {field: "c_empimage", title: "Image",editor:imageupload, template:"<img src='#: c_empimage #' alt='Employee Photo' style='width: 50px; height:50px;'/>"},
        {command: ["edit","destroy"], title: "Action", width: "200px"}
        ],
        editable: {
            mode: "popup",
            window: {
                width: "500px",
                height:"700px"
            }
        },
        pageable: true,
        sortable: true,
        filterable: true,
        edit: function(e) {
            var dataItem = e.model;
            var dob = kendo.parseDate(dataItem.c_dob);
            e.container.find(".k-calendar").data("kendoCalendar").value(dob);
        } 
    });
   
 
function imageupload(container) {
    $('<input name="Image" type="file" id="photo" data-role="upload" data-async=\'{ "saveUrl": "/kendogrid/uploadphoto", "autoUpload": true }\' class="k-input k-textbox">').appendTo(container);
    }

    dataSource.bind("requestEnd", function(e){
        if(e.type === "create" || e.type === "update" || e.type === "destroy"){
            dataSource.read();
        }
    });

    $("#grid").on("click", ".k-grid-cancel-changes", function(){
        dataSource.cancelChanges();
    })
});