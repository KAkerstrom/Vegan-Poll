$(document).ready(function () {
    $(".PollBox").append(RadioBox('gr', 'yes', 'Yes'));
    $(".PollBox").append(RadioBox('gr', 'no', 'No'));
    $(".PollBox").append(RadioBox('gr', 'maybe', 'Maybe So'));
    $(".PollBox").append('<div class="SubmitButton">Submit!</div>');

    $(".PollBox").children('.SubmitButton').on('click', function () {
        API('add_poll', { PollQuestion: "How large is Mark's vagina?", Answers: "Too small, Too big, I mean he is a giant cunt." }, function () {

        });
    });
});

function API(action, data, c) {
    $.post('/PollAPI.aspx', { action: action, data: data }, function (success, data) {
        console.log(success, data);
    });
}