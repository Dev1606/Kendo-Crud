$(document).ready(function () {
    var ImageFileName = "";
    console.log("Welcome Employee");
    GetAll();
    getDropdownValues();
    hideAlerts();
    initializeGenderRadioButtons();
    initializeShiftCheckboxes();
    initializeDatePicker();
    function formatDateForInput(dateString) {
        const dateObj = new Date(dateString);
        const formattedDate = dateObj.toISOString().slice(0, 10);
        return formattedDate;
    }
    function hideAlerts() {
        $('#messageSuccess').hide();
        $('#messageFail').hide();

    }
    function successMsg(str) {
        $("#messageSuccess").text('');
        $("#messageSuccess").append(str);
        $("#messageSuccess").show().delay(3000).fadeOut();
    }
    function alertMsg(str) {
        $("#messageFail").text('');;
        $("#messageFail").append(str);
        $("#messageFail").show().delay(3000).fadeOut();
    }
    // for getting dropdown
    function getDropdownValues() {
        var dropdown = $("#EmpDepartment, #EditEmpDepartment");
        dropdown.empty();
        $.ajax({
            url: '/KendoComponent/GetDepartment',
            type: 'GET',
            success: function (data) {
                dropdown.kendoDropDownList({
                    dataSource: data,
                    optionLabel: "Select Department" 
                });
            }
        });
    }

    //get
    function GetAll() {
        var table = $('#TableContent');
        table.empty();
        $.ajax({
            type: "GET",
            url: "/KendoComponent/AdminGetEmpData",
            dataType: 'json',
            success: function (emp) {
                emp.forEach(function (emp) {
                    var row = '<tr>';
                    row += '<td>' + emp.c_empid + '</td>';
                    row += '<td>' + emp.c_empname + '</td>';
                    row += '<td>' + emp.c_empgender + '</td>';
                    row += '<td>' + emp.c_dob + '</td>';
                    row += '<td>' + emp.c_shift + '</td>';
                    row += '<td>' + emp.c_department + '</td>';
                    row += '<td><img src="../wwwroot/uploadsimg/' + emp.c_empimage + '"></td>';
                    row += '<td>';
                    row += '<div class="d-flex justify-content-between">';
                    row += '<button type="button" id="edit" class="btn btn-outline-success edit" data-id="' + emp.c_empid + '" data-bs-toggle="modal" data-bs-target="#EditEmpModal">Edit</button>';
                    row += '<button type="button" id="del" class="btn btn-outline-danger delete" data-id="' + emp.c_empid + '">Delete</button>';

                    row += '</div>';
                    row += '</td>';
                    row += '</tr>';
                    table.append(row);
                });
            }
        });
    }

    function initializeGenderRadioButtons() {
        $("#EditEmpGender").kendoRadioGroup({
            layout: "horizontal",
            items: [
                { label: "Male", value: "Male" },
                { label: "Female", value: "Female" },
                { label: "Other", value: "Other" }
            ]
        });
    }

    function initializeShiftCheckboxes() {
        $("#EditEmpShift").kendoCheckBoxGroup({
            items: [
                { label: "Morning", value: "Morning" },
                { label: "Afternoon", value: "Afternoon" },
                { label: "Night", value: "Night" }
            ],
            layout: "horizontal"
        });
    }

    function initializeDatePicker() {
        $("#EditEmpDob").kendoDatePicker({
            format: "yyyy-MM-dd"
        });
    }
    //edit
    $(document).on('click', '#edit', function () {
        var eid = $(this).data('id');
        console.log(eid);
        $.get("/KendoComponent/GetEmpDetail", { id: eid }, function (employee) {
            var ImagePath = "~/uploadsimg/" + employee.c_empimage;
            ImageFileName = employee.c_empimage;
            console.log(employee);
            $('#Empid').attr('data-id', eid);
            $('#EditEmpId').val(employee.c_empid);
            $('#EditEmpName').val(employee.c_empname);
            $('#EditEmpGender').data("kendoRadioGroup").value(employee.c_empgender);
            $('#EditEmpDob').val(formatDateForInput(employee.c_dob));
            $("#EditEmpDepartment").data("kendoDropDownList").value(employee.c_department);
            $("#EditEmpShift").data("kendoCheckBoxGroup").value(employee.c_shift);
            $('#EditImage').attr('src', ImagePath);
            $('#EditImage').attr('data-value', employee.c_empimage);
            $('#EditModel').modal('show');
        });
    });

    $("#FinalEditBtn").click(function () {
        var formData = new FormData();
        formData.append("c_empid", $('#EditEmpId').val());
        formData.append("c_empname", $("#EditEmpName").val());
        formData.append("c_empgender", $('#EditEmpGender').data("kendoRadioGroup").value());
        formData.append("c_dob", $("#EditEmpDob").val());
        formData.append("c_shift", $("#EditEmpShift").data("kendoCheckBoxGroup").value());
        formData.append("c_department", $("#EditEmpDepartment").data("kendoDropDownList").value());
        formData.append("Image", $('#EditEmpImage')[0].files[0]); 
    
        $.ajax({
            url: "/KendoComponent/AdminUpdateEmpData",
            type: "POST",
            processData: false,
            contentType: false, 
            data: formData,
            success: function (response) {
                console.log(response);
                $("#messageSuccess").text(response.message).show();
                $("#messageFail").hide();
                GetAll();
            },
            error: function (xhr, status, error) {
                $("#messageFail").text(xhr.responseText).show();
                $("#messageSuccess").hide();
            }
        });
    });
    

    //Delete employee
    $(document).on('click', '#del', function () {
        var Id = $(this).data('id');
        console.log(Id);
        if (confirm("Are you sure you want to delete this data?")) {
            $.ajax({
                url: '/KendoComponent/AdminDeleteEmpConfirm',
                type: 'POST',
                data: {id : Id},
                dataType: 'json',
                success: function (data) {
                    GetAll();
                    alert(data.message);
                }
            });
        }
    });


});