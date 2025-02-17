﻿$(document).ready(function () {

    console.log("htis");
    console.log($('.vbmsg'));
    if ($('.vbmsg').length > 0) {
        console.log($('.vbmsg'));
        setTimeout(function () {
            $('.vbmsg').fadeOut("slow");

        },4000); //waits for 4 sec before fading out
    }

    $('#email').on('change', function () {
        var email = $(this).val();

        $.ajax({
            type: 'POST',
            url: '/Customer/ValidateEmail',
            dataType: 'json',
            data: { email: email },
            success: function (response) {
                if (!response.success) {
                    //    $('#email-error').text(response.message).show(); //using tooltip instead of span
                    $('#email').tooltip('dispose')
                        .attr('title', response.message)
                        .tooltip('show')
                        .css({ "background-color": "white", "color": "red" });
                    $('#loginbtn').prop('disabled', true)
                } else {
                    //    $('#email-error').hide();
                    $('#email').tooltip('dispose');
                    $('#loginbtn').prop('disabled', false)

                }
            },
            error: function (xhr, status, error) {
                console.error('An error occurred: ' + error);
            }
        });
    });




    $('#rEmail').on('change', function () {
        var email = $(this).val();
        console.log(email);
        console.log("not working")
        $.ajax({
            type: 'POST',
            url: '/Customer/ValidateRegisterEmail',
            dataType: 'json',
            data: { email: email },
            success: function (response) {
                if (!response.success) {
                    //    $('#email-error').text(response.message).show(); //using tooltip instead of span
                    $('#rEmail').tooltip('dispose')
                        .attr('title', response.message)
                        .tooltip('show')
                        .css({ "background-color": "white", "color": "red" });
                    $('#regBtn').prop('disabled', true)

                } else {
                    //    $('#email-error').hide();
                    $('#rEmail').tooltip('dispose')
                    $('#regBtn').prop('disabled', false)

                }
            },
            error: function (xhr, status, error) {
                console.error('An error occurred: ' + error);
            }
        });
    });

    


});
