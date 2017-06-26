function AppViewModel() {
    this.UserName = ko.observable("Enter Your User Name");
    this.Password = ko.observable("Enter Your Password");

    this.LogInClicked = function () {
        var apiUrl = "../api/Registry/" + this.UserName();
        $.ajax({
            method: "GET",
            url: apiUrl
        }).done(function (User) {
           
            if (pass == this.Password()) {
                sessionStorage.loggedInUser = User.UserName;
                alert("hello" + User.UserName);
            }

        });
    }
}

ko.applyBindings(new AppViewModel());