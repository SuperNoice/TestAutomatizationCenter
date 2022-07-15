function Send() {

    var message = $("#message").val()

    if (message == "") {
        return
    }

    var user = $("#user").val()

    $.ajax({
        url: "SendMessage",
        method: "post",
        dataType: "html",
        data: { text: message, login: user },
        success: function (data) {
            $("#message").val("")
            AddMessage(data)
        }
    })
}

function AddMessage(message) {
    var msg = JSON.parse(message)
    var time = new Date(msg.timeStamp)
    $("#chat").append("<div style='overflow-wrap: break-word;'>" + msg.user.login + " (" + time.toLocaleString() + "): " + msg.text + "</div>")
    $("#chat").animate({ scrollTop: $("#chat").prop("scrollHeight") }, 1000);
}
