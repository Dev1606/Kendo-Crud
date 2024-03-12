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
            items: ["Morning", "Afternoon", "Night"],
            layout: "horizontal"
        });
    }

    function initializeDatePicker() {
        $("#EditEmpDob").kendoDatePicker({
            format: "yyyy-MM-dd"
        });
    }
    //add
    $("#FinalEditBtn").click(function () {
        var formData = new FormData();
        formData.append("c_empid", $('#EditEmpId').val());
        formData.append("c_empname", $("#EditEmpName").val());
        formData.append("c_empgender", $('#EditEmpGender').data("kendoRadioGroup").value());
        formData.append("c_dob", $("#EditEmpDob").val());
        var shiftValues = $("#EditEmpShift").data("kendoCheckBoxGroup").value().join(",");
        formData.append("c_shift", shiftValues);
        console.log("Shiftvalues"+shiftValues);
        formData.append("c_department",$("#EditEmpDepartment").data("kendoDropDownList").value());
        formData.append("c_empimage",$("#EditEmpImage").val());
        formData.append("Image", $('#EditEmpImage')[0].files[0]);

        $.ajax({
            url: "/KendoComponent/UserAddEmpData",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (response) {
                console.log(formData);
                console.log(response);
                $("#messageSuccess").text("Employee Added").show();
                $("#messageFail").hide();
                GetAll();
            },
            error: function (xhr, status, error) {
                $("#messageFail").text(xhr.responseText).show();
                $("#messageSuccess").hide();
            }
        });
    });
});