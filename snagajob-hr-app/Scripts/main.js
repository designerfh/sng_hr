var resultlist, currentResult, applicationList, username, userId, role;

$(document).ready(function () {
    username = sessionStorage.getItem('username');
    userId = sessionStorage.getItem('userId');
    role = sessionStorage.getItem('role');

    switch (role) {
        case 'applicant':
            $('#applicant').show();
            $('#hr').hide();
            $('#hradmin').hide();
            loadApplicantResults(userId, role);
            break;
        case 'hr':
            $('#applicant').hide();
            $('#hr').show();
            $('#hradmin').hide();
            loadResults();
            break;
        case 'hradmin':
            $('#applicant').hide();
            $('#hr').hide();
            $('#hradmin').show();
            loadApplications();
            break;
    }

    $('.logout').on('click', function () {
        cleanUpAndRedirect();
    });

    $('#submitApp').on('click', function () {
        submitApplication();
    });

    $('#compApps').on('change', function () {
        var appName = $(this).find(":selected").text();
        loadResult(appName);
    });

    $('#saveAddNew').on('click', function () {
        createNewUserAndResult();
    });

    $('#applicationList').on('change', function () {
        var appId = $('#applicationList option:selected').val();
        loadApplication(appId);
    });

    $('#addApplication').on('click', function () {
        newApplication();
    });

    $('#saveApplication').on('click', function () {
        saveApplication();
    });

    $('#delApplication').on('click', function () {
        deleteApplication();
    });

    loadModal();
});


//application (roleId = 1) functions

function loadApplicantResults(userId, role) {
    $.ajax({
        url: "/v1/Result/" + userId,
        type: 'Get',
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            if (data != null)
            {
            currentResult = data;
            loadApplicantQuestions(data, role);
            }
            else
            {
                alert("You have no applications at this time. ")
            }
        }
    });
}

function loadApplicantQuestions(result, role) {
    $.ajax({
        url: "/v1/Application?id=" + result.Application_Id + '&role=' + role,
        type: 'Get',
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            $('#applicationName').text(data.Name + ' Application Questions:')
            data.Questions.forEach(function (ele, index, arr) {
                var selectedAnswer = _.find(result.Answers, function (answer) { return answer.QuestionText == ele.QuestionText });

                var optText = "";
                ele.Answers.forEach(function (el, ind, ar) {
                    var selectedText = "";
                    if (selectedAnswer !== undefined && el.AnswerText == selectedAnswer.AnswerText) {
                        selectedText = "selected=true";
                    }
                    optText += '<option value=' + ind + ' ' + selectedText + '>' + el.AnswerText + '</option>'
                });
                $('#questionTable tbody').append('<tr><td>' + ele.QuestionText + '</td><td><select>' + optText + '</select></td></tr>');
            });
        }
    });
};

function submitApplication() {
    currentResult.Answers = [];

    var rows = $('#questionTable').find('tr').not(":first").toArray();
    rows.forEach(function (ele, index, arr) {
        var questionText = $(ele).find("td:first").text();
        var answerSelected = $(ele).find("select");
        var answerText = $(answerSelected).find(":selected").text();

        var newQues = {
            QuestionText: questionText,
            AnswerText: answerText
        };
        currentResult.Answers.push(newQues);
    });

    $.ajax({
        url: "/v1/Result",
        type: 'Put',
        dataType: 'text json',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(currentResult),
        success: function (data) {
            alert('Your application has been submitted');
            cleanUpAndRedirect();
        }
    });
};

//

//hr (roleId = 2) functions

function loadModal() {
    $.ajax({
        url: "/v1/Application/Names",
        type: 'Get',
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            var optText = "";
            data.forEach(function (ele, ind, arr) {
                optText += '<option value=' + ele.Id + '>' + ele.Name + '</option>'
            });
            $('#addNewApplication').empty();
            $('#addNewApplication').append(optText);
        }
    });
};

function loadResults() {
    $.ajax({
        url: "/v1/Result",
        type: 'Get',
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            resultlist = data;
            var optText = "";
            data.forEach(function (ele, ind, arr) {
                optText += '<option>' + ele.ApplicationName + ' - ' + ele.UserName + '</option>';
            });
            $('#compApps').append(optText);
        }
    });
};

function loadResult(resultName) {
    $('#answerTable tbody').empty();

    var appResult = _.find(resultlist, function (result) { return (result.ApplicationName + ' - ' + result.UserName) == resultName });
    appResult.Answers.forEach(function (ele, index, arr) {
        $('#answerTable tbody').append('<tr><td>' + ele.QuestionText + '</td><td>' + ele.AnswerText + '</td></tr>');
    });
};

function createNewUserAndResult() {
    var newUser = {
        Name: $('#addNewUserName').val(),
        Password: $('#addNewUserPwd').val(),
        Role: 'applicant'
    };

    $.ajax({
        url: "/v1/User",
        type: 'Post',
        dataType: 'text json',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(newUser),
        success: function (data) {
            newUser._id = data._id;
        }
    }).then(function () {
        var newResult = {
            Application_ID: $('#addNewApplication').val(),
            ApplicationName: $('#addNewApplication').find(":selected").text(),
            User_Id: newUser._id,
            UserName: newUser.Name
        };
        createApplicationResult(newResult);
    });
};

function createApplicationResult(newResult) {
    $.ajax({
        url: "/v1/Result",
        type: 'Post',
        dataType: 'text json',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(newResult),
        success: function (data) {
            clearAndHideModal();
        }
    });
};

function clearAndHideModal() {
    $('#addNewUserName').val("");
    $('#addNewUserPwd').val("");
    $('#addNewApplication')[0].selectedIndex = 0;

    $('#modalAddNew').modal('hide');
};

//

//hradmin (roleId = 3) functions

function loadApplications() {
    $.ajax({
        url: "/v1/Application/Names",
        type: 'Get',
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            var optText = "";
            data.forEach(function (ele, ind, arr) {
                optText += '<option value=' + ele.Id + '>' + ele.Name + '</option>'
            });
            $('#applicationList').empty();
            $('#applicationList').append(optText);
        }
    });
};

function loadApplication(appId) {
    $.ajax({
        url: "/v1/Application?id=" + appId + '&role=' + role,
        type: 'Get',
        dataType: 'text json',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            $('#adminAnswerTable tbody').empty();
            $('#newApplicationName').val(data.Name).attr('data-appId', appId);


            data.Questions.forEach(function (ele, index, arr) {
                var optText = "";
                ele.Answers.forEach(function (el, ind, ar) {
                    var checked;
                    if (el.Correct == true) {
                        checked = 'checked';
                    }
                    else {
                        checked = '';
                    }
                    optText += '<div><input type="text" placeholder="Enter Answer" value="' + el.AnswerText + '" /><input type="checkbox" ' + checked + ' /></div>'
                });
                var addblanks = 4 - ele.Answers.length;
                for (var i = 0; i < addblanks; i++) {
                    optText += '<div><input type="text" placeholder="Enter Answer" /><input type="checkbox" /></div>'
                }
                $('#adminAnswerTable tbody').append('<tr><td><textarea rows="6" >' + ele.QuestionText + '</textarea></td><td>' + optText + '</td></tr>');
            });

            if (data.Questions.length != 6) {
                for (var i = 0; i < 6 - data.Questions.length; i++) {
                    var optText = "";
                    for (var ii = 0; ii < 4; ii++) {
                        optText += '<div><input type="text" placeholder="Enter Answer" /><input type="checkbox" /></div>'
                    }
                    $('#adminAnswerTable tbody').append('<tr><td><textarea rows="6" ></textarea></td><td>' + optText + '</td></tr>');
                }
            }
        }
    });
};

function newApplication() {
    $('#adminAnswerTable tbody').empty();
    $('#newApplicationName').val('').removeAttr('data-appId');

    for (var i = 0; i < 10; i++) {
        var optText = "";
        for (var ii = 0; ii < 4; ii++) {
            optText += '<div><input type="text" placeholder="Enter Answer" /><input type="checkbox" /></div>'
        }
        $('#adminAnswerTable tbody').append('<tr><td><textarea rows="6" ></textarea></td><td>' + optText + '</td></tr>');
    }
};

function saveApplication() {
    var questions = [];

    var rows = $('#adminAnswerTable tbody').find('tr').toArray();
    rows.forEach(function (ele, index, arr) {
        var questionTxt = $(ele).find('textarea').val();
        if (questionTxt != "") {
            var answerList = [];
            var answers = $(ele).find('input[type=text]').toArray();
            answers.forEach(function (el, ind, ar) {
                var answerText = $(el).val();
                if (answerText != "") {
                    var $chkbx = $(el).siblings('input[type=checkbox]');
                    var isChecked = $chkbx.is(':checked');
                    var newAns = {
                        AnswerText: answerText,
                        Correct: isChecked
                    }
                    answerList.push(newAns);
                }
            });

            var question = { QuestionText: questionTxt, Answers: answerList };
            questions.push(question);
        }
    });

    var saveAppName = $('#newApplicationName').val();
    var saveAppId = $('#newApplicationName').attr('data-appid');

    var action;
    var application = { Name: saveAppName, Questions: questions };

    if (saveAppId != undefined) {
        application._id = saveAppId;
        action = "Put";
    }
    else {
        action = "Post";
    }

    $.ajax({
        url: "/v1/Application",
        type: action,
        dataType: 'text json',
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(application),
        success: function (data) {
            if (action == "Post") {
                alert('New application has been submitted');
            }
            else {
                alert('Application has been updated');
            }

            loadApplications();
            newApplication();
        }
    });


};

function deleteApplication() {
    var delAppId = $('#newApplicationName').attr('data-appId');

    $.ajax({
        url: "/v1/Application?id=" + delAppId,
        type: 'Delete',
        dataType: 'text json',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            alert('Application has been deleted');

            loadApplications();
            newApplication();
        }
    });
};


//

function cleanUpAndRedirect() {
    sessionStorage.removeItem('username');
    sessionStorage.removeItem('userId');
    sessionStorage.removeItem('role');

    window.location.replace('/Login.html');
}