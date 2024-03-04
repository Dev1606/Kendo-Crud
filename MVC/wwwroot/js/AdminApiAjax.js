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

    $('#LogoutBtn').on('click', function ()
    {
        localStorage.removeItem('token');
        window.location = '/UserApi/Login';
    });

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
    // for getting dropdown
    function getDropdownValues() {
        var dropdown = $("#c_department");
        dropdown.empty();
        $.ajax({
            url: 'https://localhost:7068/api/MVCApi/GetDropDepartment',
            type: 'GET',
            headers: {
                contentType: "application/json",
                Authorization: 'Bearer ' + localStorage.getItem('token')
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
                Authorization: 'Bearer ' + localStorage.getItem('token')
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
                    row += '<td> <img src="'+emp.c_empimage +'" alt="Image Not Found" style="height: 15%;width:15%;"></td>';
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
                $('#c_empid').val(eid);
                console.log("checkid"+$('#c_empid').val(eid));
                console.log("EID"+eid);
                $('#c_empname').val(employee.c_empname);
                // Setting radio button for gender
                if (employee.c_empgender === 'Male') {
                    $("input[name='c_empgender'][value='Male']").prop("checked", true);
                } else if (employee.c_empgender === 'Female') {
                    $("input[name='c_empgender'][value='Female']").prop("checked", true);
                } else if (employee.c_empgender === 'Other') {
                    $("input[name='c_empgender'][value='Other']").prop("checked", true);
                }
                $('#c_dob').val(formatDateForInput(employee.c_dob));
                $('input[name="c_shift"]').val(employee.c_shift);
                $("#c_department").val(employee.c_department);
                $('#EditModal').modal('show');
            },
            error: function (xhr, status, error) {
                // Handle errors here
                console.error('Error fetching employee details:', error);
            }
        });
    });
    // Event handler for the Update button
    $("#FinalEditBtn").click(function (event) {
        event.preventDefault();
        var c_empid = $('#c_empid').val();
        console.log("ID"+c_empid);
        var formData = new FormData(document.getElementById('editEmpForm'));
        formData.append("c_empid",c_empid);
        console.log(formData);
        $.ajax({
            url: "https://localhost:7068/api/MVCApi/UpdateEmpData",
            type: "PUT",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                GetAll();
                console.log(response);
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    });

//Delete employee
$(document).on('click', '#del', function () {
    var Id = $(this).data('id');
    console.log(Id);
    if (confirm("Are you sure you want to delete this data?")) {
        $.ajax({
            url: 'https://localhost:7068/api/MVCApi/DeleteEmpData?id=' + Id,
            type: 'DELETE',
            dataType: "json",
            contentType: "application/json",
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
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
    var username = "";
    $.ajax({
        url: 'https://localhost:7068/api/MVCApi/GetTokenData',
        type: 'GET',
        data: { usertoken: token },
        success: function (userData) {
            console.log('User Data:', userData);
            username = userData.userName;
            $.ajax({
                url: "/User/SetUserData",
                method: "POST",
                data: { username: username },
                async: false,
                success: function (response) {
                    if (response.success) {
                        // Handle successful response, e.g., display a success message
                        console.log("Username set successfully!");
                    } else {
                        // Handle unsuccessful response, e.g., display an error message
                        console.error("Failed to set username.");
                    }
                }
            });
        }
    });

}

function GetToken() {
    var token = localStorage.getItem('token');
    console.log(token);
}
});