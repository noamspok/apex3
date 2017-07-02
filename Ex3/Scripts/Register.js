$(function () {
    if (sessionStorage.getItem("loggedInUser") === null) {
        $("#nav-placeholder").load("navigationbar.html");
    }
    else {
        $("#nav-placeholder").load("SignedNavBar.html");
    }
});


ko.validation.rules['mustEqual'] = {
    validator: function (val, otherVal) {
        return val === otherVal;
    },
    message: "Passwords don't match!"
};

ko.validation.registerExtenders();

function AppViewModel() {
    var that = this;
    this.UserName = ko.observable("").extend({ required: { message: "Please enter a UserName" } });
    this.Password = ko.observable("").extend({ required: { message: "Please enter your Password" } });
    this.Password2 = ko.observable("").extend({
        required: { message: "Passwords don't match!" },
        mustEqual: that.Password 

    });
    this.Email = ko.observable("").extend({ required: { message: "Please enter your E-mail" } });
    
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
        $.post(apiUrl, JsonData, function (result) {
            sessionStorage.loggedInUser = ko.toJS(that.UserName);
            window.location.href = "Maze.html";
        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert(textStatus + " " + errorThrown);
        });

    };
    
    this.isFormValid = ko.computed(function () {

        return this.Password() && this.Password2() && this.UserName() && this.Email() && this.Password2.isValid();
    }, this);
}

ko.applyBindings(new AppViewModel());