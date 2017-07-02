
$(function () {
    if (sessionStorage.getItem("loggedInUser") === null) {
        $("#nav-placeholder").load("navigationbar.html");
    }
    else {
        $("#nav-placeholder").load("SignedNavBar.html");
    }
    });
 

function AppViewModel() {
    var that = this;
    this.UserName = ko.observable("");
    this.Password = ko.observable("");

    this.LogInClicked = function () {
        var apiUrl = "../api/Registry/" + this.UserName();
        $.ajax({
            method: "GET",
            url: apiUrl
        }).done(function (User) {
            if (typeof User[0] !== 'undefined') {
                var pass = User[0]["Password"];
                if (pass == ko.toJS(that.Password)) {
                    sessionStorage.loggedInUser = User[0]["UserName"];
                    window.location.href = "Maze.html";
                }
                else {
                    alert("wrong password");
                }

            }
            else {
                alert("wrong UserName");
            }


        });
    };
    this.isFormValid = ko.computed(function () {

        return this.Password() && this.UserName();
    }, this);
}

ko.applyBindings(new AppViewModel());