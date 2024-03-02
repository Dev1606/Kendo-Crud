$(document).ready(function () {
    console.log("Welcome Employee");
    GetAll();
    GetAllUser();
    hideAlerts();
    //AddUser();
    getDropdownValues();
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
        var dropdown = $("#EditEmpDepartment");
        dropdown.empty();
        $.ajax({
            url: 'https://localhost:7068/api/MVCApi/GetDropDepartment',
            type: 'GET',
            success: function (data) {
                data.forEach((Designation) => {
                    var row = '<option class="dropdown-item" value="' + Designation + '">' + Designation + '</option>';
                    dropdown.append(row);
                });
            }
        });
    }

    //New Code Fore AddEmp():
    // document.getElementById('printbtn').addEventListener('submit', function (event) {
    //     event.preventDefault();
    //     var form = document.getElementById('printbtn');
    //     var formData = new FormData(form);

    //     //formData.append("c_userid", username);
    //     var fileInput = document.getElementById('file');
    //     if (fileInput.files.length === 0) {
    //         formData.delete("file");
    //         formData.append("file", null);
    //     }

    //     for (var pair of formData.entries()) {
    //         console.log(pair[0] + ': ' + pair[1]);
    //     }
    //     var empid = formData.get("c_empid");
    //     if (empid && empid > 0) {
    //         fetch('https://localhost:7068/api/MVCApi/UpdateEmpData?id='+ empid, {
    //             method: 'PUT',
    //             body: formData,
                
    //         })
    //             .then(response => {
    //                 if (!response.ok) {
    //                     throw new Error('Failed to updated employee.');
    //                 }
    //                 return response.text();
    //             })
    //             .then(data => {
    //                 console.log(data);
    //                 alert("Employee updated successfully!");
    //                 //window.location.href = '/MvcEmployee/Index';
    //             })
    //             .catch(error => {
    //                 console.error('Error updated employee:', error.message);
    //                 alert("Failed to Update employee.");
    //             });
    //     }
    //     else {
    //         formData.append("c_empid" , 1111);
    //         fetch('https://localhost:7068/api/MVCApi/UserAddEmpData', {
    //             method: 'POST',
    //             body: formData
    //         })
    //             .then(response => {
    //                 if (!response.ok) {
    //                     throw new Error('Failed to Add employee.');
    //                 }
    //                 return response.text();
    //             })
    //             .then(data => {
    //                 console.log(data);
    //                 alert("Employee Add successfully!");
    //                 //window.location.href = '/MvcEmployee/Index';
    //             })
    //             .catch(error => {
    //                 console.error('Error Add employee:', error.message);
    //                 alert("Failed to Add employee.");
    //             });
    //     }
    // });

    //Get All User Details:
    function GetAllUser() {
        var table = $('#TableContent11');
        table.empty();
        $.ajax({
            type: "GET",
            url: "https://localhost:7068/api/MVCApi/GetEmpData",
            success: function (emp) {
                emp.forEach(function (emp) {
                    var row = '<tr>';
                    row += '<td>' + emp.c_empid + '</td>';
                    row += '<td>' + emp.c_empname + '</td>';
                    row += '<td>' + emp.c_empgender + '</td>';
                    row += '<td>' + emp.c_dob + '</td>';
                    row += '<td>' + emp.c_shift + '</td>';
                    row += '<td>' + emp.c_department + '</td>';
                    row += '<td>' + emp.c_empimage + '</td>';
                    row += '<td>';
                    // row += '<div class="d-flex justify-content-between">';
                    // row += '<button type="button" id="edit" class="btn btn-outline-success edit" data-id="' + emp.c_empid + '">Edit</button>';
                    // row += '<button type="button" id="del" class="btn btn-outline-danger delete" data-id="' + emp.c_empid + '">Delete</button>';

                    row += '</div>';
                    row += '</td>';
                    row += '</tr>';
                    table.append(row);
                });
            }
        });
    }
     

    //Get All Admin Details:
    function GetAll() {
        var table = $('#TableContent');
        table.empty();
        $.ajax({
            type: "GET",
            url: "https://localhost:7068/api/MVCApi/GetEmpData",
            success: function (emp) {
                emp.forEach(function (emp) {
                    var row = '<tr>';
                    row += '<td>' + emp.c_empid + '</td>';
                    row += '<td>' + emp.c_empname + '</td>';
                    row += '<td>' + emp.c_empgender + '</td>';
                    row += '<td>' + emp.c_dob + '</td>';
                    row += '<td>' + emp.c_shift + '</td>';
                    row += '<td>' + emp.c_department + '</td>';
                    row += '<td>' + emp.c_empimage + '</td>';
                    row += '<td>';
                    row += '<div class="d-flex justify-content-between">';
                    row += '<button type="button" id="edit" class="btn btn-outline-success edit" data-id="' + emp.c_empid + '">Edit</button>';
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
        $.get("https://localhost:7068/api/MVCApi/GetEmpDetail", { id: eid }, function (employee) {
            console.log(employee);
            $('#Empid').attr('data-id', eid);
            $('#EditEmpName').val(employee.c_empname);
            // Setting radio button for gender
            if (employee.c_empgender === 'Male') {
                $("input[name='EditEmpGender'][value='Male']").prop("checked", true);
            } else if (employee.c_empgender === 'Female') {
                $("input[name='EditEmpGender'][value='Female']").prop("checked", true);
            } else if (employee.c_empgender === 'Other') {
                $("input[name='EditEmpGender'][value='Other']").prop("checked", true);
            }
            $('#EditEmpDob').val(formatDateForInput(employee.c_dob));
            $("#EditEmpDepartment").val(employee.c_department).map;
            $('input[name="EditEmpShift"]').val(employee.c_shift);
            $('#EditEmpImage').val(employee.Image);
            $('#EditModel').modal('show');
        });
    });
    // Event handler for the Update button
    $('#FinalEditBtn').on('click', function () {
        var employee = {
            c_empid: parseInt($('#Empid').attr('data-id')),
            c_empname: $('#EditEmpName').val(),
            c_empgender: $("input[name='EditEmpGender']:checked").val(),
            c_dob: $('#EditEmpDob').val().split('T')[0], // Extracting date part
            c_department: $("#EditEmpDepartment").val(),
            c_shift: $('input[name="EditEmpShift"]:checked').map(function () { return this.value; }).get(),
            c_empimage: parseInt($('#EditEmpImage').val()),
        };
        console.log(employee);
        $.ajax({
            url: 'https://localhost:7068/api/MVCApi/UpdateEmpData',
            type: 'PUT',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(employee),
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
        if (confirm("Are you sure you want to delete this data?")) {
            $.ajax({
                url: 'https://localhost:7068/api/MVCApi/DeleteEmpData',
                type: 'DELETE',
                data: JSON.stringify(Id),
                contentType: "application/json",
                success: function (data) {
                    GetAll();
                    alert(data.message);
                }
            });
        }
    });
});