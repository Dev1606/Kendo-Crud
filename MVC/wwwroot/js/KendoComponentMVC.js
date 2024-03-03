$(document).ready(function(){

    var user = {
        c_uid: 0,
        c_uname: $('#txtName').val(),
        c_uemail: $('#txtEmail').val(),
        c_password: $('#txtPassword').val(),
        c_confirmpassword: $('#txtConfirmPassword').val()
    };  

    // Name
    $('#txtName').kendoTextBox({
        label: {
            content: "User Name",
            floating: true
        },
        change: function(){
            user.c_ename = kendo.toString(this.value());
        }
    });

    // Email
    $('#txtEmail').kendoTextBox({
        label: {
            content: "Email",
            floating: true
        },
        change: function(){
            user.c_email = kendo.toString(this.value());
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
            // console.log(user);
            $.ajax({
                url: "",
                type: "POST",
                // contentType: "application/json",
                // data: JSON.stringify(user),
                data: user,
                success: function (data) {
                    if(data.success == true){
                        window.location.href="/User/Login";
                    }else{
                        window.location.href="/User/Register";
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
            user.c_email = kendo.toString(this.value());
        }
    });

    // Password
    $('#txtlgPassword').kendoTextBox({
        label: {
            content: "Password",
            floating: true
        },
        change: function(){
            user.c_password = kendo.toString(this.value());
        }
    });

    $("#LoginButton").kendoButton({
        themeColor: "primary",
        click: function(){
            // console.log(login);
            $.ajax({
                url: "/KendoComponent/Login",
                type: "POST",
                //contentType: "application/json",
                // data: JSON.stringify(login),
                data: user,
                
            }).done(function (data) {
                if(data.success == true){
                    console.log(data);
                    alert("Login Successfully...");
                    window.location.href="/KendoComponent/AdminIndex";
                }else{
                    alert("Invalid...");
                    console.log(data);
                    //window.location.href="/KendoComponent/AdminIndex";
                }
            });
        }
    });
});