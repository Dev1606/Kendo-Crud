$(document).ready(function () {

    var token = localStorage.getItem('token');
    if (token == null) {
        // Redirect to the login page if the token is not present
        window.location = '/UserApi/Login';
    } else {
        // Fetch user data using the token
        fetchUserData(token);

        // Other initialization functions
        GetAllUser();
        hideAlerts();
        getDropdownValues();
        GetToken();
    }

    $('#LogoutBtn').on('click', function () {
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

    //Reset values
    function ResetValues() {
        $('#prodname').val('');
        $('#proddisc').val('');
        $('#prodquantity').val('');
        $('#prodprice').val('');
    }
    // for getting dropdown
    function getDropdownValues() {
        var dropdown = $("#c_department");
        dropdown.empty();
        $.ajax({
            url: 'https://localhost:7068/api/MVCApi/GetDropDepartment',
            type: 'GET',
            headers: {
                Authorization: 'Bearer ' + localStorage.getItem('token')
            },
            success: function (data) {
                // console.log(data);
                data.forEach((Designation) => {
                    var row = '<option class="dropdown-item" value="' + Designation + '">' + Designation + '</option>';
                    dropdown.append(row);
                });
            }
        });
    }

    // //Add User Data:
    // $('#printbtn').on('click', function () {
    //     AddUser();
    //     console.log("clicked");
    //     GetAllUser();
    // });
    // function AddUser() {
    //     var formData = new FormData();
    //     formData.append('c_empname', $('#EmpName').val());
    //     formData.append('c_empgender', $('input[name="rdbtn"]:checked').val());
    //     formData.append('c_dob', $('#EmpDob').val().split('T')[0]);
    //     $('input[name="chkbox"]:checked').each(function () {
    //         formData.append('c_shift[]', $(this).val());
    //     });
    //     formData.append('c_department', $('#EditEmpDepartment').val());
    //     //formData.append('Image',$('#EmpImage')[0].files[0]);

    //     //save kri ne run karavje

    //     var img = $('#EmpImage')[0].files[0];
    //     $.ajax({
    //         url: 'https://localhost:7068/api/MVCApi/UserAddEmpData',
    //         type: 'POST',
    //         dataType: 'json',
    //         contentType: 'application/json',
    //         data: { emp: JSON.stringify(formData), file: img },
    //         success: function (data) {
    //             GetAllUser();
    //             successMsg(data.message);
    //         }
    //     });
    // }
    $("#addEmpForm").submit(function (event) {
        event.preventDefault();
        var formData = new FormData(this);
        console.log(formData);
        $.ajax({
            url: "https://localhost:7068/api/MVCApi/UserAddEmpData",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                GetAllUser();
                console.log(response);
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    });
    //Get All User Details:
    function GetAllUser() {
        var table = $('#TableContent11');
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
                    row += '<td> <img src="' + emp.c_empimage + '" alt="Image Not Found" style="height:50%;width:50%;"></td>';
                    row += '<td>';
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