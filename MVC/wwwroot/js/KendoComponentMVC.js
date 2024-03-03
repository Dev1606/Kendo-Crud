// $(document).ready(function () {
//     var ImageFileName = "";
//     console.log("Welcome Employee");
//     UserGetEmpData();
//     getDropdownValues();
//     hideAlerts();
//     initializeGenderRadioButtons();
//     initializeShiftCheckboxes();
//     initializeDatePicker();
//     submitbtn();

//     function formatDateForInput(dateString) {
//         const dateObj = new Date(dateString);
//         const formattedDate = dateObj.toISOString().slice(0, 10);
//         return formattedDate;
//     }

//     function hideAlerts() {
//         $('#messageSuccess').hide();
//         $('#messageFail').hide();
//     }

//     function successMsg(str) {
//         $("#messageSuccess").text('');
//         $("#messageSuccess").append(str);
//         $("#messageSuccess").show().delay(3000).fadeOut();
//     }

//     function alertMsg(str) {
//         $("#messageFail").text('');
//         $("#messageFail").append(str);
//         $("#messageFail").show().delay(3000).fadeOut();
//     }

//     // for getting dropdown
//     function getDropdownValues() {
//         var dropdown = $("#DepartmentDorpDown");
//         dropdown.empty();
//         $.ajax({
//             url: '/KendoComponent/GetDepartment',
//             type: 'GET',
//             success: function (data) {
//                 dropdown.kendoDropDownList({
//                     dataSource: data,
//                     optionLabel: "Select Department"
//                 });
//             }
//         });
//     }

//     //get UserGetEmpData
//     function UserGetEmpData() {
//         var table = $('#TableContent');
//         table.empty();
//         $.ajax({
//             type: "GET",
//             url: "/KendoComponent/UserGetEmpData",
//             dataType: 'json',
//             success: function (emp) {
//                 emp.forEach(function (emp) {
//                     var row = '<tr>';
//                     row += '<td>' + emp.c_empid + '</td>';
//                     row += '<td>' + emp.c_empname + '</td>';
//                     row += '<td>' + emp.c_empgender + '</td>';
//                     row += '<td>' + emp.c_dob + '</td>';
//                     row += '<td>' + emp.c_shift + '</td>';
//                     row += '<td>' + emp.c_department + '</td>';
//                     row += '<td><img src="../wwwroot/uploadsimg/' + emp.c_empimage + '"></td>';
//                     row += '<td>';
//                     row += '</td>';
//                     row += '</tr>';
//                     table.append(row);
//                 });
//             }
//         });
//     }

//     function initializeGenderRadioButtons() {
//         $("#GenderRadioButton").kendoRadioGroup({
//             layout: "horizontal",
//             items: [
//                 { label: "Male", value: "Male" },
//                 { label: "Female", value: "Female" },
//                 { label: "Other", value: "Other" }
//             ]
//         });
//     }

//     function initializeShiftCheckboxes() {
//         $("#ShiftCheckbox").kendoCheckBoxGroup({
//             items: [
//                 { label: "Morning", value: "Morning" },
//                 { label: "Afternoon", value: "Afternoon" },
//                 { label: "Night", value: "Night" }
//             ],
//             layout: "horizontal"
//         });
//     }

//     function initializeDatePicker() {
//         $("#dob").kendoDatePicker({
//             format: "yyyy-MM-dd"
//         });
//     }

//     // Submit Button
//     function submitbtn() {
//         $("#SubmitButton").kendoButton({
//             themeColor: "primary",
//             click: function () {
//                 var formData = {
//                     empname: $("#EmpName").val(),
//                     empgender: $("input[name='GenderRadioButton']:checked").val(),
//                     dob: $("#dob").val(),
//                     shift: $("#ShiftCheckbox").val(),
//                     empimage: ImageFileName,
//                     department: $("#DepartmentDorpDown").data("kendoDropDownList").value()
//                 };

//                 $.ajax({
//                     url: "/KendoComponent/UserAddEmpData",
//                     type: "POST",
//                     contentType: "application/json",
//                     data: JSON.stringify(formData),
//                     success: function (data) {
//                         if (data.success == true) {
//                             successMsg("Employee data added successfully.");
//                             UserGetEmpData(); // Refresh employee data table
//                         } else {
//                             alertMsg("Failed to add employee data.");
//                         }
//                     },
//                     error: function (xhr, status, error) {
//                         alertMsg(xhr.responseText);
//                     }
//                 });
//             }
//         });
//     }

// });
// -------------------------------------
$(document).ready(function(){
    var user = {
        c_uname: $('#txtName').val(),
        c_uemail: $('#txtEmail').val(),
        c_password: $('#txtPassword').val()
    };  
    // Name
    $('#txtName').kendoTextBox({
        label: {
            content: "User Name",
            floating: true
        },
        change: function(){
            user.c_uname = kendo.toString(this.value());
            console.log(user.c_uname);
        }
    });
    // Email
    $('#txtEmail').kendoTextBox({
        label: {
            content: "Email",
            floating: true
        },
        change: function(){
            user.c_uemail = kendo.toString(this.value());
            console.log(user.c_uemail);
        }
    });
    // Password
    $('#txtPassword').kendoTextBox({
        label: {
            content: "Password",
            floating: true
        },
        change: function(){
            user.c_password = kendo.toString(this.value());
            console.log(user.c_password);
        }
    });

    // ConfirmPassword
    $('#txtConfirmPassword').kendoTextBox({
        label: {
            content: "ConfirmPassword",
            floating: true
        },
    });
    $("#SubmitButton").kendoButton({
        themeColor: "primary",
        click: function(){
            console.log(user);
            $.ajax({
                url: "/KendoComponent/Register",
                type: "POST",
                data:user,
                success: function (data){
                    console.log(JSON.stringify(user));
                    console.log(data);
                    console.log(data.success);
                    if(data.success == true){
                        alert("Register successfully");
                        window.location.href="/KendoComponent/Login";
                    }else{
                        alert("Fail to register");
                        window.location.href="/KendoComponent/Register";
                    }
                }
            });
        }
    });
    var login = {
        c_uemail: $('#txtlgEmail').val(),
        c_password: $('#txtlgPassword').val()
    }; 
    // Email
    $('#txtlgEmail').kendoTextBox({
        label: {
            content: "Email",
            floating: true
        },
        change: function(){
            login.c_uemail = kendo.toString(this.value());
            console.log(login.c_uemail);
        }
    });
    
    // Password
    $('#txtlgPassword').kendoTextBox({
        label: {
            content: "Password",
            floating: true
        },
        change: function(){
            login.c_password = kendo.toString(this.value());
            console.log(login.c_password);
        }
    });
    $("#LoginButton").kendoButton({
        themeColor: "primary",
        click: function(){
            console.log(login);
            $.ajax({
                url: "/KendoComponent/Login",
                type: "POST",
               data:login,
            }).done(function (data) {
                // debugger
                // if(data.success == true){
                //     alert("Login Successfully...");
                //     window.location.href="/KendoComponent/AdminIndex";
                // }else{
                //     alert("Invalid...");
                //     window.location.href="/KendoComponent/AdminIndex";
                // }
                if(data.success==true){
                    var urlstr="/"+data.controller+"/"+data.action;
                    window.location.href=urlstr;
                }
                else{
                    alert("Invalid Credentials");
                }
            });
        }
    });
});