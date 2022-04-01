// Write your JavaScript code.

function postComment(userID, doctorID) {
    var IDUser = userID;
    var IDDoctor = doctorID;
    var comment = $('#txtComment').val();

    if (comment == null || comment == "")
        return;

    $.ajax({
        type: 'POST',
        url: '/Comment/PostComment',
        data: { "comment": comment, "userId": IDUser, "doctorId": IDDoctor },
        complete: function () {
            $("#txtComment").val("");
            $.ajax({
                type: 'GET',
                url: '/Comment/GetComment',
                contentType: 'application/json',
                data: { "doctorId": IDDoctor },
                dataType: 'HTML',
                success: function (data) {
                    $('#commentContent').html(data);
                }
            });
        }
    });
}
