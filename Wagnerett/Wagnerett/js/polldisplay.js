var DummyPoll = {
    id: "A67ggBgt298dsdfH67",
    question: "Do you bees?",
    type: 1,
    created: "Feb 28, 2018 6:42AM",
    closed: null,
    answers: [
        {
            id: 23,
            text: "Yes",
            votes: 17
        },
        {
            id: 24,
            text: "No",
            votes: 2
        },
        {
            id: 25,
            text: "Maybe So",
            votes: 10
        }
    ]
};

function GenVoteBox(poll) {
    var tr = $('<div class="PollBox">');

    tr.append($('<div class="Question">').append(poll.question));

    poll.answers.forEach(function (answer) {
        tr.append(RadioBox(poll.id, answer.id, answer.text));
    });

    tr.append('<div class="SubmitButton">Submit!</div>');

    return tr;
};

function GenResultsBox(poll) {
    var tr = $('<div class="PollBox">');
    var tv = SumVotes(poll);

    tr.append($('<div class="Question">').append(poll.question));

    poll.answers.forEach(function (answer) {
        var ab = $('<div class="Answer">');
        var rb = $('<div class="RadioBox">');
        var lb = $('<div class="Label">');

        var per = Math.floor((answer.votes / tv) * 100.0);

        lb.append(answer.text);

        var pb = $('<div class="Percent">');
        pb.text(per + "%");

        var tb = $('<div class="Tally">');
        tb.append(answer.votes + " votes");

        if (per < 50)
            tb.css({
                'right': '-105px',
                'text-align': 'left',
                'color': '#999'
            });

        pb.append(tb);

        pb.css('width', per + '%');

        lb.append(pb);

        rb.append(lb);
        ab.append(rb);

        tr.append(ab);
    });

    return tr;
}

function SumVotes(poll) {
    var tr = 0;

    poll.answers.forEach(function (answer) {
        tr += answer.votes;
    });

    return tr;
}

$(document).ready(function () {
    $(".PollBox").append(RadioBox('gr', 'yes', 'Yes'));
    $(".PollBox").append(RadioBox('gr', 'no', 'No'));
    $(".PollBox").append(RadioBox('gr', 'maybe', 'Maybe So'));
    $(".PollBox").append('<div class="SubmitButton">Submit!</div>');

    $(".PollBox").children('.SubmitButton').on('click', function () {
        API('add_poll', { PollQuestion: "How large is Mark's vagina?", Answers: "Too small, Too big, I mean he is a giant cunt." }, function () {

        });
    });

    $("#PollList").append(GenVoteBox(DummyPoll));
    $("#PollList").append(GenResultsBox(DummyPoll));
});

function API(action, data, c) {
    $.post('/PollAPI.aspx', { action: action, data: data }, function (data, error) {
        console.log(data);
        console.log(error);
    });
}