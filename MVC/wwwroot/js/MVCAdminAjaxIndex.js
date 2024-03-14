$(document).ready(function () {
    var ImageFileName = "";
    console.log("Welcome Employee");
    GetAll();
    getDropdownValues();
    hideAlerts();
    //for set date time in formate
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

    //Reset values
    function ResetValues() {
        $('#prodname').val('');
        $('#proddisc').val('');
        $('#prodquantity').val('');
        $('#prodprice').val('');
    }
    // for getting dropdown
    function getDropdownValues() {
        var dropdown = $("#EmpDepartment, #EditEmpDepartment");
        dropdown.empty();
        $.ajax({
            url: '/MVCAjax/GetDepartment',
            type: 'GET',
            success: function (data) {
                data.forEach((Designation) => {
                    var row = '<option class="dropdown-item" value="' + Designation + '">' + Designation + '</option>';
                    dropdown.append(row);
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
            url: "/MVCAjax/AdminGetEmpData",
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
                    row += '<td><img src="/uploadsimg/'+emp.c_empimage+'" alt="Image not found" height="50"></td>';
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
    $('#reset').on('click', Reset);
    function Reset() {
        $('#name').val("");
        $('#courseNameDropdown').val("");
        $('#c_gender').val("");
        $('#date').val("");
        $('#salary').val("");
    }

    //edit
    $(document).on('click', '#edit', function () {
        var eid = $(this).data('id');
        console.log(eid);
        $.get("/MVCAjax/GetEmpDetail", { id: eid }, function (employee) {
            console.log(employee);
            var ImagePath = "/uploadsimg/" + employee.c_empimage;
            ImageFileName = employee.c_empimage;
            $('#EditEmpId').attr('data-id', eid);
            $('#EditEmpName').val(employee.c_empname);
            // Setting radio button for gender
            if (employee.c_empgender === 'Male') {
                $("input[name='EditEmpGender'][value='Male']").prop("checked", true);
            } else if (employee.c_empgender === 'Female') {
                $("input[name='EditEmpGender'][value='Female']").prop("checked", true);
            } else if (employee.c_empgender === 'Other') {
                $("input[name='EditEmpGender'][value='Others']").prop("checked", true);
            }
            $('#EditEmpDob').val(formatDateForInput(employee.c_dob));
            $("#EditEmpDepartment").val(employee.c_department).map;
            $('input[name="EditEmpShift"]').val(employee.c_shift);
            $('#EditImage').attr('src', ImagePath);
            $('#EditImage').attr('data-value', employee.c_empimage);
            $('#EditModel').modal('show');
        });
    });
    // Event handler for the Update button
    $('#FinalEditBtn').on('click', function () {
        var employee = {
            c_empid: parseInt($('#EditEmpId').attr('data-id')),
            c_empname: $('#EditEmpName').val(),
            c_empgender: $("input[name='EditEmpGender']:checked").val(),
            c_dob: $('#EditEmpDob').val().split('T')[0], // Extracting date part
            c_department: $("#EditEmpDepartment").val(),
            c_shift: $('input[name="EditEmpShift"]:checked').map(function () { return this.value; }).get(),
            c_empimage: ImageFileName,
        };
        console.log(employee);
        $.ajax({
            url: '/MVCAjax/AdminUpdateEmpData',
            type: 'POST',
            dataType: 'json',
            data: employee,
            success: function (data) {
                console.log(data);
                GetAll();
                $('#EditModel').modal('hide');
                alert(data.message);
            }
        });
    });

    //Delete employee
    $(document).on('click', '#del', function () {
        var Id = $(this).data('id');
        console.log(Id);
        if (confirm("Are you sure you want to delete this data?")) {
            $.ajax({
                url: '/MVCAjax/AdminDeleteEmpConfirm',
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