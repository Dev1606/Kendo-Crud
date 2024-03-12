$(document).ready(function () {
    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "https://localhost:7068/api/MVCApi/UserGetEmpData",
                headers: {
                    Authorization: 'Bearer ' + localStorage.getItem('token')
                },
                dataType: "json"
            },
            create: {
                url: "https://localhost:7068/api/MVCApi/AddKendoEmpData",
                headers: {
                    Authorization: 'Bearer ' + localStorage.getItem('token')
                },
                type: "POST",
                dataType: "json"
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options.models) {
                    var formData = new FormData();
                    // For create or update operations
                    options.models.forEach(function(model) {
                        // Append each model's data to the form data
                        for (var prop in model) {
                            if (prop === 'Image') {
                                formData.append('Image', model[prop], model[prop].name);
                            } else {
                                formData.append(prop, model[prop]);
                            }
                        }
                    });
                    return formData;
                }
                return options;
            }

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
            { field: "c_empid", title: "Emp ID" },
            { field: "c_empname", title: "Emp Name" },
            {
                field: "c_empgender", title: "Gender", editor: function (container, options) {
                    $('<input id="male" type="radio" name="' + options.field + '" value="Male" selectable="true" />').appendTo(container);
                    $('<label for="male">Male</label>').appendTo(container);
                    $('<input id="Female" type="radio" name="' + options.field + '" value="Female"/>').appendTo(container);
                    $('<label for="Female">Female</label>').appendTo(container);
                }
            },
            {
                field: "c_dob",
                title: "DOB",
                editor: function (container, options) {
                    $(container).kendoCalendar({
                        componentType: "modern",
                        format: "yyyy/MM/dd",
                        change: function () {
                            console.log("Change :: " + kendo.toString(this.value(), 'yyyy/MM/dd'));
                            options.model.set("c_dob", kendo.toString(this.value(), 'yyyy/MM/dd'));

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
            {
                field: "c_shift", title: "Shift", editor: function (container, options) {
                    var shiftValues = options.model.c_shift.split(",");
                    $(container).kendoCheckBoxGroup({
                        items: ["Morning", "Afternoon", "Night"],
                        layout: "horizontal",
                        change: function () {
                            var selectedValues = $(container).kendoCheckBoxGroup("value");
                            var formattedValue = selectedValues.join(", ");
                            options.model.set("c_shift", formattedValue);
                            console.log("c_shift", formattedValue);
                        }
                        
                    });
                    $(container).kendoCheckBoxGroup("value", shiftValues);
                }
            },
            {
                field: "c_department", title: "Department", editor: function (container, options) {
                    $('<input name="' + options.field + '" id="stateDropdown" checked="checked" optionLabel="Select" style="width: 100%;" />').appendTo(container).kendoDropDownList({
                        dataSource: {
                            transport: {
                                read: "https://localhost:7068/api/MVCApi/GetDropDepartment",
                                datatype: "json"
                            }
                        },
                    });
                }
            },
            {
                field: "c_empimage", title: "Image", editor: function (container, options) {
                    imageupload(container, options);
                }, template: "<img src='#: c_empimage #' alt='Employee Photo' style='width: 50px; height:50px;'/>"
            },
            { command: ["edit", "destroy"], title: "&nbsp;", width: "200px" }],
        editable: "popup",
        toolbar: ["create"],
        pageable: true,
        sortable: true,
        filterable: true,
        edit: function (e) {
            var dataItem = e.model;
            var dob = kendo.parseDate(dataItem.c_dob);
            e.container.find(".k-calendar").data("kendoCalendar").value(dob);
        }
    });
    function imageupload(container, options) {
        // Create the file input element
        var fileInput = $('<input name="Image" type="file" id="photo" data-role="upload" data-async=\'{ "saveUrl": "/kendogrid/UploadPhoto", "autoUpload": true }\' class="k-input k-textbox">');
        fileInput.appendTo(container);
        fileInput.on('change', function () {
            var file = this.files[0];
            c_empimage = file.name;
            console.log('Selected file:', c_empimage);
            console.log('Selected file:', file.name);
            options.model.set("c_empimage", file.name);
            console.log("c_empimage", file.name);
        });
    }
    dataSource.bind("requestEnd", function (e) {
        if (e.type === "create" || e.type === "update" || e.type === "destroy") {
            dataSource.read();
        }
    });
    $("#grid").on("click", ".k-grid-cancel-changes", function () {
        dataSource.cancelChanges();
    });
});