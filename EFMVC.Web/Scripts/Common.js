$(document).ready(function () {
    if (window.location.href.toLowerCase().indexOf("admindashboard/index") > -1) {
        $("#adminIndex").addClass("active");
        $("#adminHelp").removeClass("active");
    }
    
    else if (window.location.href.toLowerCase().indexOf("dashboard") > -1) {
        $("#Campaigns").addClass("active");
        $("#Clients").removeClass("active");
        $("#Adverts").removeClass("active");
        $("#Accounts").removeClass("active");
        $("#Billing").removeClass("active");
        $("#Help").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#Currency").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("client") > -1) {
        $("#Clients").addClass("active");
        $("#Campaigns").removeClass("active");
        $("#Adverts").removeClass("active");
        $("#Accounts").removeClass("active");
        $("#Billing").removeClass("active");
        $("#Help").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#Currency").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("advert") > -1) {
        $("#Adverts").addClass("active");
        $("#Clients").removeClass("active");
        $("#Campaigns").removeClass("active");
        $("#Accounts").removeClass("active");
        $("#Billing").removeClass("active");
        $("#Help").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#Currency").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("account") > -1) {
        $("#Accounts").addClass("active");
        $("#Adverts").removeClass("active");
        $("#Clients").removeClass("active");
        $("#Campaigns").removeClass("active");
        $("#Billing").removeClass("active");
        $("#Help").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#Currency").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("userprofile") > -1) {
        $("#userprofile").addClass("active");
        $("#Accounts").addClass("active");
        //$("#Accounts").removeClass("active");
        $("#Adverts").removeClass("active");
        $("#Clients").removeClass("active");
        $("#Campaigns").removeClass("active");
        $("#Billing").removeClass("active");
        $("#Help").removeClass("active");
        $("#Currency").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("billing") > -1) {
        $("#Billing").addClass("active");
        $("#Accounts").removeClass("active");
        $("#Adverts").removeClass("active");
        $("#Clients").removeClass("active");
        $("#Campaigns").removeClass("active");
        $("#Help").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#Currency").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("currency") > -1) {
        $("#Currency").addClass("active");
        $("#Billing").removeClass("active");
        $("#Accounts").removeClass("active");
        $("#Adverts").removeClass("active");
        $("#Clients").removeClass("active");
        $("#Campaigns").removeClass("active");
        $("#Help").removeClass("active");
        $("#userprofile").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("help") > -1) {
        $("#Help").addClass("active");
        $("#Billing").removeClass("active");
        $("#Accounts").removeClass("active");
        $("#Adverts").removeClass("active");
        $("#Clients").removeClass("active");
        $("#Campaigns").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#Currency").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("adminquestion") > -1) {
        $("#adminIndex").removeClass("active");
        $("#adminHelp").addClass("active");
    }

// 22-05-2019 common for check only numeric/digit only allowed

    $(".onlyDigit").keypress(function (e) {
        //if the letter is not digit then display error and don't type anything
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });

    //15-07-2019 common for check only numeric allowed
    $(".only-numeric").keypress(function (e) {
        var keyCode = e.which ? e.which : e.keyCode

        if (!(keyCode >= 48 && keyCode <= 57)) {
            $(".error").css("display", "inline");
            return false;
        } else {
            $(".error").css("display", "none");
        }
    });
    
    $(".kenyaPhoneLength").keypress(function (e) {
        //if the letter is not digit then display error and don't type anything
        if (this.value.length > 11) {
            //display error message
            // $("#errmsg").html("Numeric values only allowed").show().fadeOut("slow");
            return false;
        }
    });

    $(".trimmed").on('blur', function () {
        // the following function will be executed every half second
        if ($(this).val() != null) {
            $(this).val($(this).val().toString().trim());
        }
        var s = $(this).val().replace(/\</g, '');
        s = s.replace(/\>/g, '');
        $(this).val(s);
    });

    $(".digitLength").keypress(function (e) {
        //if the letter is not digit then display error and don't type anything
        if (this.value.length > 25) {
            //display error message
            // $("#errmsg").html("Numeric values only allowed").show().fadeOut("slow");
            return false;
        }
    });
});

//Add 28-05-2019 Check Decimal Value
function isNumber(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (
        //(charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
        return false;
    return true;
}

//Add 15-05-2019
//Function for Advertiser
function advertiserMultipleTabLogout() {
    function lsTest() {
        var test = 'test';
        try {
            localStorage.setItem(test, test);
            localStorage.removeItem(test);
            return true;
        } catch (e) {
            return false;
        }
    }

    // listen to storage event
    window.addEventListener('storage', function (event) {
        // do what you want on logout-event
        if (event.key == 'logout-event') {
            //$('#console').html('Received logout event! Insert logout script here.');
            //window.location = "logout.php";
            window.location.pathname = "/";
        }
    }, false);

    $(document).ready(function () {
        if (lsTest()) {
            $('#logoutbtn').on('click', function () {
                // change logout-event and therefore send an event
                localStorage.setItem('logout-event', 'logout' + Math.random());
                return true;
            });
        } else {
            // setInterval or setTimeout
        }
    });
}

function popupAdvertiserMultipleTabLogout() {
    function lsTest() {
        var test = 'test';
        try {
            localStorage.setItem(test, test);
            localStorage.removeItem(test);
            return true;
        } catch (e) {
            return false;
        }
    }

    // listen to storage event
    window.addEventListener('storage', function (event) {
        // do what you want on logout-event
        if (event.key == 'logout-event') {
            //$('#console').html('Received logout event! Insert logout script here.');
            //window.location = "logout.php";
            window.location.pathname = "/";
        }
    }, false);

    $(document).ready(function () {
        if (lsTest()) {
                // change logout-event and therefore send an event
                localStorage.setItem('logout-event', 'logout' + Math.random());
                return true;
        } else {
            // setInterval or setTimeout
        }
    });
}

//Function for Advertiser
function sessionTimeOut() {
    var url = $("#dialog").data('request-url');

    String.prototype.format = function () {
        var s = this,
            i = arguments.length;
        while (i--) {
            s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
        }
        return s;
    };

    var pocTimeoutDialog = {
        calculateTimer: 0,
        settings: {
            timeout: 1200000000, //2000000 //1200000000 //20 minutes
            countdown: 20, //20 seconds
            title: 'Your session is about to expire!',
            message: 'You will be logged out in {0} seconds.',
            question: 'Do you want to continue with your session?',
            keep_alive_url: window.location.href,
            logout_redirect_url: 'Landing/Index'
        },
        init: function () {
            // alert('Hi');
            this.setupDialogTimer();
        },
        setupDialogTimer: function () {
            var self = this;
            self.setTimer = window.setTimeout(function () {
                self.setupDialog();
            }, (this.settings.timeout - this.settings.countdown) * 1000);
        },
        clearDialogTimer: function () {
            var self = this;
            window.clearTimeout(self.setTimer);
            self.init();
        },
        setupDialog: function () {
            var self = this;
            self.destroyDialog();

            $('<div class="modal-dialog modal-sm">' +
                '<div class="modal-content">' +
                '<div class="modal-header">' +
                '<div class="row">' +
                '<label class="col-lg-12 control-label" style="text-align: center;font-size: medium;">' + this.settings.title + '</label>' +
                '</div>' +
                '</div>' +
                '<div class="modal-body">' +
                '<form class="form-horizontal">' +
                '<div class="form-group">' +
                '<div class="row">' +
                '<label class="col-lg-12 control-label" style="text-align: center;">' + this.settings.message.format(' <span id = "sessionTimeoutCountdown" class= "session-timeout-countdown" > ' + this.settings.countdown + '</span>') + '</label>' +
                '</div>' +
                '<div class="row">' +
                '<label class="col-lg-12 control-label" style="text-align: center;">' + this.settings.question + '</label>' +
                '</div>' +
                '</div>' +
                '</form>' +
                '</div>' +
                '<div class="modal-footer">' +
                '<button id="dialogKeepSession" data-dismiss="modal" class="dialog-keep-session-btn btn btn-sm btn-blue">Yes, Keep me signed in</button> &nbsp;' +
                '<button id="dialogSignOut" data-dismiss="modal" class="dialog-sign-out-btn btn btn-sm btn-blue">No, Sign me out</button>' +
                '</div>' +
                '</div>' +
                '</div>').appendTo('#dialog');

            $("#dialog").modal();
            $("#seconds").html(this.settings.countdown);
            $('#dialogKeepSession').on('click', function () {
                self.keepAlive();
                $("#dialog").hide();
            });
            $('#dialogSignOut').on('click', function () {
                self.signOut();
                $("#dialog").hide();
            });
            self.startCountdown();
        },
        destroyDialog: function () {
            if ($("#sessionTimeoutDialog").length) {
                $('#sessionTimeoutDialog').remove();
            }
        },
        startCountdown: function () {
            var self = this,
                counter = this.settings.countdown;
            self.clearCountdown();
            this.calculateTimer = window.setInterval(function () {
                counter -= 1;
                $("#sessionTimeoutCountdown").html(counter);
                if (counter <= 0) {
                    self.clearCountdown();
                    self.signOut();
                }
            }, 1000);
        },
        clearCountdown: function () {
            window.clearInterval(this.calculateTimer);
            this.calculateTimer = undefined;
        },
        keepAlive: function () {
            sessionTimeoutStatus = true;
            var self = this;
            self.destroyDialog();
            $.ajax({
                url: this.settings.keep_alive_url,
                type: "GET",
                cache: false,
                success: function () {
                    self.clearCountdown();
                    // self.init();
                },
                error: function () {
                    self.signOut();
                }
            });
        },
        signOut: function () {
            var self = this;
            self.destroyDialog();
            $.ajax({
                type: "GET",
                url: url,
                cache: false,
                success: function () {
                    self.redirectLogout();
                },
                error: function () {
                    self.signOut();
                }
            });
            //self.redirectLogout();
        },
        redirectLogout: function () {
            //window.location = this.settings.logout_redirect_url;
            var url = 'Landing/Index';
            if (typeof (history.pushState) != "undefined") {
                var obj = { Page: 'Logout', Url: url };
                history.pushState(obj, obj.Page, obj.Url);
            } else {
                alert("Browser does not support HTML5.");
            }

            window.location.pathname = this.settings.logout_redirect_url;
        }
    };

    $(function () {
        pocTimeoutDialog.init();
    });
}