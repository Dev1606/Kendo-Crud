// if (localStorage.getItem('token') == null) {
//     window.location = '/UserApi/Login';
// }
$(document).ready(function () {

    var token = localStorage.getItem('token');
    if (token == null) {
        // Redirect to the login page if the token is not present
        window.location = '/UserApi/Login';
    } else {
        // Fetch user data using the token
        fetchUserData(token);
        
        // Other initialization functions
        GetAll();
        hideAlerts();
        getDropdownValues();
    }

    // console.log("Welcome Admin Api");
    // GetAll();
    // hideAlerts();
    // getDropdownValues();
    // GetToken();
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
            headers : {
                contentType: "application/json",
                Authorization: 'Bearer '+localStorage.getItem('token')
            },
            success: function (data) {
                console.log(data);
                    data.forEach((Designation) => {
                    var row = '<option class="dropdown-item" value="' + Designation + '">' + Designation + '</option>';
                    dropdown.append(row);
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
            headers: {
                // "Authorization": localStorage.getItem('token')
                Authorization: 'Bearer '+localStorage.getItem('token')
            },
            success: function (emp) {
                emp.forEach(function (emp) {
                    var row = '<tr>';
                    row += '<td>' + emp.c_empid + '</td>';
                    row += '<td>' + emp.c_empname + '</td>';
                    row += '<td>' + emp.c_empgender + '</td>';
                    row += '<td>' + emp.c_dob + '</td>';
                    row += '<td>' + emp.c_shift + '</td>';
                    row += '<td>' + emp.c_department + '</td>';
                    row += '<td> <img src="'+ emp.c_empimage +'" alt="Image Not Found" style="height: 15%;width:15%;"></td>';
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
        // console.log(localStorage.getItem('token'));
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
        // console.log(eid);
    
        // Make the GET request with the Authorization header
        $.ajax({
            url: "https://localhost:7068/api/MVCApi/GetEmpDetail",
            type: "GET",
            data: { id: eid },
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            },
            success: function (employee) {
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
                $("#EditEmpDepartment").val(employee.c_department);
                $('input[name="EditEmpShift"]').val(employee.c_shift);
                $('#EditModel').modal('show');
            },
            error: function (xhr, status, error) {
                // Handle errors here
                console.error('Error fetching employee details:', error);
            }
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
            c_empimage: $('#EditEmpImage').val()
        };
        debugger
        console.log(employee);
        $.ajax({
            url: 'https://localhost:7068/api/MVCApi/UpdateEmpData',
            type: 'PUT',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(employee),
            headers : {
                Authorization: 'Bearer '+localStorage.getItem('token')
            },
            success: function (data) {
               debugger
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
                url: 'https://localhost:7068/api/MVCApi/DeleteEmpData?id='+Id,
                type: 'DELETE',
                dataType:"json",
                contentType: "application/json",
                headers : {
                    Authorization: 'Bearer '+localStorage.getItem('token')
                },
                success: function (data) {
                    console.log(data);
                    GetAll();
                    alert(data.message);
                }
            });
        }
    });

    function fetchUserData(token) {
        $.ajax({
            url: 'https://localhost:7068/api/MVCApi/GetTokenData',
            type: 'GET',
            data: { usertoken: token },
            success: function (userData) {
                console.log('User Data:', userData);
                // Handle user data, e.g., display user information on the page
            },
            error: function (xhr, status, error) {
                console.error('Error fetching user data:', error);
                // Handle errors, e.g., redirect to login page or show an error message
            }
        });
    }

    function GetToken()
    {
        var token = localStorage.getItem('token');
        console.log(token);
    }
});