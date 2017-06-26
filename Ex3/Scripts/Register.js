function AppViewModel() {
    this.UserName = ko.observable("Enter Your User Name");
    this.Password = ko.observable("Enter Your Password");
    this.Password2 = ko.observable("Verify Your Password");
    this.Email = ko.observable("Enter Your E-Mail");
    
    this.RegBtn = function () {
        var JsUser = ko.toJS(this.UserName);
        var JsPass = ko.toJS(this.Password);
        var JsEmail = ko.toJS(this.Email);
        var JsonData = {
            "UserName": JsUser,
            "Password": JsPass,
            "Email": JsEmail
        };
        var apiUrl = "../api/Registry";
        $.post(apiUrl, JsonData, function (result) { alert(result) });
    };
    
}

ko.applyBindings(new AppViewModel());