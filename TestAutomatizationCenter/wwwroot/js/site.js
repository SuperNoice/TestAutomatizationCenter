function LoadMesseges() {
    $.ajax({
        url: "GetMessages",
        method: "get",
        success: function (data) {
            $("#message").val("")
            for (var i = 0; i < data.length; i++) {
                AddMessage(data[i])
            }
            ScrollChatToEnd()
        }
    })
}

function Send() {

    var message = $("#message").val()

    if (message == "") {
        return
    }

    var user = $("#user").val()

    $.ajax({
        url: "SendMessage",
        method: "post",
        data: { text: message, login: user },
        success: function (data) {
            $("#message").val("")
            AddMessage(data)
            ScrollChatToEnd()
        }
    })
}

function AddMessage(message) {
    var time = new Date(message.timeStamp)
    $("#chat").append("<div style='overflow-wrap: break-word;'>" + message.user.login + " (" + time.toLocaleString() + "): " + message.text + "</div>")
}

function ScrollChatToEnd() {
    $("#chat").animate({ scrollTop: $("#chat").prop("scrollHeight") }, 1000);
}
