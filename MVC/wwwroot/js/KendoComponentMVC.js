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