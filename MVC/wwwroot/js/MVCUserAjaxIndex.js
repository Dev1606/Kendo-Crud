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
                    row += '<td><img src="../wwwroot/uploadsimg/'+emp.c_empimage+'"></td>';
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

        //Adding Employee
        $('#printbtn').on('click', function () {
            console.log("HI");
            AddEmployee();
            GetAll();
        });
        function AddEmployee() {
            var formData = new FormData();
            formData.append('c_empname',$('#EmpName').val());
            formData.append('c_empgender',$("input[name='EmpGender']:checked").val());
            formData.append('c_dob',$('#EmpDob').val().split('T')[0]);
            formData.append('c_shift',$('input[name="EmpShift"]:checked').map(function () { return this.value; }).get());
            formData.append('c_department',$("#EmpDepartment").val());
            formData.append('Image',$('#EmpImage')[0].files[0]);
            // Debug
            // console.log(student.c_studimage);
    
            $.ajax({
                url: '/MVCAjax/UserAddEmpData',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
            }).done((data) => {
                successMsg(data.message);
                GetAll();
            });
    
        }

});