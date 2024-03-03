$(document).ready(function () {
    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "https://localhost:7074/kendoGrid/UserGetEmpData",
                dataType: "json"
            },
            create: {
                url: "https://localhost:7074/KendoGrid/UserAddEmpData",
                type: "POST",
                dataType: "json"
            },
        },
        pageSize: 10,
        schema: {
            model: {
                id: "c_empid",
                fields: {
                    c_empid: { type: "number", editable: false, nullable: false },
                    c_empname: { type: "string", validation: { required: true } },
                    c_empgender: { type: "string", validation: { required: true } },
                    c_dob: { type: "string", validation: { required: true } },
                    c_shift: { type: "string", validation: { required: true } },
                    c_department: { type: "string", validation: { required: true } },
                    c_empimage: { type: "string", validation: { required: true } },
                }
            }
        }
    });

    $("#grid").kendoGrid({
        dataSource: dataSource,
        columns: [
            { field: "c_empid", title: "ID" },
            { field: "c_empname", title: "Employee Name" },
            { field: "c_empgender", title: "Gender", editor: EmpGenderEditor },
            {
                field: "c_dob",
                title: "DOB",
                editor: function (container, options) {
                    $(container).kendoCalendar({
                        format: "yyyy/MM/dd",
                        change: function () {
                            console.log("Change :: " + kendo.toString(this.value(), 'yyyy/MM/dd'));
                            options.model.set("c_dob", kendo.toString(this.value(), 'yyyy/MM/dd'));
                        },
                        navigate: function () {
                            console.log("Navigate");
                        }
                    });
                }
            },
            {
                field: "c_shift", title: "Shift", editor: function (container, options) {

                    $(container).kendoCheckBoxGroup({
                        items: ["Morning", "Afternoon", "Night"],
                        layout: "horizontal",
                        change: function () {
                            var selectedValues = $(container).kendoCheckBoxGroup("value")
                            console.log(selectedValues);
                            var arrayExpression = `${selectedValues.join(",")}`;
                            options.model.set("c_shift", arrayExpression);
                            console.log("c_shift", arrayExpression);
                        }
                    });
                },
            },
            { field: "c_empimage", title: "Image", editor: imageupload, template: "<img src='#: c_empimage #' alt='Employee Photo' style='width: 50px; height:50px;'/>" },
            {
                field: "c_department", title: "Department", editor: function (container, options) {
                    $('<input name="' + options.field + '" id="stateDropdown" checked="checked" optionLabel="Select" style="width: 100%;" />').appendTo(container).kendoDropDownList({
                        dataSource: {
                            transport: {
                                read: "https://localhost:7074/kendoGrid/GetDepartment",
                                datatype: "json",
                            }
                        },

                    });
                }
            },
        ],
        editable: "popup",
        toolbar: ["create"],
        pageable: true,
        sortable: true,
        filterable: true,
    });

    dataSource.bind("requestEnd", function (e) {
        if (e.type === "create" || e.type === "update" || e.type === "destroy") {
            dataSource.read();
        }
    });

    $("#grid").on("click", ".k-grid-cancel-changes", function () {
        dataSource.cancelChanges();
    })

    //RadioButton
    function EmpGenderEditor(container, options) {
        var radioButtonsHtml = '<input type="radio" name="c_empgender" value="Male" id="radioMale" class="k-radio" /><label class="k-radio-label" for="radioMale">Male</label>' +
            '<input type="radio" name="c_empgender" value="Female" id="radioFemale" class="k-radio" /><label class="k-radio-label" for="radioFemale">Female</label>';
        $(radioButtonsHtml).appendTo(container);
    }

    //Image Upload
    function imageupload(container, options) {
        $('<input name="Image" type="file" id="photo" data-role="upload" data-async=\'{ "saveUrl": "/kendogrid/uploadphoto", "autoUpload": true }\' class="k-input k-textbox">').appendTo(container);
    }

});
