$(document).ready(function () {
    $(".PollBox").append(RadioBox('gr', 'yes', 'Yes'));
    $(".PollBox").append(RadioBox('gr', 'no', 'No'));
    $(".PollBox").append(RadioBox('gr', 'maybe', 'Maybe So'));
});

function RadioBox(group, name, label) {

    var tr = $('<div class="RadioBox">');

    tr.attr('id', name);

    tr.append('<div class="Button"></div>')
    tr.append($('<div class="Label">').append(label));

    return $('<div class="Answer">').append(tr);
}