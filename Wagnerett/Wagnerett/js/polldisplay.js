String.prototype.hexEncode = function () {
    var hex, i;

    var result = "";
    for (i = 0; i < this.length; i++) {
        hex = this.charCodeAt(i).toString(16);
        result += ("000" + hex).slice(-4);
    }

    return result
};

function GenVoteBox(poll) {
    var tr = $('<div class="PollBox">');

    tr.append($('<div class="Question">').append(poll.PollQuestion));

    console.log(poll);

    poll.Answers.forEach(function (answer) {
        tr.append(RadioBox(poll.PollID, answer.ID, answer.Text));
    });

    var sb = $('<div class="SubmitButton">Submit!</div>');
    tr.append(sb);

    sb.on('click', function () {
        var par = {
            PollID: poll.PollID,
            AnswerID: RadioSelected(poll.PollID)
        };

        if (par.AnswerID == null) {
            alert('Please select a response.');
        }
        else {
            API('vote', par, function (data, error) {
                var vp = Cookies.getJSON('votedPolls');
                if (vp == undefined || vp == null)
                    vp = {};

                vp[par.PollID] = true;

                Cookies.set('votedPolls', JSON.stringify(vp), { expires: 90 });
                
                API('get_poll', { PollID: par.PollID }, function (data) {
                    tr.replaceWith(GenResultsBox(data.Poll));
                });
            });
        }
    });

    return tr;
};

function GenResultsBox(poll) {
    var tr = $('<div class="PollBox">');
    var tv = SumVotes(poll);

    tr.append($('<div class="Question">').append(poll.PollQuestion));

    poll.Answers.forEach(function (answer) {
        var ab = $('<div class="Answer">');
        var rb = $('<div class="RadioBox">');
        var lb = $('<div class="Label">');

        var per = Math.floor((answer.Votes / tv) * 100.0);
        if (tv <= 0)
            per = 0.0;

        lb.append(answer.Text);

        var pb = $('<div class="Percent">');
        pb.text(per + "%");

        var p = answer.Votes == 1;

        var tb = $('<div class="Tally">');
        tb.append(answer.Votes + (p ? " vote" : " votes"));

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

function GenNewPollBox() {
    var tr = $('<div class="PollBox">');

    var qBox = $('<input type="text" class="Question" placeholder="Question" />');
    qBox.css({
        background: 'none',
        border: 'none',
        outline: 'none',

        width: '100%',
        boxSizing: 'border-box',

        margin: '0',
        marginBottom: '20px',
        padding: '5px',
        paddingLeft: '10px',
        paddingRight: '10px',
        borderRadius: '3px',

        backgroundColor: '#444'
    });


    tr.append(qBox);
    tr.append('<div class="AnswerBox"></div>');
    addAnswerBox();

    tr.append('<div class="addBtnHolder"><div class="btnAddAnswer noselect">Add Answer</div></div>');
    tr.append('<div class="SubmitButton">Submit!</div>');

    tr.children('.addBtnHolder').children('.btnAddAnswer').on('click', addAnswerBox);
    tr.children('.SubmitButton').on('click', function () {
        var obj = {};
        obj.PollQuestion = tr.children('.Question').val();
        obj.Answers = [];

        var asrs = tr.children('.AnswerBox').children();

        for (var i = 0; i < asrs.length; i++) {
            console.log($(asrs[i]).children('.RadioBox'));
            obj.Answers.push($(asrs[i]).children('.RadioBox').children('.Label').val().hexEncode());
        }

        console.log(obj);
    
        API('add_poll', obj, function (data, error) {
            console.log(data.data);
            API('get_poll', { PollID: data.data.PollID }, function (data) {
                console.log(data);

                tr.remove();

                $('#HeaderDivider').after(GenResultsBox(data.Poll));

                //$("#PollList").append(GenResultsBox(data.Poll));
                //tr.replaceWith(GenResultsBox(data.Poll));

            });

            tr.replaceWith();
        });
    });

    return tr;

    function addAnswerBox() {
        var ab = $('<div class="Answer">');
        var rb = $('<div class="RadioBox" style="position: relative;">');
        var lb = $('<input type="text" class="Label" placeholder="Answer" />');
        var db = $('<div class="btnDelete"><div class="delMin"></div></div>')

        rb.append(lb);
        rb.append(db);
        ab.append(rb);

        //tr.append(ab);

        ab.css({
            position: 'relative',
            opacity: 0.0,
            marginTop: '-30px',
            zIndex: 0
        });

        ab.animate({
            opacity: 1.0,
            marginTop: '0',
            zIndex: 10
        }, 500);

        tr.children('.AnswerBox').append(ab);
        lb.focus();
        
        lb.css({
            background: 'none',
            border: 'none',
            outline: 'none',

            padding: '5px',

            backgroundColor: '#444',
            color: 'white',
            borderRadius: '4px',

            fontFamily: 'ComicNeueBold'
        });

        db.on('click', function () {
            console.log($(this).parent().children('input').val());

            var cc = $(this).parent().parent().parent().children().length;

            $(this).parent().parent().animate({
                marginBottom: '-25px',
                opacity: 0.0
            }, {
                duration: 500,
                complete: function () {
                    $(this).remove();

                    if (cc <= 1)
                        addAnswerBox();
                }
            });
        });

        lb.on('keydown', function (e) {
            var keyCode = e.keyCode || e.which;

            if (keyCode == 9) {
                e.preventDefault();
                
                if ($(this).val().length > 0)
                    addAnswerBox();
            }
        });
    }
};

function SumVotes(poll) {
    var tr = 0;

    poll.Answers.forEach(function (answer) {
        tr += answer.Votes;
    });

    return tr;
}

function GenAddBox() {
    var tr = $('<div id="AddBox" class="PollBox">');
    var ab = $('<div id="btnAdd">New Poll</div>');
    
    tr.append(ab);
    tr.css({
    });

    return tr;
}

function GenDivider() {
    var tr = $('<div id="HeaderDivider" class="PollBox">');
    tr.css({
        'margin-bottom': '20px'
    });

    return tr;
}

$(document).ready(function () {
    var div = GenDivider();
    $("#PollList").append(div);

    $("#btnAdd").on('click', function () {
        div.after(GenNewPollBox());
    });

    var par = {
        PollCount: 50
    };

    API('get_poll_history', par, function (data, error) {
        var polls = data.Polls.reverse();

        console.log(polls);

        var vp = Cookies.getJSON('votedPolls');
        if (vp == undefined || vp == null)
            vp = {};

        polls.forEach(function (poll) {
            div.after(((poll.PollID in vp) ? GenResultsBox : GenVoteBox)(poll));
        });
    });
});

function API(action, data, c) {
    $.post('/PollAPI.aspx', { action: action, data: data }, function (data, error) {
        c(data, error);
    });
}