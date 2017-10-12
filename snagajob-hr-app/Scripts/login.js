$(document).ready(function () {

    if (sessionStorage.getItem('username') != null) {
        window.location.replace('/Main.html');
    }

    $('#btnLogin').on('click', function () {

        var username = $('#username').val();
        var password = $('#password').val();

        $.ajax({
            url: "/v1/User/Login",
            type: 'Post',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Authorization", "Basic " + btoa(username + ":" + password));
            },
            success: function (data, status, httpObj) {
                if (httpObj.status == 200) {
                    data.forEach(function (ele, ind, arr) {
                        sessionStorage.setItem(ele.Key, ele.Value);
                    });
                    window.location.replace('/Main.html');
                }
            },
            error: function (httpObj) {
                if (httpObj.status == 401 || httpObj.status == 403) {
                    alert('You are not authorized to log in. Please contact someone.');
                }
            }
        });
    });

});